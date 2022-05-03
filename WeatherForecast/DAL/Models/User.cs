using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public int RoleId { get; set; }
    }
}
