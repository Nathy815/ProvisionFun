using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Condominium : Base
    {
        public string Name { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public bool Validated { get; set; }

        // Collections
        public virtual ICollection<Team> Teams { get; set; }
    }
}
