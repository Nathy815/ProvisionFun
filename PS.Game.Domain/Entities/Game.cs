using PS.Game.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Game : Base
    {
        public string Name { get; set; }

        // Collections
        public virtual ICollection<Tournament> Tournaments { get; set; }
    }
}
