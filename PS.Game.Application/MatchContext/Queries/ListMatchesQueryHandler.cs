using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using PS.Game.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PS.Game.Application.MatchContext.Queries
{
    public class ListMatchesQueryHandler : IRequestHandler<ListMatchesQuery, MatchesVM>
    {
        private readonly MySqlContext _sqlContext;

        public ListMatchesQueryHandler(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<MatchesVM> Handle(ListMatchesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var _matches = await _sqlContext.Set<Match>()
                                         .Include(m => m.Auditor)
                                         .Include(m => m.Player1)
                                            .ThenInclude(p => p.Players)
                                                .ThenInclude(p => p.Player)
                                         .Include(m => m.Player2)
                                            .ThenInclude(p => p.Players)
                                                .ThenInclude(p => p.Player)
                                         .Include(m => m.Tournament)
                                         .Where(m => m.Active && m.Tournament.Active)
                                         .ToListAsync();

                var _list = new MatchesVM(_matches);

                return _list;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
