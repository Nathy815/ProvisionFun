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
    public class GetTopMatchesQueryHandler : IRequestHandler<GetTopMatchesQuery, List<GetMatchQueryVM>>
    {
        private readonly MySqlContext _sqlContext;

        public GetTopMatchesQueryHandler(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<List<GetMatchQueryVM>> Handle(GetTopMatchesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var _matches = await _sqlContext.Set<Match>()
                                            .Include(m => m.Tournament)
                                                .ThenInclude(t => t.Game)
                                            .Include(m => m.Player1)
                                            .Include(m => m.Player2)
                                            .Where(m => m.Active &&
                                                        m.Date > DateTime.Now)
                                            .OrderBy(m => Guid.NewGuid())
                                            .Take(3)
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
