using Microsoft.Extensions.Configuration;
using SharedSettingsLib.Secrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedSettingsLib
{
  public class AppSettings
  {

    public IConfigurationRoot? appsettings { get; set; }
    //DB
    public static string ConnectionStrings => nameof(ConnectionStrings);
    public static string ConnectionString => nameof(ConnectionString);
    public static string DataSource => nameof(DataSource);
    public static string InitialCatalog => nameof(InitialCatalog);
    public static string UserID => nameof(UserID);
    public static string Password => nameof(Password);
    //JWT
    public static string JwtSettings => nameof(JwtSettings);
    public static string Secret => nameof(Secret);
    public static string SignKey => nameof(SignKey);
    public static string Issuer => nameof(Issuer);
    #region JWT
    public static string JwtSettingsSignKey(IConfigurationRoot? appsettings)
    {
      var value = appsettings.GetValue<string>($"{JwtSettings}:{SignKey}") ?? "";
      var Decrypt = CryptographyContext.Decrypt(value);
      return string.IsNullOrEmpty(Decrypt) ? value : Decrypt;
    }
    public static string SecretSignKey(IConfigurationRoot? appsettings)
    {
      return JwtSettingsSignKey(appsettings);
    }
    #endregion
    #region DBConnection
    public static string DefaultConnectionString(IConfigurationRoot? appsettings)
    {
      var value = appsettings.GetValue<string>($"{ConnectionStrings}:{ConnectionString}");
      var Decrypt = CryptographyContext.Decrypt(value, SecretSignKey(appsettings));
      var dataSource = DefaultConnectionDataSource(appsettings);
      var initialCatalog = DefaultConnectionInitialCatalog(appsettings);
      var userID = DefaultConnectionUserID(appsettings);
      var password = DefaultConnectionPassword(appsettings);
      return (string.IsNullOrEmpty(Decrypt) ? value : Decrypt)
        .Replace($"data source={DataSource};", $"data source={dataSource};")
        .Replace($"initial catalog={InitialCatalog};", $"initial catalog={initialCatalog};")
        .Replace($"user id={UserID};", $"user id={userID};")
        .Replace($"password={Password};", $"password={password};")
        ;
    }
    public static string DefaultConnectionDataSource(IConfigurationRoot? appsettings)
    {
      var result = appsettings?.GetValue<string>($"{ConnectionStrings}:{DataSource}") ?? "";
      var decrypted = CryptographyContext.Decrypt(result, SecretSignKey(appsettings));
      return string.IsNullOrEmpty(decrypted) ? result : decrypted;
    }
    public static string DefaultConnectionInitialCatalog(IConfigurationRoot? appsettings)
    {
      var result = appsettings?.GetValue<string>($"{ConnectionStrings}:{InitialCatalog}") ?? "";
      var decrypted = CryptographyContext.Decrypt(result, SecretSignKey(appsettings));
      return string.IsNullOrEmpty(decrypted) ? result : decrypted;
    }
    public static string DefaultConnectionUserID(IConfigurationRoot? appsettings)
    {
      var result = appsettings?.GetValue<string>($"{ConnectionStrings}:{UserID}") ?? "";
      var decrypted = CryptographyContext.Decrypt(result, SecretSignKey(appsettings));
      return string.IsNullOrEmpty(decrypted) ? result : decrypted;
    }
    public static string DefaultConnectionPassword(IConfigurationRoot? appsettings)
    {
      var result = appsettings?.GetValue<string>($"{ConnectionStrings}:{Password}") ?? "";
      var decrypted = CryptographyContext.Decrypt(result, SecretSignKey(appsettings));
      return string.IsNullOrEmpty(decrypted) ? result : decrypted;
    }
    #endregion
  }
}
