global using Serilog;
using Microsoft.OpenApi.Models;
using WebAPI;

var builder = WebApplication.CreateBuilder(args);

var appsettings = new ConfigurationBuilder()
  .SetBasePath(builder.Environment.ContentRootPath)
  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
  .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
  .AddEnvironmentVariables()
  .Build();

Log.Logger = new LoggerConfiguration()
  .MinimumLevel.Debug()
  .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
  .Enrich.FromLogContext()
  .ReadFrom.Configuration(appsettings)
  .WriteTo.Console()
  .CreateLogger();

// Add services to the container.
DiServices.RegisterTypes(builder, appsettings);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMvc(options =>
{
  options.Filters.Add(typeof(WebAPI.Filter.AuthorizationFilter));
  options.Filters.Add(typeof(WebAPI.Filter.ExceptionHandledActionFilter));  
});

//.AddJsonOptions(options =>
//{
//  options.JsonSerializerOptions.Converters.Add(new JsonLocalDateTimeConverter());
//  options.JsonSerializerOptions.PropertyNamingPolicy = null; // web api 輸出的 JSON 不要自動轉換為字首小寫 (預設為 PascalCase) _
//   options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
//  options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(
//    UnicodeRanges.All, UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs); // 中文編碼 _
// });
  builder.Services.AddSwaggerGen(op => {
    //讓 Swagger 可以使用 JWT 驗證執行 Web API 
    //op.OperationFilter<SWIP_WebApi.SwaggerCustomHeaderFilter>(); //加入自訂 header `urlrouting` 
    op.SwaggerDoc("v1", new OpenApiInfo { 
      Title = "WebApi", Version = "v1" 
    });
    op.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
      Name = "Authorization",
      In = ParameterLocation.Header,
      Type = SecuritySchemeType.Http,
      //Type = SecuritySchemeType.ApiKey,
      Scheme = "Bearer",
      BearerFormat = "JWT",
      Description = "", //"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJzd2FnZ2VyIiwianRpIjoiYTJjMjk1ZDYtNmE1OS00MjM2LTk1MmUtYzFjZTYxMmFiZmIxIiwicm9sZXMiOiJ1c2VyIiwibmJmIjoxNjU1NzgxNjQzLCJleHAiOjE2NTU3ODM0NDMsImlhdCI6MTY1NTc4MTY0MywiaXNzIjoiU3dpcFdlYkFwaUp3dCJ9.PC7TaBx5C1KZD18GyGH4B2XhB1AGsyyWb_lAvgyZ9ak", // id `swagger` token 
    });
    op.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
      {
        new OpenApiSecurityScheme
        {
          Reference = new OpenApiReference
          {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
          }
        },
        new string[] {}
      }
    });
  });
var app = builder.Build();

var AppSettings = app.Services.GetService<SharedSettingsLib.AppSettings>();
AppSettings!.appsettings = appsettings;
//AppSettings!.InitDefaultSettings(); 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
