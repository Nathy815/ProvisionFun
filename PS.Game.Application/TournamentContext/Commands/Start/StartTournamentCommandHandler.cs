using Application.Services;
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

namespace Application.TournamentContext.Commands.Start
{
    public class StartTournamentCommandHandler : Util, IRequestHandler<StartTournamentCommand, bool>
    {
        private readonly MySqlContext _sqlContext;

        public StartTournamentCommandHandler(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<bool> Handle(StartTournamentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var _tournament = await _sqlContext.Set<Tournament>()
                                            .Include(t => t.Matches)
                                            .Include(t => t.Teams)
                                                .ThenInclude(t => t.Condominium)
                                            .Where(t => t.Id == request.TournamentID)
                                            .FirstOrDefaultAsync();

                var _mode = _tournament.Mode == PS.Game.Domain.Enums.eMode.Both ? PS.Game.Domain.Enums.eMode.Solo : _tournament.Mode;
                var _count = _tournament.Mode == PS.Game.Domain.Enums.eMode.Both ? 2 : 1;

                while (_count > 0)
                {
                    var _teams = _tournament.Teams.Where(t => t.Active &&
                                                                     t.Status == PS.Game.Domain.Enums.eStatus.Finished &&
                                                                     t.Mode == _mode)
                                                         .OrderByDescending(t => t.PaymentDate.Value)
                                                         .ToList();

                    var _condominiums = _teams.Select(t => t.Condominium).Distinct().ToList();

                    var _skip = false;

                    // Confrontos entre condomínios
                    if (_tournament.Matches.Count == 0)
                    {
                        foreach (var _condominium in _condominiums)
                        {
                            var _group = _teams.Where(t => t.CondominiumID == _condominium.Id).ToList();

                            if (_group.Count > 1)
                            {
                                _skip = true;

                                var _matches = GenerateSwitching(_group, _tournament, _mode);

                                await _sqlContext.Matches.AddRangeAsync(_matches, cancellationToken);
                            }
                        }
                    }

                    if (!_skip)
                    {
                        if (_condominiums.Count % 2 == 0) // Pares - Chaveamento
                        {
                            var _matches = GenerateSwitching(_teams, _tournament, _mode);

                            await _sqlContext.Matches.AddRangeAsync(_matches, cancellationToken);
                        }
                        else // Ímpares - Liga
                        {
                            var _matches = GenerateLeague(_teams, _tournament, _mode);

                            await _sqlContext.Matches.AddRangeAsync(_matches, cancellationToken);
                        }
                    }

                    _mode = _tournament.Mode == PS.Game.Domain.Enums.eMode.Both ? PS.Game.Domain.Enums.eMode.Team : _tournament.Mode;
                    _count--;
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
