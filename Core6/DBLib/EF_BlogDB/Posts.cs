using System;
using System.Collections.Generic;

namespace DBLib.EF_BlogDB;

public partial class Posts
{
    public int PostId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public int BlogId { get; set; }

    public virtual Blogs Blog { get; set; } = null!;
}
