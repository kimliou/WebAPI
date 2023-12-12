using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using SharedSettingsLib.Attributes;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace WebAPI
{
  public class DiServices
  {
    //public static void RegisterTypes(WebApplicationBuilder bu, IConfigurationRoot? appsettings)
    public static void RegisterTypes(WebApplicationBuilder bu, IConfigurationRoot? appsettings)
    {
      //連線字串
      var connectionString = SharedSettingsLib.AppSettings.DefaultConnectionString(appsettings);
      var jwtSettingsSignKey = SharedSettingsLib.AppSettings.JwtSettingsSignKey(appsettings);
      var jwtSettingsIssuer = SharedSettingsLib.AppSettings.JwtSettingsIssuer(appsettings);
      var se = bu.Services;
      se.AddSingleton<Serilog.ILogger>(Log.Logger);
      //EntityDBContext 
      se.AddDbContext<DBLib.EF_BlogDB.BlogDBContext>(dbContextOptionsBuilder =>
        {
           dbContextOptionsBuilder.UseSqlServer(connectionString); // get connectionString settings from appsettings.json _
                                                                   // dbContextOptionsBuilder.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());//EnableRetryOnFailure 自動重試失敗的資料庫命令 
           dbContextOptionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(SqlServerEventId.DecimalTypeKeyWarning)
            // 忽略程式啟動時的 Entity Framework 警告 warn: Microsoft.EntityFrameworkCore.Model.Validation[30003] The decimal property 'SEQ_NO' is part of a key on entity type 'SYS_CD' ...
            );
        },
      ServiceLifetime.Transient, ServiceLifetime.Singleton);

      se.AddHttpClient();
      #region ` JWT `
      se
      .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      // .AddAuthentication(options => { options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; })
      .AddJwtBearer(options =>
      {
        // 當驗證失敗時，回應標頭會包含 WWW-Authenticate 標頭，這裡會顯示失敗的詳細錯誤原因
        options.IncludeErrorDetails = true; // 預設值為 true，有時會特別關閉

        options.TokenValidationParameters = new TokenValidationParameters
        {
          // 透過這項宣告，就可以從 "sub" 取值並設定給 User.Identity.Name
          NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
          // 透過這項宣告，就可以從 "roles" 取值，並可讓 [Authorize] 判斷角色
          RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",

          // 一般我們都會驗證 Issuer
          ValidateIssuer = true,
          ValidIssuer = jwtSettingsIssuer, // settings.GetJwtSettingsIssuer(),
                                           // 通常不太需要驗證 Audience
          ValidateAudience = false,
          //ValidAudience = "JwtAuthDemo", // 不驗證就不需要填寫

          // 一般我們都會驗證 Token 的有效期間
          ValidateLifetime = true,

          // 如果 Token 中包含 key 才需要驗證，一般都只有簽章而已
          ValidateIssuerSigningKey = false,

          // "1234567890123456" 應該從 IConfiguration 取得
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettingsSignKey))
        };
      });
      #endregion
      se.AddScoped<SqlConnection>();
      se.AddScoped<DBLib.IApplicationDbContext, DBLib.ApplicationDbContext>();
      try
      {
        //需要去掃的專案名稱
        $"{nameof(WebAPI)},{nameof(SharedSettingsLib)},{nameof(BLLLib)},{nameof(DBLib)}"
        .Trim().Split(',').ToList().Where(get => !string.IsNullOrEmpty(get)).ToList().ForEach(_namespace =>
        {
          // Singleton
          Assembly.Load(_namespace).GetTypes()
            .Where(type1 => !type1.IsInterface)
            //掃程式裡面標籤有使用InjectSingleton
            .Where(type2 => type2.GetCustomAttributes().Contains(new InjectSingleton()))
            .ToList().ForEach(implementationType =>
            {
              var serviceType =
              Assembly.Load(_namespace).GetTypes().FirstOrDefault(
                _interfaceType => _interfaceType.Name.Equals(implementationType.GetInterfaces().FirstOrDefault()?.Name, StringComparison.Ordinal))
              ?? implementationType;
              se.AddSingleton(serviceType, implementationType);
            });
          // Scoped
          Assembly.Load(_namespace).GetTypes()
            .Where(type1 => !type1.IsInterface)
            .Where(type2 =>
            //掃程式裡面標籤有使用Inject or InjectScoped or 名稱是_Table結尾的
              type2.GetCustomAttributes().Contains(new Inject())
              || type2.GetCustomAttributes().Contains(new InjectScoped())
              || type2.Name.EndsWith("_Table", StringComparison.Ordinal)
            )
            .ToList().ForEach(implementationType =>
            {
              var serviceType =
              Assembly.Load(_namespace).GetTypes().FirstOrDefault(
                _interfaceType => _interfaceType.Name.Equals(implementationType.GetInterfaces().FirstOrDefault()?.Name, StringComparison.Ordinal))
              ?? implementationType;
              se.AddScoped(serviceType, implementationType);
            });
          //Transient
          Assembly.Load(_namespace).GetTypes()
            .Where(type1 => !type1.IsInterface)
            .Where(type2 =>
              type2.GetCustomAttributes().Contains(new InjectTransient())
            )
            .ToList().ForEach(implementationType =>
            {
              var serviceType =
              Assembly.Load(_namespace).GetTypes().FirstOrDefault(
                _interfaceType => _interfaceType.Name.Equals(implementationType.GetInterfaces().FirstOrDefault()?.Name, StringComparison.Ordinal))
              ?? implementationType;
              se.AddTransient(serviceType, implementationType);
            });
        });
      }
      catch (Exception)
      {
        //Log
      }
    }
  }
}
