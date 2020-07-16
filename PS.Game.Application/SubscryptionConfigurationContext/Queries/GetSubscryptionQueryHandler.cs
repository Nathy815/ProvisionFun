using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Domain.ViewModels;
using Persistence.Contexts;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Application.SubscryptionConfigurationContext.Queries
{
    public class GetSubscryptionQueryHandler : IRequestHandler<GetSubscryptionQuery, GetSubscryptionDetailVM>
    {
        private readonly MySqlContext _sqlContext;

        public GetSubscryptionQueryHandler(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<GetSubscryptionDetailVM> Handle(GetSubscryptionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var _team = await _sqlContext.Set<Team>()
                                        .Include(t => t.Players)
                                            .ThenInclude(tp => tp.Player)
                                        .Include(t => t.Condominium)
                                        .Include(t => t.Tournament)
                                        .Where(t => t.Id == request.Id)
                                        .FirstOrDefaultAsync();

                var _result = new GetSubscryptionDetailVM(_team);

                return _result;
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
