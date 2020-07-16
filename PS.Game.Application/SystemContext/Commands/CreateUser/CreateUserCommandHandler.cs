using Application.Services;
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

namespace Application.SystemContext.Commands.CreateUser
{
    public class CreateUserCommandHandler : Util, IRequestHandler<CreateUserCommand, bool>
    {
        private readonly MySqlContext _sqlContext;

        public CreateUserCommandHandler(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var _user = await _sqlContext.Set<User>()
                                        .Where(u => u.Email.Equals(request.Email))
                                        .FirstOrDefaultAsync();

                if (_user != null)
                {
                    _user.Active = true;
                    _user.Name = request.Name;
                    _user.Password = HashPassword(request.Email);
                    _user.RoleID = request.RoleID;

                    _sqlContext.Users.Update(_user);
                }
                else
                {
                    _user = new User
                    {
                        Id = Guid.NewGuid(),
                        Active = true,
                        Name = request.Name,
                        Email = request.Email,
                        Password = HashPassword(request.Email),
                        RoleID = request.RoleID
                    };

                    await _sqlContext.Users.AddAsync(_user, cancellationToken);
                }

                await _sqlContext.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
