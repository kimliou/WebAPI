using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharedSettingsLib.Extensions;

public static class InputValidation
{
  public static bool InputIsValid(this object input)
  {
    return input.CheckInputIsValid().IsValid;
  }

  public static (bool IsValid, IEnumerable<string> Message) CheckInputIsValid(this object input)
  {
    var IsValid = true;
    var Message = new List<string>();
    foreach (var field in input.GetType().GetProperties())
    {
      var Value = field.GetValue(input);
      var Required = field.GetCustomAttribute<RequiredAttribute>();
      if (Required is not null
        && string.IsNullOrEmpty(Value?.ToString()))
      {
        IsValid = false;
        if (string.IsNullOrEmpty(Required.ErrorMessage))
        {
          Message.Add($"X {field.Name} 必要輸入");
        }
        else
        {
          Message.Add(Required.ErrorMessage);
        }
      }

      var StringLength = field.GetCustomAttribute<StringLengthAttribute>();
      if (StringLength is not null
        && !(StringLength.MinimumLength <= $"{Value}".Length && $"{Value}".Length <= StringLength.MaximumLength))
      {
        IsValid = false;
        if (string.IsNullOrEmpty(StringLength.ErrorMessage))
        {
          Message.Add($"X {field.Name} 長度錯誤, 最小長度: {StringLength.MinimumLength}, 最大長度: {StringLength.MaximumLength}");
        }
        else
        {
          Message.Add(StringLength.ErrorMessage);
        }
      }
    }
    return (IsValid, Message);
  }
}

