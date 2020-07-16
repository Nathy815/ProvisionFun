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

namespace Application.TemplateContext.Queries
{
    public class ListTemplateTournamentsQueryHandler : IRequestHandler<ListTemplateTournamentsQuery, List<TemplateGameVM>>
    {
        private readonly MySqlContext _sqlContext;

        public ListTemplateTournamentsQueryHandler(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<List<TemplateGameVM>> Handle(ListTemplateTournamentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var _games = await _sqlContext.Set<Game>()
                                        .Include(g => g.Tournaments)
                                            .ThenInclude(t => t.Teams)
                                        .Where(g => g.Active)
                                        .ToListAsync();

                var _list = new List<TemplateGameVM>();
                foreach (var _game in _games)
                    if (_game.Tournaments.Count > 0)
                        _list.Add(new TemplateGameVM(_game));

                return _list;
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
