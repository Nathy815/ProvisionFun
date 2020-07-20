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

namespace Application.Services
{
    public class Hangfire : IHangfire
    {
        private readonly MySqlContext _sqlContext;
        private readonly IUtil _util;

        public Hangfire(MySqlContext sqlContext, IUtil util)
        {
            _sqlContext = sqlContext;
            _util = util;
        }

        public async Task<bool> GerarPartidas()
        {
            try
            {
                var _tournaments = await _sqlContext.Set<Tournament>()
                                            .Include(t => t.Teams)
                                                .ThenInclude(t => t.Condominium)
                                            .Where(t => t.Active &&
                                                        t.RoundSolo == eRound.NotStarted &&
                                                        t.RoundTeam == eRound.NotStarted &&
                                                        t.EndSubscryption <= DateTime.Now)
                                            .ToListAsync();

                foreach (var _tournament in _tournaments)
                {
                    var _modes = _tournament.Mode == eMode.Both ? 2 : 1;

                    while (_modes > 0)
                    {
                        var _mode = _tournament.Mode == eMode.Solo || _modes == 2 ? eMode.Solo : eMode.Team;

                        var _teams = _tournament.Teams.Where(t => t.Active && t.Mode == _mode && t.Status == eStatus.Finished).OrderBy(t => t.PaymentDate).ToList();
                        var _condominiums = _teams.Select(t => t.Condominium).Distinct().ToList();

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
                                var _matches = new List<Match>();

                                if (_group.Count > 1)
                                {
                                    if (_group.Count % 2 == 0) // Número par
                                        _matches = _util.GenerateSwitching(_group, _tournament, _mode);
                                    else // Número ímpar
                                        _matches = _util.GenerateLeague(_group, _tournament, _mode);

                                    await _sqlContext.Matches.AddRangeAsync(_matches);
                                }
                            }
                        }
                        else
                        {
                            var _matches = new List<Match>();
                            if (_teams.Count == 16 || (_teams.Count > 16 && _teams.Count % 2 == 0)) // 16 ou par maior que 16
                            {
                                if (_mode == eMode.Solo)
                                    _tournament.RoundSolo = _teams.Count == 16 ? eRound.Fase4 : eRound.Fase2;
                                else
                                    _tournament.RoundTeam = _teams.Count == 16 ? eRound.Fase4 : eRound.Fase2;

                                _matches = _util.GenerateSwitching(_teams, _tournament, _mode);
                            }
                            else // Número ímpar ou menor que 16
                            {
                                if (_mode == eMode.Solo)
                                    _tournament.RoundSolo = eRound.Fase3;
                                else
                                    _tournament.RoundTeam = eRound.Fase3;

                                _matches = _util.GenerateLeague(_teams, _tournament, _mode);
                            }

                            await _sqlContext.Matches.AddRangeAsync(_matches);
                        }

                        _modes -= 1;
                    }

                    _sqlContext.Tournaments.Update(_tournament);
                }

                await _sqlContext.SaveChangesAsync();

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> GerarRodadas()
        {
            try
            {
                var _tournaments = await _sqlContext.Set<Tournament>()
                                            .Include(t => t.Teams)
                                                .ThenInclude(t => t.Condominium)
                                            .Include(t => t.Matches)
                                            .Where(t => t.Active && 
                                                        (t.RoundSolo != eRound.NotStarted ||
                                                        t.RoundTeam != eRound.NotStarted))
                                            .ToListAsync();

                foreach (var _tournament in _tournaments)
                {
                    var _modes = _tournament.Mode == eMode.Both ? 2 : 1;

                    while (_modes > 0)
                    {
                        var _mode = _tournament.Mode == eMode.Solo || _modes == 2 ? eMode.Solo : eMode.Team;
                        var _round = _mode == eMode.Solo ? _tournament.RoundSolo : _tournament.RoundTeam;
                        var _matches = new List<Match>();

                        var _teams = _tournament.Teams.Where(t => t.Active && t.Status == eStatus.Finished).ToList();
                        
                        

                        await _sqlContext.Matches.AddRangeAsync(_matches);
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
