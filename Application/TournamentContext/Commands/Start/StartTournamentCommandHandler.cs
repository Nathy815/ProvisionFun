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
    public class StartTournamentCommandHandler : IRequestHandler<StartTournamentCommand, bool>
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

                var _mode = _tournament.Mode == Domain.Enums.eMode.Both ? Domain.Enums.eMode.Solo : _tournament.Mode;
                var _count = _tournament.Mode == Domain.Enums.eMode.Both ? 2 : 1;

                while (_count > 0)
                {
                    var _teams = _tournament.Teams.Where(t => t.Active &&
                                                                     t.Status == Domain.Enums.eStatus.Finished &&
                                                                     t.Mode == _mode)
                                                         .OrderByDescending(t => t.PaymentDate.Value)
                                                         .ToList();

                    var _condominiums = _teams.Select(t => t.Condominium).Distinct().ToList();

                    if (_condominiums.Count % 2 == 0) // Pares - Chaveamento
                    {
                        var _sequence = 1; // 1
                        var _player1 = 0; // 0

                        while (_player1 < _teams.Count) // 0 < 5 | 2 < 5
                        {
                            var _player2 = _player1 + 1; // 1 | 3

                            var _match = new Match
                            {
                                Id = Guid.NewGuid(),
                                Player1ID = _teams.ElementAt(_player1).Id, // 0 | 2
                                Player2ID = _teams.ElementAt(_player2).Id, // 1 | 3
                                Sequence = _sequence, // 1 | 2
                                TournamentID = _tournament.Id
                            };

                            await _sqlContext.Matches.AddAsync(_match);

                            _player1 += 2; // 2 | 4
                            _sequence += 1; // 1 | 2
                        }
                    }
                    else // Ímpares - Liga
                    {
                        
                    }

                    _mode = _tournament.Mode == Domain.Enums.eMode.Both ? Domain.Enums.eMode.Team : _tournament.Mode;
                    _count--;
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
