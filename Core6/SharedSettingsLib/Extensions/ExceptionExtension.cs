using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedSettingsLib.Extensions
{
  public static class ExceptionExtension
  {
    /// <summary>
    /// 簡化 Serilog.Log.Logger.Error
    /// // MethodBase.GetCurrentMethod().GetMethodInfo()
    /// </summary>
    /// <param name="ex"></param>
    /// <param name="methodInfo"></param>
    public static void LogError(this Exception ex, string methodInfo)
    {
      if (ex != null)
      {
        if (ex.InnerException != null)
        {
          Serilog.Log.Logger.Error(ex, $"{methodInfo}: {ex.InnerException.GetType().Name}: {ex.InnerException.Message}");
        }
        Serilog.Log.Logger.Error(ex, $"{methodInfo}: {ex.GetType().Name}: {ex.Message}");
      }
    }

  }
}
