using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Team : Base
    {
        public string Name { get; set; }
        public int Icon { get; set; }
        public int Color { get; set; }
        public eMode Mode { get; set; }
        public eStatus Status { get; set; }
        public double Price { get; set; }
        public bool SubscryptionSent { get; set; }
        public bool PaymentSent { get; set; }
        public bool FinishedSent { get; set; }
        public DateTime? ValidatedDate { get; set; }
        public DateTime? PaymentDate { get; set; }

        // Relational
        public Guid CondominiumID { get; set; }
        public virtual Condominium Condominium { get; set; }

        public Guid TournamentID { get; set; }
        public virtual Tournament Tournament { get; set; }

        // Collections
        public virtual ICollection<TeamPlayer> Players { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
