using System;
using System.Collections.Generic;

namespace TestEFCore.Models;

public partial class Blog
{
    public int BlogId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
