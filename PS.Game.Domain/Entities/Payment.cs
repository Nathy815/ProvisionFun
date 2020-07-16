using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Payment : Base
    {
        public string DocumentNumber { get; set; }
        public string Number { get; set; }
        public string NumberVD { get; set; }
        public string FormatedNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public double Price { get; set; }
        public bool Validated { get; set; }

        // Relational
        public Guid TeamID { get; set; }
        public virtual Team Team { get; set; }
    }
}
