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
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserQueryVM>
    {
        private readonly MySqlContext _sqlContext;

        public GetUserQueryHandler(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<GetUserQueryVM> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var _user = await _sqlContext.Set<User>()
                                        .Include(u => u.Role)
                                        .Where(u => u.Id == request.userID)
                                        .FirstOrDefaultAsync();

                var _result = new GetUserQueryVM(_user);

                return _result;
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
