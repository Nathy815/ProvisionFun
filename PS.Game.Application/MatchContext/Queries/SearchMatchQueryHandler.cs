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

namespace Application.MatchContext.Queries
{
    public class SearchMatchQueryHandler : IRequestHandler<SearchMatchQuery, List<GetMatchQueryVM>>
    {
        private readonly MySqlContext _sqlContext;

        public SearchMatchQueryHandler(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<List<GetMatchQueryVM>> Handle(SearchMatchQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var _matches = await _sqlContext.Set<Match>()
                                        .Include(m => m.Tournament)
                                        .Include(m => m.Player1)
                                            .ThenInclude(m => m.Players)
                                                .ThenInclude(m => m.Player)
                                        .Include(m => m.Player2)
                                            .ThenInclude(m => m.Players)
                                                .ThenInclude(m => m.Player)
                                        .Where(m => m.Active &&
                                                    m.Tournament.Active &&
                                                    m.Date > DateTime.Now &&
                                                    (m.Player1.Players.Any(p => p.Active &&
                                                                               p.Player.CPF.Equals(request.CPF)) ||
                                                    m.Player2.Players.Any(p => p.Active &&
                                                                               p.Player.CPF.Equals(request.CPF))))
                                        .OrderBy(m => m.Date.Value)
                                        .ToListAsync();
                
                var _list = new List<GetMatchQueryVM>();

                foreach (var _match in _matches)
                    _list.Add(new GetMatchQueryVM(_match));

                return _list;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
