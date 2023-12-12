using System;
using System.Collections.Generic;

namespace DBLib.EF_BlogDB;

public partial class Blog
{
    public int? BlogId { get; set; }

    public string? Url { get; set; }

    public int? Rating { get; set; }
}
