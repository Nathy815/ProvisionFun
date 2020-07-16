using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SystemContext.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly MySqlContext _sqlContext;

        public DeleteUserCommandHandler(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var _user = await _sqlContext.Set<User>()
                                        .Where(u => u.Id == request.userID)
                                        .FirstOrDefaultAsync();

                _user.Active = false;

                _sqlContext.Users.Update(_user);

                await _sqlContext.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
