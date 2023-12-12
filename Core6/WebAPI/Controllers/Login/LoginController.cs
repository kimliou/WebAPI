using BLLLib.Services.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedSettingsLib.Models.Login;

namespace WebAPI.Controllers.Login
{
  [Route("api/[controller]")]
  [ApiController]
  public class LoginController : ControllerBase
  {
    public LoginController(ILoginService loginService)
    {
      LoginService = loginService;
    }
    public ILoginService LoginService { get; }
    [AllowAnonymous]
    [HttpPost]
    public ActionResult<LoginResult> Login(LoginQuery login)
    {
      return LoginService.GetLoginResult(login.UserID!);
    }
  }
}
