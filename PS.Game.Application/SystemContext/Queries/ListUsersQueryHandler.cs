using Domain.Entities;
using Domain.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SystemContext.Queries
{
    public class ListUsersQueryHandler : IRequestHandler<ListUsersQuery, List<GetUserQueryVM>>
    {
        private readonly MySqlContext _sqlContext;

        public ListUsersQueryHandler(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<List<GetUserQueryVM>> Handle(ListUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var _users = await _sqlContext.Set<User>()
                                        .Include(u => u.Role)
                                        .Where(u => u.Active &&
                                                    !u.IsMaster)
                                        .ToListAsync();

                var _list = new List<GetUserQueryVM>();
                foreach (var _user in _users)
                    _list.Add(new GetUserQueryVM(_user));

                return _list;
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
