using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Base
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}