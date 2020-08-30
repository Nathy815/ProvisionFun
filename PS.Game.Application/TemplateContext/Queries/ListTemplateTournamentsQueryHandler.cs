using Domain.Entities;
using Domain.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using PS.Game.Domain.Enums;
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
                var _tournaments = await _sqlContext.Set<Tournament>()
                                             .Include(t => t.Teams)
                                             .Where(t => t.Active)
                                             .ToListAsync();

                var _list = new List<TemplateGameVM>();
                var _games = Enum.GetValues(typeof(eGame));

                foreach (var _game in _games)
                    _list.Add(new TemplateGameVM((eGame)_game, _tournaments.Where(t => t.Game == (eGame)_game).ToList()));

                return _list;
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
