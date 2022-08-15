using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedSettingsLib.Extensions
{
  public static class DecimalExtension
  {
    /// <summary>
    /// 去尾數0，並轉為字串
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string DecimalNotZeroToString(this decimal? value)
    {
      return value.HasValue ? (value / 1.000000000000000000000000000000000m).ToString()! : "";
    }
    /// <summary>
    /// 去0
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static decimal? DecimalNotZero (this decimal? value)
    {
      return value.HasValue ? (value / 1.000000000000000000000000000000000m) : null;
    }
  }
}
