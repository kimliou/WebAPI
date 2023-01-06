using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedSettingsLib.Models.DB
{
  public class BlogQuery
  {
    public int? BlogId { get; set; }
    [StringLength(50)]
    public string? Url { get; set; }
    public int? Rating { get; set; }
  }
}
