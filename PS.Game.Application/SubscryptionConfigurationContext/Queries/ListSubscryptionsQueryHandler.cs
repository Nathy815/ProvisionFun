using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Domain.ViewModels;
using System.Threading;
using System.Threading.Tasks;
using Persistence.Contexts;
using Domain.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PS.Game.Domain.ViewModels;

namespace Application.SubscryptionConfigurationContext.Queries
{
    public class ListSubscryptionsQueryHandler : IRequestHandler<ListSubscryptionsQuery, ListSubscryptionsQueryVM>
    {
        private readonly MySqlContext _sqlContext;

        public ListSubscryptionsQueryHandler(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<ListSubscryptionsQueryVM> Handle(ListSubscryptionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var _teams = await _sqlContext.Set<Team>()
                                        .Include(t => t.Players)
                                            .ThenInclude(p => p.Player)
                                        .Include(t => t.Tournament)
                                        .Where(t => t.Tournament.Active)
                                        .ToListAsync();

                var _list = new ListSubscryptionsQueryVM(_teams);

                return _list;
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
