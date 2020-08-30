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

namespace PS.Game.Application.TournamentContext.Commands.DeleteTournament
{
    public class DeleteTournamentCommandHandler : IRequestHandler<DeleteTournamentCommand, bool>
    {
        private readonly MySqlContext _sqlContext;

        public DeleteTournamentCommandHandler(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<bool> Handle(DeleteTournamentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var _tournament = await _sqlContext.Set<Tournament>()
                                            .Where(t => t.Id == request.Id)
                                            .FirstOrDefaultAsync();

                _tournament.Active = false;

                _sqlContext.Tournaments.Update(_tournament);

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
