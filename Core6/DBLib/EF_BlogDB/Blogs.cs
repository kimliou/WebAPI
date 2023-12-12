using System;
using System.Collections.Generic;

namespace DBLib.EF_BlogDB;

public partial class Blogs
{
    public int BlogId { get; set; }

    public string Url { get; set; } = null!;

    public int Rating { get; set; }

    public virtual ICollection<Posts> Posts { get; } = new List<Posts>();
}
