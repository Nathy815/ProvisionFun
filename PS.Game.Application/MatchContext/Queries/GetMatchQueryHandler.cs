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
    public class GetMatchQueryHandler : IRequestHandler<GetMatchQuery, MatchVM>
    {
        private readonly MySqlContext _sqlContext;

        public GetMatchQueryHandler(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<MatchVM> Handle(GetMatchQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var _match = await _sqlContext.Set<Match>()
                                        .Include(m => m.Tournament)
                                        .Include(m => m.Player1)
                                        .Include(m => m.Player2)
                                        .Include(m => m.Auditor)
                                        .Where(m => m.Id == request.Id)
                                        .FirstOrDefaultAsync();

                if (_match == null) throw new Exception();

                var _result = new MatchVM(_match);

                return _result;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
