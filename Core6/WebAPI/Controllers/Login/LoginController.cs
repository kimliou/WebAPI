using BLLLib.Services.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedSettingsLib.Models.Login;
using WebAPI.JWT;

namespace WebAPI.Controllers.Login
{
  [Route("api/[controller]")]
  [ApiController]
  public class LoginController : ControllerBase
  {
    public LoginController(ILoginService loginService, JwtHelpers jwt)
    {
      LoginService = loginService;
      Jwt = jwt;
    }
    public ILoginService LoginService { get; }
    public JwtHelpers Jwt { get; }
    [AllowAnonymous]
    [HttpPost]
    public ActionResult<LoginResult> Login(LoginQuery login)
    {
      Console.WriteLine(Jwt.GenerateToken(login.UserID!));
      var loginResult = LoginService.GetLoginResult(login);
      loginResult.TokenString = Jwt.GenerateToken(login.UserID!); // 產生 token 回傳
      loginResult.Success = true;

      return loginResult;
    }
  }
}
