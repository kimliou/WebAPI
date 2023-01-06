using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedSettingsLib.Models.DB
{
  public class BlogResult
  {
    public int? BlogId { get; set; }
    public string? Url { get; set; }
    public int? Rating { get; set; }
  }
}
