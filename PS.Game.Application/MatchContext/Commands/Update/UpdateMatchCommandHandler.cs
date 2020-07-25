using Application.Services;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using PS.Game.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.MatchContext.Commands.Update
{
    public class UpdateMatchCommandHandler : BackgroundService, IRequestHandler<UpdateMatchCommand, bool>
    {
        private readonly MySqlContext _sqlContext;

        public UpdateMatchCommandHandler(MySqlContext sqlContext, IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
            _sqlContext = sqlContext;
        }

        public async Task<bool> Handle(UpdateMatchCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var _match = await _sqlContext.Set<Match>()
                                        .Include(m => m.Player1)
                                            .ThenInclude(p => p.Players)
                                        .Include(m => m.Player2)
                                            .ThenInclude(p => p.Players)
                                        .Where(m => m.Id == request.Id)
                                        .FirstOrDefaultAsync();

                if (request.Winner.HasValue)
                {
                    _match.Winner = request.Winner.Value;
                    _match.Player1Score = request.ScorePlayer1.HasValue ? request.ScorePlayer1.Value : 0;
                    _match.Player2Score = request.ScorePlayer2.HasValue ? request.ScorePlayer2.Value : 0;
                    if (_match.Winner.Value == _match.Player1.Id)
                        _match.Player2.Status = eStatus.Eliminated;
                    else
                        _match.Player1.Status = eStatus.Eliminated;
                    _match.Comments = request.Comments;

                    var _tournament = await _sqlContext.Set<Tournament>()
                                                .Include(t => t.Matches)
                                                    .ThenInclude(m => m.Player1)
                                                .Where(t => t.Id == _match.TournamentID)
                                                .FirstOrDefaultAsync();

                    var _mode = _match.Player1.Players.Count == 1 ? eMode.Solo : eMode.Team;
                    var _matches = _tournament.Matches.Where(m => m.Active &&
                                                                  m.Player1.Mode == _mode &&
                                                                  m.Round == _match.Round &&
                                                                  !m.Winner.HasValue)
                                                      .ToList();

                    if (_matches.Count == 0)
                        await GerarPartida(_match.TournamentID, _sqlContext, new CancellationToken());
                }
                else
                {
                    if (request.Date.HasValue)
                        _match.Date = request.Date;

                    if (request.AuditorID != null)
                        _match.AuditorID = request.AuditorID;
                }

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
