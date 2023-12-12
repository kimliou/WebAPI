using System;
using System.Collections.Generic;

namespace DBLib.EF_BlogDB;

public partial class Users
{
    public string UserID { get; set; } = null!;

    public string? UserName { get; set; }

    public string? UserMobile { get; set; }

    public string? UserEmail { get; set; }

    public string? IsLogon { get; set; }

    public DateTime? LogonTime { get; set; }

    public DateTime UpdateTime { get; set; }

    public string UpdateUser { get; set; } = null!;
}
