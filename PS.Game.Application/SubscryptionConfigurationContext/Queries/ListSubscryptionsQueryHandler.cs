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

namespace Application.SubscryptionConfigurationContext.Queries
{
    public class ListSubscryptionsQueryHandler : IRequestHandler<ListSubscryptionsQuery, List<GetSubscryptionVM>>
    {
        private readonly MySqlContext _sqlContext;

        public ListSubscryptionsQueryHandler(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<List<GetSubscryptionVM>> Handle(ListSubscryptionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var _teams = await _sqlContext.Set<Team>()
                                        .Include(t => t.Players)
                                        .Include(t => t.Tournament)
                                        .Where(t => t.Active)
                                        .ToListAsync();

                var _list = new List<GetSubscryptionVM>();

                foreach (var _team in _teams)
                    _list.Add(new GetSubscryptionVM(_team));

                return _list;
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
