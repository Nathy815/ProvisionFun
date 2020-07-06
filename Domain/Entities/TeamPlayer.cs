using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class TeamPlayer : Base
    {
        // Relational
        public Guid PlayerID { get; set; }
        public virtual Player Player { get; set; }

        public Guid TeamID { get; set; }
        public virtual Team Team { get; set; }

        public bool IsPrincipal { get; set; }
    }
}
