using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.SystemContext.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<bool>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Guid RoleID { get; set; }
    }
}