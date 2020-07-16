using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModels
{
    public class GetAuditorsQueryVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public GetAuditorsQueryVM(User user)
        {
            Id = user.Id;
            Name = user.Name;
        }
    }
}
