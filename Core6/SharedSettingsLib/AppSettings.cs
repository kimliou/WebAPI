using Microsoft.Extensions.Configuration;
using SharedSettingsLib.Attributes;
using SharedSettingsLib.Secrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedSettingsLib
{
  [InjectSingleton]
  public partial class AppSettings
  {
    public IConfigurationRoot? appsettings { get; set; }
    //DB
    public static string ConnectionStrings => nameof(ConnectionStrings);
    public static string DefaultConnection => nameof(DefaultConnection);
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
    public static string JwtSettingsIssuer(IConfigurationRoot? appsettings)
    {
      var value = appsettings.GetValue<string>($"{JwtSettings}:{Issuer}");
      var Decrypt = CryptographyContext.Decrypt(value, SecretSignKey(appsettings));
      return string.IsNullOrEmpty(Decrypt) ? value : Decrypt;
    }
    public string GetSecretSignKey()
    {
      return JwtSettingsSignKey(appsettings);
    }
    public static string SecretSignKey(IConfigurationRoot? appsettings)
    {
      return JwtSettingsSignKey(appsettings);
    }
    public string GetJwtSettingsIssuer()
    {
      return JwtSettingsIssuer(appsettings);
    }

    public string GetJwtSettingsSignKey()
    {
      return JwtSettingsSignKey(appsettings);
    }
    public static string ExpireMinutes => nameof(ExpireMinutes);
    public int GetJwtExpireMinutes()
    {
      int result = appsettings?.GetValue<int>($"{JwtSettings}:{ExpireMinutes}") ?? 30;
      return (result > 0) ? result : 30;
    }
    #endregion
    #region DBConnection
    public string GetDefaultConnectionString()
    {
      //目前使用localDB Windows驗證，當需要使用帳號密碼跟加密在把下面那段打開。
      var value = appsettings.GetValue<string>($"{ConnectionStrings}:{DefaultConnection}:{ConnectionString}");
      //var Decrypt = CryptographyContext.Decrypt(value, GetSecretSignKey());
      //var dataSource = GetDefaultConnectionDataSource();
      //var initialCatalog = GetDefaultConnectionInitialCatalog();
      //var userID = GetDefaultConnectionUserID();
      //var password = GetDefaultConnectionPassword();
      //return (string.IsNullOrEmpty(Decrypt) ? value : Decrypt)
      //  .Replace($"data source={DataSource};", $"data source={dataSource};")
      //  .Replace($"initial catalog={InitialCatalog};", $"initial catalog={initialCatalog};")
      //  .Replace($"user id={UserID};", $"user id={userID};")
      //  .Replace($"password={Password};", $"password={password};")
      //  ;

      return value;
    }

    public static string DefaultConnectionString(IConfigurationRoot? appsettings)
    {
      //目前使用localDB Windows驗證，當需要使用帳號密碼跟加密在把下面那段打開。
      var value = appsettings.GetValue<string>($"{ConnectionStrings}:{DefaultConnection}:{ConnectionString}");

      //var Decrypt = CryptographyContext.Decrypt(value, SecretSignKey(appsettings));
      //var dataSource = DefaultConnectionDataSource(appsettings);
      //var initialCatalog = DefaultConnectionInitialCatalog(appsettings);
      //var userID = DefaultConnectionUserID(appsettings);
      //var password = DefaultConnectionPassword(appsettings);
      //return (string.IsNullOrEmpty(Decrypt) ? value : Decrypt)
      //  .Replace($"data source={DataSource};", $"data source={dataSource};")
      //  .Replace($"initial catalog={InitialCatalog};", $"initial catalog={initialCatalog};")
      //  .Replace($"user id={UserID};", $"user id={userID};")
      //  .Replace($"password={Password};", $"password={password};");
      return value;
    }
    public string GetDefaultConnectionDataSource()
    {
      var result = appsettings?.GetValue<string>($"{ConnectionStrings}:{DefaultConnection}:{DataSource}") ?? "";
      var decrypted = CryptographyContext.Decrypt(result, SecretSignKey(appsettings));
      return string.IsNullOrEmpty(decrypted) ? result : decrypted;
    }
    public static string DefaultConnectionDataSource(IConfigurationRoot? appsettings)
    {
      var result = appsettings?.GetValue<string>($"{ConnectionStrings}:{DefaultConnection}:{DataSource}") ?? "";
      var decrypted = CryptographyContext.Decrypt(result, SecretSignKey(appsettings));
      return string.IsNullOrEmpty(decrypted) ? result : decrypted;
    }

    public string GetDefaultConnectionInitialCatalog()
    {
      var result = appsettings?.GetValue<string>($"{ConnectionStrings}:{DefaultConnection}:{InitialCatalog}") ?? "";
      var decrypted = CryptographyContext.Decrypt(result, SecretSignKey(appsettings));
      return string.IsNullOrEmpty(decrypted) ? result : decrypted;
    }
    public static string DefaultConnectionInitialCatalog(IConfigurationRoot? appsettings)
    {
      var result = appsettings?.GetValue<string>($"{ConnectionStrings}:{DefaultConnection}:{InitialCatalog}") ?? "";
      var decrypted = CryptographyContext.Decrypt(result, SecretSignKey(appsettings));
      return string.IsNullOrEmpty(decrypted) ? result : decrypted;
    }
    public string GetDefaultConnectionUserID()
    {
      var result = appsettings?.GetValue<string>($"{ConnectionStrings}:{DefaultConnection}:{UserID}") ?? "";
      var decrypted = CryptographyContext.Decrypt(result, SecretSignKey(appsettings));
      return string.IsNullOrEmpty(decrypted) ? result : decrypted;
    }
    public static string DefaultConnectionUserID(IConfigurationRoot? appsettings)
    {
      var result = appsettings?.GetValue<string>($"{ConnectionStrings}:{DefaultConnection}:{UserID}") ?? "";
      var decrypted = CryptographyContext.Decrypt(result, SecretSignKey(appsettings));
      return string.IsNullOrEmpty(decrypted) ? result : decrypted;
    }
    public string GetDefaultConnectionPassword()
    {
      var result = appsettings?.GetValue<string>($"{ConnectionStrings}:{DefaultConnection}:{Password}") ?? "";
      var decrypted = CryptographyContext.Decrypt(result, SecretSignKey(appsettings));
      return string.IsNullOrEmpty(decrypted) ? result : decrypted;
    }
    public static string DefaultConnectionPassword(IConfigurationRoot? appsettings)
    {
      var result = appsettings?.GetValue<string>($"{ConnectionStrings}:{DefaultConnection}:{Password}") ?? "";
      var decrypted = CryptographyContext.Decrypt(result, SecretSignKey(appsettings));
      return string.IsNullOrEmpty(decrypted) ? result : decrypted;
    }
    #endregion
  }
}
