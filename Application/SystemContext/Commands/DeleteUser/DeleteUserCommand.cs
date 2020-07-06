using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.SystemContext.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public Guid userID { get; set; }

        public DeleteUserCommand(Guid Id)
        {
            userID = Id;
        }
    }
}
