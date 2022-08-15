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

  }
}
