using System;
using System.Collections.Generic;
using System.Globalization;
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
    public static string ToString_DateFormat(this DateTime _DateTime, string Format)
    {
      //判斷是否為民國年，決定要不要讀取日曆套件
      int FormatCount = Format.Count(f => f == 'y');
      if (FormatCount == 3)
      {
        CultureInfo culture = new CultureInfo("zh-TW");
        culture.DateTimeFormat.Calendar = new TaiwanCalendar();
        return _DateTime.ToString(Format, culture);
      }
      else
      {
        return _DateTime.ToString(Format);
      }
    }
  }
}
