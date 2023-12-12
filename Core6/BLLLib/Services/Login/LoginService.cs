using DBLib.EF_BlogDB;
using SharedSettingsLib.Attributes;
using SharedSettingsLib.Models.DB;
using SharedSettingsLib.Models.Login;
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
    LoginResult GetLoginResult(string userID);
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
    public LoginResult GetLoginResult(string userID)
    {
      var result = new LoginResult();
      try
      {
        var userData = DBContext.Users.Where(u => u.UserID == userID).ToList();
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
        var methodInfo = $"{MethodInfo.GetCurrentMethod()?.ReflectedType?.Namespace}.{MethodInfo.GetCurrentMethod()?.ReflectedType?.Name}.{MethodInfo.GetCurrentMethod()?.Name}()";
        Log.Error(ex, $"{methodInfo}: {ex.GetType().Name}: {ex.Message}");
        throw;
      }
      return result;
    }
  }
}
