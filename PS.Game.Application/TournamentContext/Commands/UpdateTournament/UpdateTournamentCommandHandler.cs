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

namespace Application.TournamentContext.Commands.UpdateTournament
{
    public class UpdateTournamentCommandHandler : IRequestHandler<UpdateTournamentCommand, bool>
    {
        private readonly MySqlContext _sqlContext;

        public UpdateTournamentCommandHandler(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<bool> Handle(UpdateTournamentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (!request.GameID.HasValue)
                {
                    var _game = await _sqlContext.Set<Game>()
                                            .Where(g => g.Name.Equals(request.Game))
                                            .FirstOrDefaultAsync();

                    if (_game == null)
                    {
                        _game = new Game
                        {
                            Id = Guid.NewGuid(),
                            Name = request.Game
                        };

                        await _sqlContext.Games.AddAsync(_game, cancellationToken);
                    }

                    request.GameID = _game.Id;
                }

                var _tournament = await _sqlContext.Set<Tournament>()
                                            .Where(t => t.Id == request.Id)
                                            .FirstOrDefaultAsync();

                _tournament.Name = request.Name;
                _tournament.StartSubscryption = request.StartSubscryption;
                _tournament.EndSubscryption = request.EndSubscryption;
                _tournament.GameID = request.GameID.Value;
                _tournament.Mode = request.Mode;
                _tournament.Plataform = request.Plataform;
                _tournament.PlayerLimit = request.Mode == PS.Game.Domain.Enums.eMode.Solo ? 1 : request.PlayerLimit;
                _tournament.SubscryptionLimit = request.SubscryptionLimit;

                _sqlContext.Tournaments.Update(_tournament);

                await _sqlContext.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
