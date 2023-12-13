using SharedSettingsLib.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedSettingsLib.Models.Login
{

  public class LoginResult
  {
    public bool? Success { get; set; }

    public string? TokenString { get; set; }
    public UsersResult? UserData { get; set; }
  }
}