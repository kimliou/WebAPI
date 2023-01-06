using WebAPI;

var builder = WebApplication.CreateBuilder(args);

var appsettings = new ConfigurationBuilder()
  .SetBasePath(builder.Environment.ContentRootPath)
  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
  .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
  .AddEnvironmentVariables()
  .Build();

// Add services to the container.
DiServices.RegisterTypes(builder, appsettings);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
