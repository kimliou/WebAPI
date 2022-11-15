using BLLLib.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]

  public class TestDIController : ControllerBase
  {
    private ITestDIService _testDIService { get; }

    public TestDIController(ITestDIService testDIService)
    {
      _testDIService = testDIService;
    }

    // GET: api/<TestDIController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
      var getResult = _testDIService.GetResult();
      return new string[] { getResult };
    }
  }
}
