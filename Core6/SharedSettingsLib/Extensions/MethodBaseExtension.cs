using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharedSettingsLib.Extensions
{
  public static class MethodBaseExtension
  {
    public static string GetMethodInfo(this MethodBase? methodBase)
    {
      return $"{methodBase?.ReflectedType?.Namespace}.{methodBase?.ReflectedType?.Name}.{methodBase?.Name}";
    }
  }
}
