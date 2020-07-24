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
    public class GetTournamentQueryHandler : IRequestHandler<GetTournamentQuery, GetTournamentQueryVM>
    {
        private readonly MySqlContext _sqlContext;

        public GetTournamentQueryHandler(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<GetTournamentQueryVM> Handle(GetTournamentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var _tournament = await _sqlContext.Set<Tournament>()
                                            .Where(t => t.Id == request.tournamentID)
                                            .FirstOrDefaultAsync();

                var _result = new GetTournamentQueryVM(_tournament);

                return _result;
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
