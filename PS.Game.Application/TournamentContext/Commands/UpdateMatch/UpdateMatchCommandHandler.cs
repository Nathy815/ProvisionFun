using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.TournamentContext.Commands.UpdateMatch
{
    public class UpdateMatchCommandHandler : IRequestHandler<UpdateMatchCommand, bool>
    {
        private readonly MySqlContext _sqlContext;

        public UpdateMatchCommandHandler(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<bool> Handle(UpdateMatchCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var _match = await _sqlContext.Set<Match>()
                                        .Where(m => m.Id == request.Id)
                                        .FirstOrDefaultAsync();

                _match.Date = request.Date;
                _match.AuditorID = request.AuditorID;

                _sqlContext.Matches.Update(_match);

                await _sqlContext.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
