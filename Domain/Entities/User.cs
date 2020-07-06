using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class User : Base
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsMaster { get; set; }
        
        // Relational
        public Guid RoleID { get; set; }
        public virtual Role Role { get; set; }
    }
}
