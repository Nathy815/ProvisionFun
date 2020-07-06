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
    public class GetGameQueryHandler : IRequestHandler<GetGameQuery, GetGameQueryVM>
    {
        private readonly MySqlContext _sqlContext;

        public GetGameQueryHandler(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<GetGameQueryVM> Handle(GetGameQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var _game = await _sqlContext.Set<Game>()
                                        .Where(g => g.Id == request.gameID)
                                        .FirstOrDefaultAsync();

                var _result = new GetGameQueryVM(_game);

                return _result;
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
