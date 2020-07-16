using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModels
{
    public class GetRolesQueryVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public GetRolesQueryVM(Role role)
        {
            Id = role.Id;
            Name = role.Name;
        }
    }
}
