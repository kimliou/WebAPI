using Microsoft.EntityFrameworkCore;
using SharedSettingsLib.Attributes;
using System.Data.SqlClient;
using System.Reflection;

namespace WebAPI
{
  public class DiServices
  {
    //public static void RegisterTypes(WebApplicationBuilder bu, IConfigurationRoot? appsettings)
    public static void RegisterTypes(WebApplicationBuilder bu, IConfigurationRoot? appsettings)
    {
      //連線字串
      var connectionString = SharedSettingsLib.AppSettings.DefaultConnectionString(appsettings);
      var se = bu.Services;
      ////EntityDBContext 
//      se.AddDbContext<SWIP_CTBCSB_AP_DbLib.EF_CTBCSB_AP.CTBCSBAPContext>(dbContextOptionsBuilder =>
//      {
//        dbContextOptionsBuilder.UseSqlServer(connectionString); // 220704, Morgan: get connectionString settings from appsettings.json _
//                                                                // dbContextOptionsBuilder.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());// 221214, Morgan: EnableRetryOnFailure 自動重試失敗的資料庫命令 
//        dbContextOptionsBuilder.ConfigureWarnings(warnings =>
//          warnings.Ignore(SqlServerEventId.DecimalTypeKeyWarning)
//        // 忽略程式啟動時的 Entity Framework 警告 warn: Microsoft.EntityFrameworkCore.Model.Validation[30003] The decimal property 'SEQ_NO' is part of a key on entity type 'SYS_CD' ...
//        );
//      },
//ServiceLifetime.Transient, ServiceLifetime.Singleton);

      se.AddHttpClient(); 
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
