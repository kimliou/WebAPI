using BLLLib.Services.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedSettingsLib.Models.DB;

namespace WebAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BlogController : ControllerBase
  {
    public BlogController(IBlogServices blogServices)
    {
      _blogServices = blogServices;
    }
    private IBlogServices _blogServices { get; }
    [HttpPost]
    public BlogResult Create(BlogResult input)
    {
      return _blogServices.Create(input);
    }
  }
}
