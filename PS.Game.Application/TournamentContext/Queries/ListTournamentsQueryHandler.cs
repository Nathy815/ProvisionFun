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
    public class ListTournamentsQueryHandler : IRequestHandler<ListTournamentsQuery, List<GetTournamentQueryVM>>
    {
        private readonly MySqlContext _sqlContext;

        public ListTournamentsQueryHandler(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<List<GetTournamentQueryVM>> Handle(ListTournamentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var _tournaments = await _sqlContext.Set<Tournament>()
                                                .Include(t => t.Game)
                                                .Where(t => t.Active)
                                                .ToListAsync();

                var _list = new List<GetTournamentQueryVM>();
                foreach (var _tournament in _tournaments)
                    _list.Add(new GetTournamentQueryVM(_tournament));

                return _list;
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
