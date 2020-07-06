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
    public class ListRolesQueryHandler : IRequestHandler<ListRolesQuery, List<GetRolesQueryVM>>
    {
        private readonly MySqlContext _sqlContext;

        public ListRolesQueryHandler(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<List<GetRolesQueryVM>> Handle(ListRolesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var _roles = await _sqlContext.Set<Role>()
                                        .Where(r => r.Active)
                                        .ToListAsync();

                var _list = new List<GetRolesQueryVM>();
                foreach (var _role in _roles)
                    _list.Add(new GetRolesQueryVM(_role));

                return _list;
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
