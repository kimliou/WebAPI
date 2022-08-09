using SharedSettingsLib.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLLLib.Services
{
  public interface ITestDIService
  {
    public string GetResult();
  }
  [Inject]
  public class TestDIService: ITestDIService
  {
    public string GetResult()
    {
      return "成功";
    }
  }
}
