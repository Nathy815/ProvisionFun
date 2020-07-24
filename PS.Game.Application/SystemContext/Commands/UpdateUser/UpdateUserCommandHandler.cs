using Application.Services;
using Application.Services.Interfaces;
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

namespace Application.SystemContext.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : Util, IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly MySqlContext _sqlContext;

        public UpdateUserCommandHandler(MySqlContext sqlContext, IEmail email) : base(email)
        {
            _sqlContext = sqlContext;
        }

        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var _user = await _sqlContext.Set<User>()
                                        .Where(u => u.Id == request.Id)
                                        .FirstOrDefaultAsync();

                _user.Name = request.Name;
                _user.Password = HashPassword(request.Password);
                _user.RoleID = request.RoleID;

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
