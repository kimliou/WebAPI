using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedSettingsLib.Extensions
{
  public static class StringExtension
  {
    /// <summary>
    /// Trim。若來源null回傳null
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string? StringTrim(this string? input)
    {
      return string.IsNullOrEmpty(input) ? null : input.Trim();
    }
    /// <summary>
    /// 如果轉型失敗回傳null
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static decimal? ToDecimal(this string? value)
    {
      Decimal? result = null;
      try
      {
        result = Convert.ToDecimal(value);
      }
      catch (Exception)
      {
        result = null;
      }
      return result;
    }

    public static int? ToInt16(this string? value)
    {
      return string.IsNullOrEmpty(value) ? null : Convert.ToInt16(value);
    }
  }
}
