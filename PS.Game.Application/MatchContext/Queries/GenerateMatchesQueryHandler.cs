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

namespace PS.Game.Application.MatchContext.Queries
{
    public class GenerateMatchesQueryHandler : IRequestHandler<GenerateMatchesQuery, bool>
    {
        private readonly MySqlContext _sqlContext;
        private readonly IEmail _email;
        private readonly IMatchService _service;
        
        public GenerateMatchesQueryHandler(IEmail email, IMatchService service, MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
            _email = email;
            _service = service;
        }

        public async Task<bool> Handle(GenerateMatchesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var _tournament = await _sqlContext.Set<Tournament>()
                                                .Include(t => t.Teams)
                                                    .ThenInclude(t => t.Condominium)
                                                .Include(t => t.Teams)
                                                    .ThenInclude(t => t.Players)
                                                        .ThenInclude(p => p.Player)
                                                .Where(t => t.Id == request.Id)
                                                .FirstOrDefaultAsync();

                var _count = _tournament.Mode == eMode.Both ? 2 : 1;

                while (_count > 0)
                {
                    var _mode = _tournament.Mode == eMode.Solo || _count == 2 ? eMode.Solo : eMode.Team;

                    // Só gera partidas manuais para a primeira rodada
                    if ((_mode == eMode.Solo && _tournament.RoundSolo == eRound.NotStarted ) ||
                        (_mode == eMode.Team && _tournament.RoundTeam == eRound.NotStarted))
                    {
                        var _eliminateTeams = _tournament.Teams.Where(t => t.Active &&
                                                                           t.Mode == _mode &&
                                                                           t.Status != eStatus.Finished)
                                                               .ToList();

                        foreach (var _eliminate in _eliminateTeams)
                        {
                            _eliminate.Status = eStatus.Cancelled;
                            _eliminate.CancellationComments = "Sentimos muito, mas não houve confirmação de pagamento dentro do período de inscrições.";
                            _eliminate.CancellationSent = await _email.SendEmail(_eliminate, eStatus.Cancelled);
                        }

                        if (_eliminateTeams.Count > 0)
                            _sqlContext.Teams.UpdateRange(_eliminateTeams);

                        var _teams = _tournament.Teams.Where(t => t.Active &&
                                                                  t.Mode == _mode &&
                                                                  t.Status == eStatus.Finished)
                                                      .ToList();

                        // Apenas uma inscrição, já declara o vencedor
                        if (_teams.Count == 1)
                        {
                            var _team = _teams.FirstOrDefault();
                            _team.Status = eStatus.Winner;

                            _sqlContext.Teams.Update(_team);
                        }
                        else
                        {
                            var _matches = await _service.GenerateRound1(_tournament, _mode);

                            await _sqlContext.Matches.AddRangeAsync(_matches, cancellationToken);
                        }
                    }

                    _count -= 1;
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
