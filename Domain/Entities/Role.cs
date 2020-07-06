using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Role : Base
    {
        public string Name { get; set; }

        // Collections
        public virtual ICollection<User> Users { get; set; }
    }
}
