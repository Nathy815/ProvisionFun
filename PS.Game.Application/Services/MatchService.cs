using Application.Services.Interfaces;
using Domain.Entities;
using PS.Game.Application.Services.Interfaces;
using PS.Game.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Game.Application.Services
{
    public class MatchService : IMatchService
    {
        private readonly IEmail _email;

        public MatchService(IEmail email)
        {
            _email = email;
        }

        public Task<List<Match>> GenerateRound1(Tournament _tournament, eMode _mode)
        {
            try
            {
                var _teams = _tournament.Teams.Where(t => t.Active && t.Mode == _mode && t.Status == eStatus.Finished).OrderBy(t => t.PaymentDate).ToList();
                var _condominiums = _teams.Select(t => t.Condominium).Distinct().ToList();
                var _matches = new List<Match>();

                if (_teams.Count > 0)
                {
                    if (_teams.Count != _condominiums.Count) // Se houver competidores do mesmo condomínio
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
                                    _matches.AddRange(GenerateSwitching(_group, _tournament, _mode));
                                else // Número ímpar
                                    _matches.AddRange(GenerateLeague(_group, _tournament, _mode));
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

                            _matches.AddRange(GenerateSwitching(_teams, _tournament, _mode));
                        }
                        else // Número ímpar ou menor que 16
                        {
                            if (_mode == eMode.Solo)
                                _tournament.RoundSolo = eRound.Fase3;
                            else
                                _tournament.RoundTeam = eRound.Fase3;

                            _matches.AddRange(GenerateLeague(_teams, _tournament, _mode));
                        }
                    }
                }

                return Task.Run(() => { return _matches; });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Task<List<Match>> GenerateRound2(Tournament _tournament, eMode _mode)
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

                    _matches.AddRange(GenerateSwitching(_teams, _tournament, _mode));
                }
                else if (_teams.Count % 2 == 0 && _teams.Count > 16) // Se for par e maior que 16, gera chaveamento
                {
                    if (_mode == eMode.Solo)
                        _tournament.RoundSolo = eRound.Fase2;
                    else
                        _tournament.RoundTeam = eRound.Fase2;

                    _matches.AddRange(GenerateSwitching(_teams, _tournament, _mode));
                }
                else // Se for ímpar ou menor que 16, gera partidas todos contra todos
                {
                    if (_mode == eMode.Solo)
                        _tournament.RoundSolo = eRound.Fase3;
                    else
                        _tournament.RoundTeam = eRound.Fase3;

                    _matches.AddRange(GenerateLeague(_teams, _tournament, _mode));
                }

                return Task.Run(() => { return _matches; });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Task<List<Match>> GenerateRound3(Tournament _tournament, eMode _mode)
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

                    _matches.AddRange(GenerateSwitching(_teams, _tournament, _mode));
                }
                else
                {
                    if (_mode == eMode.Solo)
                        _tournament.RoundSolo = eRound.Fase3;
                    else
                        _tournament.RoundTeam = eRound.Fase3;

                    _matches.AddRange(GenerateLeague(_teams, _tournament, _mode));
                }

                return Task.Run(() => { return _matches; });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Task<List<Match>> GenerateRound4(Tournament _tournament, eMode _mode)
        {
            try
            {
                var _teams = _tournament.Teams.Where(t => t.Active &&
                                                          t.Mode == _mode &&
                                                          t.Status == eStatus.Finished).ToList();
                var _matches = new List<Match>();

                if (_teams.Count == 16) // Top
                {
                    _matches.AddRange(GenerateSwitching(_teams, _tournament, _mode));
                }
                else // Melhores do campeonato
                {
                    var _top = _teams.OrderByDescending(t => t.MatchesAsPlayer1.Where(m => m.Active && m.Winner.HasValue && m.Winner == t.Id).Count() +
                                                             t.MatchesAsPlayer2.Where(m => m.Active && m.Winner.HasValue && m.Winner == t.Id).Count())
                                     .Take(16)
                                     .ToList();

                    _matches.AddRange(GenerateSwitching(_top, _tournament, _mode));
                }

                return Task.Run(() => { return _matches; });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<List<Team>> EliminateTeams(List<Team> _teams, eMode _mode, eRound _round)
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
                        {
                            _team.Status = eStatus.Eliminated;
                            await _email.SendEmail(_team, eStatus.Eliminated);
                        }
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
                    {
                        _team.Status = eStatus.Eliminated;
                        await _email.SendEmail(_team, eStatus.Eliminated);
                    }
                }

                return _teams;
            }
            catch (Exception ex)
            {
                return _teams;
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
