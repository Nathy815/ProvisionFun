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

namespace Application.TournamentContext.Queries
{
    public class ListAuditorsQueryHandler : IRequestHandler<ListAuditorsQuery, List<GetAuditorsQueryVM>>
    {
        private readonly MySqlContext _sqlContext;

        public ListAuditorsQueryHandler(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<List<GetAuditorsQueryVM>> Handle(ListAuditorsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var _list = new List<GetAuditorsQueryVM>();

                var _users = await _sqlContext.Set<User>()
                                        .Where(u => u.Active &&
                                                    u.RoleID == new Guid("2f743547-6ab3-4f99-93a0-457eab81fecf"))
                                        .ToListAsync();

                foreach (var _user in _users)
                    _list.Add(new GetAuditorsQueryVM(_user));

                return _list;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
