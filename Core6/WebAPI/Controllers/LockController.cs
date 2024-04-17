using BLLLib.Services;
using BLLLib.Services.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedSettingsLib.Models.DB;
using SharedSettingsLib.Models.Public;

namespace WebAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class LockController : ControllerBase
  {
    public LockController(ILockService lockService)
    {
      LockService = lockService;
    }
    ILockService LockService { get; set; }

    [HttpGet]
    public IActionResult LockDemo()
    {
      LockService.Start();
      return Ok();
    }
  }
}
