using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedSettingsLib.Extensions
{
  public static class DateTimeExtension
  {
    public static string yyyy_MM_dd => "yyyy-MM-dd";
    public static string yyyy_MM_dd_HH_mm_ss_fff => "yyyy-MM-dd HH:mm:ss.fff";


    public static string ToString_yyyy_MM_dd(this DateTime _DateTime)
    {
      return _DateTime.ToString(yyyy_MM_dd);
    }
    public static string ToString_yyyy_MM_dd_HH_mm_ss_fff(this DateTime _DateTime)
    {
      return _DateTime.ToString(yyyy_MM_dd_HH_mm_ss_fff);
    }

    public static string ToString_yyyy_MM_dd_HH_mm_ss_fff(this DateTime? _DateTime)
    {
      return (_DateTime.HasValue) ? _DateTime.Value.ToString(yyyy_MM_dd_HH_mm_ss_fff) : "";
    }

  }
}
