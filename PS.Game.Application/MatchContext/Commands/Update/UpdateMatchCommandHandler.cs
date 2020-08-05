using Application.Services;
using Application.Services.Interfaces;
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
        private readonly IEmail _email;

        public UpdateMatchCommandHandler(MySqlContext sqlContext, IEmail email, IServiceScopeFactory scopeFactory) : base(scopeFactory, email)
        {
            _sqlContext = sqlContext;
            _email = email;
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

                // Se vai informar o vencedor, é uma partida agendada
                if (request.Winner.HasValue)
                {
                    _match.Winner = request.Winner.Value;
                    _match.Player1Score = request.ScorePlayer1.HasValue ? request.ScorePlayer1.Value : 0;
                    _match.Player2Score = request.ScorePlayer2.HasValue ? request.ScorePlayer2.Value : 0;
                    _match.Comments = request.Comments;

                    var _tournament = await _sqlContext.Set<Tournament>()
                                                .Include(t => t.Matches)
                                                    .ThenInclude(m => m.Player1)
                                                        .ThenInclude(p => p.Players)
                                                            .ThenInclude(p => p.Player)
                                                .Include(t => t.Matches)
                                                    .ThenInclude(m => m.Player2)
                                                        .ThenInclude(p => p.Players)
                                                            .ThenInclude(p => p.Player)
                                                .Include(t => t.Teams)
                                                    .ThenInclude(t => t.Condominium)
                                                .Where(t => t.Id == _match.TournamentID)
                                                .FirstOrDefaultAsync();

                    var _mode = _match.Player1.Players.Count == 1 ? eMode.Solo : eMode.Team;

                    // Verifica a quantidade de times que ainda estão na disputa
                    var _pendingTeams = _tournament.Teams.Where(t => t.Active &&
                                                                     t.Mode == _mode &&
                                                                     t.Status == eStatus.Finished).Count();

                    var _pendingMatches = _tournament.Matches.Where(m => m.Active &&
                                                                         m.Player1.Mode == _mode &&
                                                                         !m.Winner.HasValue).Count();

                    var _round = _tournament.Mode == eMode.Solo ? _tournament.RoundSolo : _tournament.RoundTeam;

                    // Se for fase 2 ou 4, o perdedor está eliminado
                    if (_round == eRound.Fase2 || _round == eRound.Fase4)
                    {
                        if (_match.Winner.Value == _match.Player1ID)
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

                    // Se tiver menos de 3 times na disputa, o vencedor da partida é o campeão
                    if (_pendingTeams < 3)
                    {
                        if (_match.Winner.Value == _match.Player1ID)
                            _match.Player1.Status = eStatus.Winner;
                        else
                            _match.Player2.Status = eStatus.Winner;

                        // Se já tem vencedor, o torneio é inativado
                        _tournament.Active = false;

                        _sqlContext.Matches.Update(_match);

                        _sqlContext.Tournaments.Update(_tournament);

                        await _sqlContext.SaveChangesAsync(cancellationToken);
                    }
                    else if (_pendingMatches == 0) // Se não terminou, mas não tem mais partidas pendentes, gera próxima rodada
                        await GerarPartida(_tournament, _sqlContext, _mode, new CancellationToken());
                }
                else // senão, é uma partida pendente
                {
                    var _alter = false;

                    if (request.Date.HasValue)
                    {
                        _alter = _match.Date.HasValue ? true : false;
                        _match.Date = request.Date;
                    }

                    if (request.AuditorID != null)
                    {
                        _alter = _match.AuditorID.HasValue ? true : false;
                        _match.AuditorID = request.AuditorID;
                    }

                    _sqlContext.Matches.Update(_match);

                    await _sqlContext.SaveChangesAsync(cancellationToken);

                    var _count = 2;
                    while (_count > 2)
                    {
                        var _team = _count == 2 ? _match.Player1 : _match.Player2;
                        await _email.SendEmail(_team, eStatus.Winner, null, _match, _alter); // manda Winner para entrar no else
                    }
                }

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
