using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedSettingsLib.Extensions
{
  public static class LogExtension
  {
    public static void Info(this Serilog.ILogger _log, string message)
    {
      _log.Information(message);
    }

    public static void Info(this Serilog.ILogger _log, Exception exception, string message)
    {
      _log.Information(exception, message);
    }
  }
}
