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

namespace Application.TournamentContext.Commands.CreateTournament
{
    public class CreateTournamentCommandHandler : IRequestHandler<CreateTournamentCommand, bool>
    {
        private readonly MySqlContext _sqlContext;

        public CreateTournamentCommandHandler(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<bool> Handle(CreateTournamentCommand request, CancellationToken cancellationToken)
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

                var _tournament = new Tournament
                {
                    Id = Guid.NewGuid(),
                    StartSubscryption = request.StartSubscryption,
                    EndSubscryption = request.EndSubscryption,
                    Name = request.Name,
                    GameID = request.GameID.Value,
                    Mode = request.Mode,
                    Plataform = request.Plataform,
                    PlayerLimit = request.Mode == PS.Game.Domain.Enums.eMode.Solo ? 1 : request.PlayerLimit,
                    SubscryptionLimit = request.SubscryptionLimit > 0 ? request.SubscryptionLimit : 0
                };

                await _sqlContext.Tournaments.AddAsync(_tournament);

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
