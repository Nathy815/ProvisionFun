using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Player : Base
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string CPF { get; set; }
        public string Document { get; set; }

        // Collection
        public virtual ICollection<TeamPlayer> Teams { get; set; }
    }
}
