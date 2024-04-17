using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedSettingsLib.Extensions;

public static partial class DictionaryExtension
{
  public static string? GetValue(this IDictionary<string, object?>? fieldValues, string key)
  {
    if (fieldValues is null || string.IsNullOrEmpty(key)) { return null; }
    if (fieldValues.ContainsKey(key))
    {
      return fieldValues[key] is null ? null : $"{fieldValues[key]}";
    }
    return null;
  }

  public static string? SetValue(this IDictionary<string, object?>? fieldValues, string key, string value)
  {
    if (fieldValues is null || string.IsNullOrEmpty(key)) { return null; }
    if (fieldValues.ContainsKey(key))
    {
      fieldValues.Remove(key);
    }
    fieldValues.Add(key, value);

    return fieldValues.GetValue(key);
  }
}
