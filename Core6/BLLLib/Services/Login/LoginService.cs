using DBLib.EF_BlogDB;
using SharedSettingsLib.Attributes;
using SharedSettingsLib.Models.DB;
using SharedSettingsLib.Models.Login;
using SharedSettingsLib.Secrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BLLLib.Services.Login
{
  public interface ILoginService
  {
    LoginResult GetLoginResult(LoginQuery login);
  }
  [InjectScoped]
  public class LoginService : ILoginService
  {
    public LoginService(BlogDBContext dbContext,
      Serilog.ILogger log)
    {
      DBContext = dbContext;
      Log = log;
    }
    public BlogDBContext DBContext { get; }
    public Serilog.ILogger Log { get; }
    public LoginResult GetLoginResult(LoginQuery login)
    {
      var result = new LoginResult();
      try
      {
        var encryptPassword = CryptographyContext.Encrypt(login.PassWord!);
        var userData = DBContext.Users.Where(u => u.UserID == login.UserID && u.Password == encryptPassword).ToList();
        if (userData.Count != 1)
        {
          throw new Exception("user資料異常");
        }
        result.UserData = new UsersResult
        {
          LogonTime = DateTime.Now,
          UpdateTime = DateTime.Now,
          UpdateUser = nameof(LoginService),
          UserEmail = userData.FirstOrDefault()!.UserEmail,
          UserID = userData.FirstOrDefault()!.UserID,
          UserMobile = userData.FirstOrDefault()!.UserMobile,
          UserName = userData.FirstOrDefault()!.UserName
        };
      }
      catch (Exception ex)
      {
        var methodInfo = $"{MethodBase.GetCurrentMethod()?.ReflectedType?.Namespace}.{MethodBase.GetCurrentMethod()?.ReflectedType?.Name}.{MethodBase.GetCurrentMethod()?.Name}()";
        Log.Error(ex, $"{methodInfo}: {ex.GetType().Name}: {ex.Message}");
        throw;
      }
      return result;
    }
  }
}
