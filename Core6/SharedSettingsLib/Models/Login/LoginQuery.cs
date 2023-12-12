using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedSettingsLib.Models.Login
{
  public class LoginQuery
  {
    [Required]
    public string? PassWord { get; set; }
    [Required]
    public string? UserID { get; set; }
  }
}
