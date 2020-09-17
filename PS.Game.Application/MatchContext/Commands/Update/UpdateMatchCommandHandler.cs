using Application.Services;
using Application.Services.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using PS.Game.Application.Services.Interfaces;
using PS.Game.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.MatchContext.Commands.Update
{
    public class UpdateMatchCommandHandler : IRequestHandler<UpdateMatchCommand, bool>
    {
        private readonly MySqlContext _sqlContext;
        private readonly IMatchService _service;
        private readonly IEmail _email;

        public UpdateMatchCommandHandler(MySqlContext sqlContext, IEmail email, IMatchService service)
        {
            _sqlContext = sqlContext;
            _email = email;
            _service = service;
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
                                        .Include(m => m.Tournament)
                                        .Where(m => m.Id == request.Id)
                                        .FirstOrDefaultAsync();

                if (request.AuditorID.HasValue) _match.AuditorID = request.AuditorID.Value;
                if (request.Date.HasValue) _match.Date = request.Date.Value.AddHours(-3);
                if (request.Winner.HasValue) _match.Winner = request.Winner.Value;
                _match.Player1Score = request.ScorePlayer1.HasValue ? request.ScorePlayer1.Value : 0;
                _match.Player2Score = request.ScorePlayer2.HasValue ? request.ScorePlayer2.Value : 0;
                _match.Comments = request.Comments;

                _sqlContext.Matches.Update(_match);

                // Se vai informar o vencedor, é uma partida agendada
                if (request.Winner.HasValue)
                {
                    var _tournament = await _sqlContext.Set<Tournament>()
                                                .Include(t => t.Teams)
                                                    .ThenInclude(t => t.Condominium)
                                                .Include(t => t.Teams)
                                                    .ThenInclude(t => t.Players)
                                                        .ThenInclude(p => p.Player)
                                                .Where(t => t.Id == _match.TournamentID)
                                                .FirstOrDefaultAsync();

                    var _eliminate = false;

                    // Se for uma fase de números ímpares, verifica se o usuário tem outra partida marcada
                    if (_match.Round == eRound.Fase1 || _match.Round == eRound.Fase3)
                    {
                        var _dbMatch = await _sqlContext.Set<Match>()
                                                    .Where(m => m.Active &&
                                                                m.Id != _match.Id &&
                                                                !m.Winner.HasValue &&
                                                                m.TournamentID == _match.TournamentID &&
                                                                (m.Player1ID == _match.Winner.Value ||
                                                                m.Player2ID == _match.Winner.Value))
                                                    .FirstOrDefaultAsync();

                        if (_dbMatch == null)
                            _eliminate = true;
                    }
                    else // Se for uma fase de números pares, elimina o perdedor automaticamente
                        _eliminate = true;

                    if (_eliminate)
                    { 
                        if (_match.Player1ID == _match.Winner.Value)
                        {
                            _match.Player2.Status = eStatus.Eliminated;
                            await _email.SendEmail(_match.Player2, eStatus.Eliminated);
                        }
                        else
                        {
                            _match.Player1.Status = eStatus.Eliminated;
                            await _email.SendEmail(_match.Player1, eStatus.Eliminated);
                        }
                    }

                    _sqlContext.Matches.Update(_match);

                    // Verifica se é a última partida da fase
                    var _matches = _tournament.Matches.Where(m => m.Active &&
                                                                  m.Round == _match.Round &&
                                                                  m.TournamentID == _match.TournamentID &&
                                                                  !m.Winner.HasValue &&
                                                                  m.Id != _match.Id).ToList();
                    
                    if (_matches.Count == 0)
                    {
                        if (_match.Round == eRound.Fase4)
                        {
                            if (_match.Player2ID == _match.Winner.Value)
                                _match.Player2.Status = eStatus.Winner;
                            else
                                _match.Player1.Status = eStatus.Winner;

                            _sqlContext.Matches.Update(_match);
                        }
                        else
                        {
                            var _newMatches = new List<Match>();

                            if (_match.Round == eRound.Fase1)
                                _newMatches.AddRange(await _service.GenerateRound2(_tournament, _match.Player1.Mode));
                            else if (_match.Round == eRound.Fase2)
                                _newMatches.AddRange(await _service.GenerateRound3(_tournament, _match.Player1.Mode));
                            else
                                _newMatches.AddRange(await _service.GenerateRound4(_tournament, _match.Player1.Mode));

                            await _sqlContext.Matches.AddRangeAsync(_newMatches, cancellationToken);
                        }
                    }
                }

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
