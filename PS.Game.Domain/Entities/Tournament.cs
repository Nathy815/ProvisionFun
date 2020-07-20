using PS.Game.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Tournament : Base
    {
        public string Name { get; set; }
        public DateTime StartSubscryption { get; set; }
        public DateTime EndSubscryption { get; set; }
        public int SubscryptionLimit { get; set; }
        public int PlayerLimit { get; set; }
        public eMode Mode { get; set; }
        public string Plataform { get; set; }
        public eRound RoundSolo { get; set; }
        public eRound RoundTeam { get; set; }

        // Relational
        public Guid GameID { get; set; }
        public virtual Game Game { get; set; }

        // Collections
        public virtual ICollection<Team> Teams { get; set; }
        public virtual ICollection<Match> Matches { get; set; }
    }
}
