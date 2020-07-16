using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Match : Base
    {
        public int Sequence { get; set; }
        public DateTime? Date { get; set; }
        public string Comments { get; set; }
        public double Player1Score { get; set; }
        public double Player2Score { get; set; }
        public Guid? Winner { get; set; }
        public eType Type { get; set; }

        // Relational
        public Guid Player1ID { get; set; }
        public virtual Team Player1 { get; set; }

        public Guid Player2ID { get; set; }
        public virtual Team Player2 { get; set; }

        public Guid? AuditorID { get; set; }
        public virtual User Auditor { get; set; }

        public Guid TournamentID { get; set; }
        public virtual Tournament Tournament { get; set; }
    }
}
