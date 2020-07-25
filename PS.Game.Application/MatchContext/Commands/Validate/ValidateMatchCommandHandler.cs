using Application.Services;
using Application.Services.Interfaces;
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

namespace Application.MatchContext.Commands.Validate
{
    public class ValidateMatchCommandHandler : Util, IRequestHandler<ValidateMatchCommand, bool>
    {
        private readonly MySqlContext _sqlContext;

        public ValidateMatchCommandHandler(MySqlContext sqlContext, IEmail email) : base(email)
        {
            _sqlContext = sqlContext;
        }

        public async Task<bool> Handle(ValidateMatchCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var _match = await _sqlContext.Set<Match>()
                                        .Include(m => m.Player1)
                                        .Include(m => m.Player2)
                                        .Where(m => m.Id == request.MatchID)
                                        .FirstOrDefaultAsync();

                _match.Comments = request.Comments;

                if (request.Player1Score.HasValue)
                {
                    _match.Player1Score = request.Player1Score.Value;
                    _match.Player2Score = request.Player2Score.Value;
                }

                if (request.Winner.HasValue)
                    _match.Winner = request.Winner.Value;
                else
                {
                    if (_match.Player1Score > _match.Player2Score)
                        _match.Winner = _match.Player1ID;
                    else
                        _match.Winner = _match.Player2ID;
                }

                /*if (_match.Type == PS.Game.Domain.Enums.eType.Tournament)
                {
                    if (_match.Winner.Value == _match.Player1ID)
                        _match.Player2.Status = PS.Game.Domain.Enums.eStatus.Eliminated;
                    else
                        _match.Player1.Status = PS.Game.Domain.Enums.eStatus.Eliminated;
                }*/

                _sqlContext.Matches.Update(_match);

                // Verificar se é a última rodada (League) ou se é a última partida da rodada
                var _tournament = await _sqlContext.Set<Tournament>()
                                            .Include(t => t.Matches)
                                            .Where(t => t.Id == _match.TournamentID)
                                            .FirstOrDefaultAsync();

                /*if (_tournament.Type == PS.Game.Domain.Enums.eType.League &&
                    _tournament.Matches.Where(m => m.Active && (!m.Date.HasValue || !m.Winner.HasValue)).Count() == 0)
                {

                }*/

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
