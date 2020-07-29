using Domain.Entities;
using PS.Game.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using PS.Game.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services
{
    public class BackgroundService : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public BackgroundService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var _scope = _scopeFactory.CreateScope())
            {
                var _sqlContext = _scope.ServiceProvider.GetRequiredService<MySqlContext>();

                while (true)
                {
                    await GerarPartidas(_sqlContext, cancellationToken);

                    await Task.Delay(3600000, cancellationToken);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> GerarPartida(Tournament _tournament, MySqlContext _sqlContext, eMode _mode, CancellationToken token)
        {
            try
            {
                var _round = _mode == eMode.Solo ? _tournament.RoundSolo : _tournament.RoundTeam;
                var _matches = new List<Match>();

                var _teams = _tournament.Teams.Where(t => t.Active && t.Mode == _mode && t.Status == eStatus.Finished).ToList();

                if (_round == eRound.Fase4) // Se tiver na Fase 4, gera próximo chaveamento
                    _matches = GenerateSwitching(_teams, _tournament, _mode);
                else if (_round == eRound.Fase2) // Na Fase 2, verifica se dá pra pular para Fase 4 antes de gerar Fase 3
                {
                    if (_teams.Count == 16)
                    {
                        if (_mode == eMode.Solo)
                            _tournament.RoundSolo = eRound.Fase4;
                        else
                            _tournament.RoundTeam = eRound.Fase4;

                        _matches = GenerateSwitching(_teams, _tournament, _mode);
                    }
                    else
                    {
                        if (_mode == eMode.Solo)
                            _tournament.RoundSolo = eRound.Fase3;
                        else
                            _tournament.RoundTeam = eRound.Fase3;

                        _matches = GenerateLeague(_teams, _tournament, _mode);
                    }
                }
                else
                {
                    // Tanto na Fase 1 quanto na Fase 3, tem que alterar o status dos eliminados
                    _teams = await EliminateTeams(_teams, _mode, _round);

                    _teams = _teams.Where(t => t.Status == eStatus.Finished).ToList();

                    // Se der número ímpar e estiver na Fase 1, pula para a fase 3
                    if (_teams.Count % 2 != 0 && _round == eRound.Fase1)
                    {
                        if (_mode == eMode.Solo)
                            _tournament.RoundSolo = eRound.Fase3;
                        else
                            _tournament.RoundTeam = eRound.Fase3;

                        _round = eRound.Fase3;
                    }

                    // Se sobrarem 16 competidores, pula para a Fase 4
                    if (_teams.Count == 16)
                    {
                        if (_mode == eMode.Solo)
                            _tournament.RoundSolo = eRound.Fase4;
                        else
                            _tournament.RoundTeam = eRound.Fase4;

                        _matches = GenerateSwitching(_teams, _tournament, _mode);
                    }
                    else
                    {
                        if (_round == eRound.Fase1)
                        {
                            if (_mode == eMode.Solo)
                                _tournament.RoundSolo = eRound.Fase2;
                            else
                                _tournament.RoundTeam = eRound.Fase2;

                            _matches = GenerateSwitching(_teams, _tournament, _mode);
                        }
                        else // Só será Fase 3, se atender à segunda condição desse else
                            _matches = GenerateLeague(_teams, _tournament, _mode);
                    }
                }

                _sqlContext.Tournaments.Update(_tournament);

                await _sqlContext.Matches.AddRangeAsync(_matches, token);

                await _sqlContext.SaveChangesAsync(token);

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> GerarPartidas(MySqlContext _sqlContext, CancellationToken token)
        {
            try
            {
                var _tournaments = await _sqlContext.Set<Tournament>()
                                                .Include(t => t.Matches)
                                                .Include(t => t.Teams)
                                                    .ThenInclude(t => t.Condominium)
                                                .Where(t => t.Active &&
                                                            t.EndSubscryption < DateTime.Now)
                                                .ToListAsync();

                foreach (var _tournament in _tournaments)
                {
                    var _modes = _tournament.Mode == eMode.Both ? 2 : 1;

                    while (_modes > 0)
                    {
                        var _mode = _tournament.Mode == eMode.Solo || _modes == 2 ? eMode.Solo : eMode.Team;
                        var _round = _mode == eMode.Solo ? _tournament.RoundSolo : _tournament.RoundTeam;
                        var _matches = new List<Match>();

                        if (_round == eRound.NotStarted)
                            _matches = await GenerateRound1(_tournament, _mode);
                        else
                        {
                            var _countMatches = _tournament.Matches.Count;
                            var _finishedMatches = _tournament.Matches.Where(m => m.Active &&
                                                                                  m.Round == _round &&
                                                                                  !m.Winner.HasValue)
                                                                      .ToList().Count;

                            if (_countMatches == 0 || _countMatches == _finishedMatches)
                            {
                                switch (_round)
                                {
                                    case eRound.Fase1:
                                        var _teams = _tournament.Teams.Where(t => t.Active && t.Mode == _mode && t.Status == eStatus.Finished).OrderBy(t => t.PaymentDate).ToList();
                                        var _condominiums = _teams.Select(t => t.Condominium).Distinct().ToList();

                                        if (_teams.Count != _condominiums.Count)
                                            _matches = await GenerateRound1(_tournament, _mode);
                                        else
                                            _matches = await GenerateRound2(_tournament, _mode);

                                        break;
                                    case eRound.Fase2:
                                        _matches = await GenerateRound2(_tournament, _mode);

                                        break;
                                    case eRound.Fase3:
                                        _matches = await GenerateRound3(_tournament, _mode);

                                        break;
                                    case eRound.Fase4:
                                        if (!_tournament.Teams.Any(t => t.Active && t.Mode == _mode && t.Status == eStatus.Winner))
                                            _matches = await GenerateRound4(_tournament, _mode);

                                        break;
                                }
                            }
                        }
                        
                        await _sqlContext.Matches.AddRangeAsync(_matches, token);

                        _modes -= 1;
                    }

                    if (TournamentEnded(_tournament))
                        _tournament.Active = false;

                    _sqlContext.Tournaments.Update(_tournament);
                }

                await _sqlContext.SaveChangesAsync(token);

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        private Task<List<Match>> GenerateRound1(Tournament _tournament, eMode _mode)
        {
            try
            {
                var _teams = _tournament.Teams.Where(t => t.Active && t.Mode == _mode && t.Status == eStatus.Finished).OrderBy(t => t.PaymentDate).ToList();
                var _condominiums = _teams.Select(t => t.Condominium).Distinct().ToList();
                var _matches = new List<Match>();

                // Se houver competidores do mesmo condomínio
                if (_teams.Count != _condominiums.Count)
                {
                    // Fase 1: competição entre competidores do mesmo condomínio
                    if (_mode == eMode.Solo)
                        _tournament.RoundSolo = eRound.Fase1;
                    else
                        _tournament.RoundTeam = eRound.Fase1;
                    
                    foreach (var _condominium in _condominiums)
                    {
                        var _group = _teams.Where(t => t.CondominiumID == _condominium.Id).ToList();

                        if (_group.Count > 1)
                        {
                            if (_group.Count % 2 == 0) // Número par
                                _matches = GenerateSwitching(_group, _tournament, _mode);
                            else // Número ímpar
                                _matches = GenerateLeague(_group, _tournament, _mode);
                        }
                    }
                }
                else
                {
                    if (_teams.Count == 16 || (_teams.Count > 16 && _teams.Count % 2 == 0)) // 16 ou par maior que 16
                    {
                        if (_mode == eMode.Solo)
                            _tournament.RoundSolo = _teams.Count == 16 ? eRound.Fase4 : eRound.Fase2;
                        else
                            _tournament.RoundTeam = _teams.Count == 16 ? eRound.Fase4 : eRound.Fase2;

                        _matches = GenerateSwitching(_teams, _tournament, _mode);
                    }
                    else // Número ímpar ou menor que 16
                    {
                        if (_mode == eMode.Solo)
                            _tournament.RoundSolo = eRound.Fase3;
                        else
                            _tournament.RoundTeam = eRound.Fase3;

                        _matches = GenerateLeague(_teams, _tournament, _mode);
                    }
                }

                return Task.Run(() => { return _matches; });
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        private Task<List<Match>> GenerateRound2(Tournament _tournament, eMode _mode)
        {
            try
            {
                var _teams = _tournament.Teams.Where(t => t.Active && 
                                                          t.Mode == _mode &&
                                                          t.Status == eStatus.Finished).ToList();
                var _matches = new List<Match>();

                if (_teams.Count == 16) // Top
                {
                    if (_mode == eMode.Solo)
                        _tournament.RoundSolo = eRound.Fase4;
                    else
                        _tournament.RoundTeam = eRound.Fase4;

                    _matches = GenerateSwitching(_teams, _tournament, _mode);
                }
                else if (_teams.Count % 2 == 0 && _teams.Count > 16) // Se for par e maior que 16, gera chaveamento
                {
                    if (_mode == eMode.Solo)
                        _tournament.RoundSolo = eRound.Fase2;
                    else
                        _tournament.RoundTeam = eRound.Fase2;

                    _matches = GenerateSwitching(_teams, _tournament, _mode);
                }
                else // Se for ímpar ou menor que 16, gera partidas todos contra todos
                {
                    if (_mode == eMode.Solo)
                        _tournament.RoundSolo = eRound.Fase3;
                    else
                        _tournament.RoundTeam = eRound.Fase3;

                    _matches = GenerateLeague(_teams, _tournament, _mode);
                }

                return Task.Run(() => { return _matches; });
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        private Task<List<Match>> GenerateRound3(Tournament _tournament, eMode _mode)
        {
            try
            {
                var _teams = _tournament.Teams.Where(t => t.Active &&
                                                          t.Mode == _mode &&
                                                          t.Status == eStatus.Finished).ToList();
                var _matches = new List<Match>();

                if (_teams.Count == 16) // Top
                {
                    if (_mode == eMode.Solo)
                        _tournament.RoundSolo = eRound.Fase4;
                    else
                        _tournament.RoundTeam = eRound.Fase4;

                    _matches = GenerateSwitching(_teams, _tournament, _mode);
                }
                else
                {
                    if (_mode == eMode.Solo)
                        _tournament.RoundSolo = eRound.Fase3;
                    else
                        _tournament.RoundTeam = eRound.Fase3;

                    _matches = GenerateLeague(_teams, _tournament, _mode);
                }

                return Task.Run(() => { return _matches; });
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        private Task<List<Match>> GenerateRound4(Tournament _tournament, eMode _mode)
        {
            try
            {
                var _teams = _tournament.Teams.Where(t => t.Active && 
                                                          t.Mode == _mode &&
                                                          t.Status == eStatus.Finished).ToList();
                var _matches = new List<Match>();

                if (_teams.Count == 16) // Top
                {
                    _matches = GenerateSwitching(_teams, _tournament, _mode);
                }
                else // Melhores do campeonato
                {
                    var _top = _teams.OrderByDescending(t => t.MatchesAsPlayer1.Where(m => m.Active && m.Winner.HasValue && m.Winner == t.Id).Count() +
                                                             t.MatchesAsPlayer2.Where(m => m.Active && m.Winner.HasValue && m.Winner == t.Id).Count())
                                     .Take(16)
                                     .ToList();

                    _matches = GenerateSwitching(_top, _tournament, _mode);
                }

                return Task.Run(() => { return _matches; });
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        private Task<List<Team>> EliminateTeams(List<Team> _teams, eMode _mode, eRound _round)
        {
            try
            {
                if (_round == eRound.Fase1) // Eliminar os que tiverem menos vitórias
                {
                    // Nas fase 1, eliminar quem tem menos vitórias no condomínio
                    var _condominiums = _teams.Select(t => t.CondominiumID).Distinct().ToList();
                    foreach (var _id in _condominiums)
                    {
                        var _duplicateTeams = _teams.Where(t => t.CondominiumID == _id)
                                                    .OrderByDescending(t => (
                                                            t.MatchesAsPlayer1.Where(m => m.Winner == t.Id).Count() +
                                                            t.MatchesAsPlayer2.Where(m => m.Winner == t.Id).Count()
                                                    ))
                                                    .Skip(1)
                                                    .ToList();

                        foreach (var _team in _duplicateTeams)
                            _team.Status = eStatus.Eliminated;
                    }
                }
                else
                {
                    var _duplicateTeams = _teams.OrderByDescending(t => (
                                                        t.MatchesAsPlayer1.Where(m => m.Winner == t.Id).Count() +
                                                        t.MatchesAsPlayer2.Where(m => m.Winner == t.Id).Count()
                                                ))
                                                .Skip(16)
                                                .ToList();

                    foreach (var _team in _duplicateTeams)
                        _team.Status = eStatus.Eliminated;
                }

                return Task.Run(() => { return _teams; });
            }
            catch (Exception ex)
            {
                return Task.Run(() => { return _teams; });
            }
        }

        private List<Match> GenerateLeague(List<Team> _teams, Tournament _tournament, eMode _mode)
        {
            var _sequence = 1;
            var _list = new List<Match>();
            var _count = _teams.Count * (_teams.Count - 1) / 2;
            var _start = 0;
            var _end = _teams.Count - 1;
            var _player1 = _start;
            var _player2 = _end;

            while (_sequence < _count)
            {
                while (_player1 != _player2)
                {
                    var _match = new Match
                    {
                        Id = Guid.NewGuid(),
                        Player1ID = _teams.ElementAt(_player1).Id,
                        Player2ID = _teams.ElementAt(_player2).Id,
                        Round = _mode == eMode.Solo ? _tournament.RoundSolo : _tournament.RoundTeam,
                        TournamentID = _tournament.Id,
                        Sequence = _sequence
                    };

                    _list.Add(_match);

                    _sequence += 1;
                    _player1 = _player1 + 1 == _teams.Count ? 0 : _player1 + 1;
                    _player2 = _player2 - 1 < 0 ? _teams.Count - 1 : _player2 - 1;
                }

                _player1 = _start - 1 < 0 ? _teams.Count - 1 : _start - 1;
                _player2 = _end - 1 < 0 ? _teams.Count - 1 : _end - 1;
                _start = _player1;
                _end = _player2;
            }

            return _list;
        }

        private List<Match> GenerateSwitching(List<Team> _teams, Tournament _tournament, eMode _mode)
        {
            var _sequence = 1;
            var _player1 = 0;
            var _list = new List<Match>();
            _teams = _teams.OrderBy(t => t.PaymentDate).ToList();

            while (_player1 < _teams.Count)
            {
                var _player2 = _player1 + 1;

                var _match = new Match
                {
                    Id = Guid.NewGuid(),
                    Player1ID = _teams.ElementAt(_player1).Id,
                    Player2ID = _teams.ElementAt(_player2).Id,
                    Sequence = _sequence,
                    TournamentID = _tournament.Id,
                    Round = _mode == eMode.Solo ? _tournament.RoundSolo : _tournament.RoundTeam
                };

                _list.Add(_match);

                _player1 += 2;
                _sequence += 1;
            }

            return _list;
        }

        private bool TournamentEnded(Tournament tournament)
        {
            var _soloWinner = tournament.Teams.Any(t => t.Active && t.Mode == eMode.Solo && t.Status == eStatus.Winner);
            var _teamWinner = tournament.Teams.Any(t => t.Active && t.Mode == eMode.Team && t.Status == eStatus.Winner);

            if ((tournament.Mode == eMode.Solo && _soloWinner) ||
                (tournament.Mode == eMode.Team && _teamWinner) ||
                (tournament.Mode == eMode.Both && _soloWinner && _teamWinner))
                return true;

            return false;
        }
    }
}