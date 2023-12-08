using Microsoft.IdentityModel.Tokens;
using SharedSettingsLib.Attributes;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace WebAPI.JWT;


[InjectScoped]
public class JwtHelpers
{
  public JwtHelpers(
    SharedSettingsLib.AppSettings appSettings,
    Serilog.ILogger log
  )
  {
    this.Log = log;
    AppSettings = appSettings;
  }
  public Serilog.ILogger Log { get; }
  public SharedSettingsLib.AppSettings AppSettings { get; }

  public string TrimAsTokenString(string input)
  {
    return input.Replace("Bearer ", "").Replace("bearer ", "").Trim();
  }

  /// <summary>
  /// 產生 JWT Token string
  /// </summary>
  /// <param name="userID">能唯一識別使用者的名稱，通常是前端使用者登入ID</param>
  /// <param name="expireMinutes">逾期時間分鐘</param>
  /// <returns></returns>
  public string GenerateToken(string userID, int expireMinutes = 30)
  {
    var result = "";
    try
    {
      var issuer = AppSettings.GetJwtSettingsIssuer(); // Configuration.GetValue<string>("JwtSettings:Issuer");
      var signKey = AppSettings.GetJwtSettingsSignKey(); // Configuration.GetValue<string>("JwtSettings:SignKey");
      var settingsExpireMinutes = AppSettings.GetJwtExpireMinutes();

      var claims = new List<Claim>
      {
        new Claim(JwtRegisteredClaimNames.Sub, userID), // `subject`: User.Identity.Name
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // `JWT ID`
      }; // 設定要加入到 JWT Token 中的聲明資訊(Claims)
      /*
      在 RFC 7519 規格中(Section#4)，總共定義了 7 個預設的 Claims5，我們應該只用的到兩種！
      claims.Add(new Claim(JwtRegisteredClaimNames.Iss, issuer));
      claims.Add(new Claim(JwtRegisteredClaimNames.Aud, "The Audience"));
      claims.Add(new Claim(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddMinutes(30).ToUnixTimeSeconds().ToString()));
      claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())); // 必須為數字
      claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())); // 必須為數字
      claims.Add(new Claim(JwtRegisteredClaimNames.NameId, userName)); // 網路上常看到的這個 NameId 設定是多餘的
      claims.Add(new Claim(ClaimTypes.Name, userName)); // 這個 Claim 也以直接被 JwtRegisteredClaimNames.Sub 取代，所以也是多餘的
      claims.Add(new Claim("roles", "admin")); // 你可以自行擴充 "roles" 加入登入者該有的角色
      */

      try
      {
        //var defaultRole = AppSettings.GetLDAP_DefaultRole() ?? "default";
        var defaultRole = "default";
        claims.Add(new Claim("roles", defaultRole)); //如果沒有角色, 則為 "default" _
      }
      catch (Exception)
      {
        //var defaultRole = AppSettings.GetLDAP_DefaultRole() ?? "default";
        var defaultRole =  "default";
        claims.Add(new Claim("roles", defaultRole)); //如果沒有角色, 則為 "default" _
      }

      var userClaimsIdentity = new ClaimsIdentity(claims);

      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signKey)); // 建立一組對稱式加密的金鑰，主要用於 JWT 簽章之用

      var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
      /*
      HmacSha256 有要求必須要大於 128 bits，所以 key 不能太短，至少要 16 字元以上
      https://stackoverflow.com/questions/47279947/idx10603-the-algorithm-hs256-requires-the-securitykey-keysize-to-be-greater
      */
      var tokenDescriptor = new SecurityTokenDescriptor // 建立 SecurityTokenDescriptor
      {
        Issuer = issuer,
        // Audience = issuer, // 由於你的 API 受眾通常沒有區分特別對象，因此通常不太需要設定，也不太需要驗證
        // NotBefore = DateTime.Now, // 預設值就是 DateTime.Now
        // IssuedAt = DateTime.Now, // 預設值就是 DateTime.Now
        Subject = userClaimsIdentity,
        Expires = DateTime.Now.AddMinutes(expireMinutes),
        SigningCredentials = signingCredentials
      };

      // 產出所需要的 JWT securityToken 物件，並取得序列化後的 Token 結果(字串格式)
      var tokenHandler = new JwtSecurityTokenHandler();
      var securityToken = tokenHandler.CreateToken(tokenDescriptor);
      var serializeToken = tokenHandler.WriteToken(securityToken);
      result = serializeToken;
    }
    catch (Exception ex)
    {
      var methodInfo = $"{MethodInfo.GetCurrentMethod()?.ReflectedType?.Namespace}.{MethodInfo.GetCurrentMethod()?.ReflectedType?.Name}.{MethodInfo.GetCurrentMethod()?.Name}()";
      Serilog.Log.Error(ex, $"{methodInfo}: {ex.GetType().Name}: {ex.Message}");
    }
    return result;
  }

  public JwtSecurityToken? JwtSecurityToken { get; set; }
  public string Token { get; set; } = "";

  public string? VerifyTokenThenGetUserID(string tokenString)
  {
    if (string.IsNullOrWhiteSpace(tokenString))
    {
      return "";
    }
    return GetTokenClaimsSubject(VerifyToken(TrimAsTokenString(tokenString))) ?? "";
  }

  /// <summary>
  /// 取得 JwtSecurityToken 內的 Claims 資料 List
  /// </summary>
  /// <param name="token"></param>
  /// <returns></returns>
  public string? GetTokenClaimsSubject(JwtSecurityToken? token)
  {
    var result = token?.Claims.FirstOrDefault(get => get.Type == JwtRegisteredClaimNames.Sub)?.Value;

    return String.IsNullOrEmpty(result) ? "" : result;
  }
  public string? GetTokenClaimsSubject()
  {
    return this.JwtSecurityToken != null ? GetTokenClaimsSubject(this.JwtSecurityToken) : "";
  }

  /// <summary>
  /// 驗證 Token ( 目前暫不驗證逾期時間 )
  /// </summary>
  /// <param name="token"></param>
  /// <param name="validateLifetime">預設 false 不驗證逾期時間</param>
  /// <param name="requireExpirationTime">預設 false 不要求 Token 的 Claims 中必須包含 Expires</param>
  /// <param name="validateAudience">預設 false 不驗證訂閱者</param>
  /// <returns></returns>
  public JwtSecurityToken? VerifyToken(string token
    , bool validateLifetime = false, bool requireExpirationTime = false, bool validateAudience = false)
  {
    //var jwtSecurityToken = new JwtSecurityToken(token);
    var validationParameters = new TokenValidationParameters()
    {
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AppSettings.GetJwtSettingsSignKey())),

      ValidateLifetime = validateLifetime, // 驗證 token 有效期 
      RequireExpirationTime = requireExpirationTime, // 否要求 Token 的 Claims 中必須包含 Expires
      // ClockSkew = TimeSpan.Zero, // 允許的服務器時間偏移量

      ValidateIssuer = true, // 是否驗證提供者
      ValidIssuer = AppSettings.GetJwtSettingsIssuer(), // 提供者

      ValidateAudience = validateAudience, // 是否驗證訂閱者
      // ValidAudience = audience, // 訂閱者
    };
    var tokenHandler = new JwtSecurityTokenHandler();
    SecurityToken? validatedToken = null;
    try
    {
      tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
    }
    catch (SecurityTokenException ex)
    {
      Log.Error(ex.Message, ex);
    }
    catch (Exception ex)
    {
      Log.Error(ex.Message, ex);
    }
    return validatedToken as JwtSecurityToken;
  }
  public JwtSecurityToken? VerifyToken()
  {
    return this.Token != null ? VerifyToken(this.Token) : null;
  }

}
/*
TEST:
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJNb3JnYW4iLCJqdGkiOiI0ZGYwMjgyYS0wYzIwLTRiMGYtOWY5NC1kZmY3Yzc3ZDJkY2EiLCJyb2xlcyI6InVzZXIiLCJuYmYiOjE2NTA5NDUzMjksImV4cCI6MTY1MDk0NzEyOSwiaWF0IjoxNjUwOTQ1MzI5LCJpc3MiOiJTd2lwV2ViQXBpSnd0In0.qKdYCiisQEqHZ--bJVtf915OU17pnpTpkaJ8CL5LkVg;

*/
