using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModels
{
    public class GetUserQueryVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Guid RoleID { get; set; }
        public string Role { get; set; }

        public GetUserQueryVM(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
            RoleID = user.RoleID;
            Role = user.Role.Name;
        }
    }
}