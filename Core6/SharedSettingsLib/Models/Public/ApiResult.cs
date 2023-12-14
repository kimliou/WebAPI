using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedSettingsLib.Models.Public;

public class ApiResult<T>
{
  public bool Success { get; set; } = false;
  public List<Message> Message { get; set; } = new List<Message>();
  public T? Data { get; set; }
}

public class ApiResult
{
  public bool Success { get; set; } = false;
  public string? Status { get; set; } = null;
  public string? Message { get; set; } = "";
}
public class Message
{
  public string? ColumnName { get; set; } = "";
  public string? MessageText { get; set; } = "";
}