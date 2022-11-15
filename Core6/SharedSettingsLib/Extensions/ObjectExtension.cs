using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedSettingsLib.Extensions
{
  public static class ObjectExtension
  {
    /// <summary>
    /// 物件版IsNullOrEmpty
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public static (bool isNull, string message) IsPropertiesNullOrEmpty(this object model)
    {
      var sb = new StringBuilder();
      var result = true; // 假設所有屬性都是 null 或空字串 
      if (model == null)
      {
        return (true, "");
      }
      model.GetType().GetProperties().ToList().ForEach(p =>
      {
        var value = p.GetValue(model);
        if (value != null && !string.IsNullOrEmpty(value?.ToString()))
        {
          result = false;
          sb.Append($"{p.Name}: `{value?.ToString()}`;");
        }
      });
      return (result, sb.ToString());
    }


    /// <summary>
    /// 判斷 Model 物件的欄位內容產生 WHERE ( ColumnName = @ColumnName ) 的 SQL 語法 _
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public static string GenerateSqlWhere(this object model)
    {
      var list = new List<string>();
      model.GetType().GetProperties().ToList().ForEach(p =>
      {
        if (!string.IsNullOrEmpty(p.GetValue(model)?.ToString()))
        {
          list.Add($"( {p.Name} = @{p.Name} )");
        }
      });

      return
        (list.Count > 0)
        ? $"WHERE {(string.Join(" AND ", list.ToArray()))}"
        : "";
    }

    /// <summary>
    /// 比對兩個物件，欄位名稱相同的將值寫入另外一個物件。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="model"></param>
    /// <param name="destination"></param>
    /// <returns></returns>
    public static string CopyTo<T>(this object model, T destination)
    {
      var sb = new StringBuilder();
      if (model == null || destination == null)
      {
        sb.AppendLine($"object reference can not be null");
        return sb.ToString();
      }
      model.GetType().GetProperties().ToList().ForEach(p => // 每一個資料欄位 _
      {
        try
        {
          var destProperty = destination.GetType().GetProperty(p.Name);
          var destPropertyType = destProperty?.PropertyType;
          var destPropertyTypeName = destProperty?.Name ?? "";
          if (
            destProperty != null && destPropertyType != null &&
            !string.IsNullOrEmpty(destPropertyTypeName) && p.Name == destPropertyTypeName
          )
          {
            if (destPropertyType.IsGenericType && destPropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) // check if nullable _
            {
              destPropertyType = destPropertyType.GetGenericArguments()[0];
            }
            var sourceValue = p.GetValue(model);
            // if (sourceValue != null) // 值不為 NULL 才複製 
            {
              var sourcePropertyType = p.PropertyType;
              if (sourcePropertyType.IsGenericType && sourcePropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) // check if nullable _
              {
                sourcePropertyType = sourcePropertyType.GetGenericArguments()[0];
              }
              var sourceTypeIsSystemDateTime = false;
              if (sourcePropertyType?.ToString() == typeof(DateTime).ToString())
              {
                sourceTypeIsSystemDateTime = true;
                if (sourceValue != null)
                {
                  var datetimeValue = (DateTime)sourceValue;
                  sb.AppendLine($"ToString: {datetimeValue.ToString()}, ToUniversalTime: {datetimeValue.ToUniversalTime().ToString_yyyy_MM_dd_HH_mm_ss_fff()}, ToLocalTime: {datetimeValue.ToLocalTime().ToString_yyyy_MM_dd_HH_mm_ss_fff()}");
                }
              }

              if (destProperty != null && destPropertyType != null && sourcePropertyType == destPropertyType) // source type == destination type 
              {
                try
                {
                  destProperty?.SetValue(destination,
                    (sourceTypeIsSystemDateTime ? ((DateTime?)sourceValue) : sourceValue)
                  );
                }
                catch (Exception ex)
                {
                  sb.AppendLine($"{p.Name}: {ex.GetType().Name}: {ex.Message}");
                }
              }
              else if (destPropertyType?.Name == typeof(string).Name)
              {
                try
                {
                  destProperty?.SetValue(destination,
                    (sourceTypeIsSystemDateTime ? ((DateTime?)sourceValue).ToString_yyyy_MM_dd_HH_mm_ss_fff() : sourceValue.ToString())
                  );
                }
                catch (Exception ex)
                {
                  sb.AppendLine($"{p.Name}: {ex.GetType().Name}: {ex.Message}");
                }
              }
            }
          }
        }
        catch (Exception ex)
        {
          sb.AppendLine($"{ex.GetType().Name}: {ex.Message}");
        }
      });
      return sb.ToString();
    }

    /// <summary>
    /// 根據Property名稱取值，發生錯誤時回傳空白
    /// </summary>
    /// <param name="input"></param>
    /// <param name="PropName">Property名稱</param>
    /// <returns></returns>
    public static string GetPropValue(this object input, string PropName)
    {
      string result = "";
      try
      {
        var type = input.GetType();
        var prop = input.GetType().GetProperty(PropName);
        if (prop != null)
        {
          result = prop.GetValue(input, null)!.ToString()!;
        }
      }
      catch (Exception)
      {
        result = "";
      }
      return result;
    }
  }
}
