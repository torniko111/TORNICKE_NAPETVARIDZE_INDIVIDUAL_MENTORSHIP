using System;
using System.Collections.Generic;

namespace DatabaseFirstSample.Models
{
    public partial class Blog
    {
        public Blog()
        {
            Posts = new HashSet<Post>();
        }

        public int BlogId { get; set; }
        public string? Name { get; set; }
        public string? Url { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
