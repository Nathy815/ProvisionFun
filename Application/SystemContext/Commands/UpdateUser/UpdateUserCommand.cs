using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.SystemContext.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public Guid RoleID { get; set; }
    }
}
