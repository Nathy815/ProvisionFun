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
    public class ListGamesQueryHandler : IRequestHandler<ListGamesQuery, List<GetGameQueryVM>>
    {
        private readonly MySqlContext _sqlContext;

        public ListGamesQueryHandler(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<List<GetGameQueryVM>> Handle(ListGamesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var _games = await _sqlContext.Set<Game>()
                                        .Where(g => g.Active)
                                        .ToListAsync();

                var _list = new List<GetGameQueryVM>();
                foreach (var _game in _games)
                    _list.Add(new GetGameQueryVM(_game));
               
                return _list;
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
