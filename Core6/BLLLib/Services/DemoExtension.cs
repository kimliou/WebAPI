using SharedSettingsLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLLLib.Services
{
  public class DemoExtension
  {
    public void Demo()
    {
      //decimal Extension
      decimal? haszero = (decimal)12.0000000000;
      var result = haszero.DecimalNotZero();
      var result1 = haszero.DecimalNotZeroToString();

      //日期Extension
      var date = DateTime.Now;
      var dateString = date.ToString_yyyy_MM_dd();

      //物件Extension
      List<string> list = new List<string>();
      bool listbool  =list.IsPropertiesNullOrEmpty().isNull;
      string liststring =  list.IsPropertiesNullOrEmpty().message;
    }
  }
}
