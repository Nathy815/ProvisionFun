using Domain.Entities;
using PS.Game.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.ViewModels
{
    public class TemplateGameVM
    {
        //public Guid Id { get; set; }
        public eGame Game { get; set; }
        public List<TemplateTournamentVM> Tournaments { get; set; }

        public TemplateGameVM(eGame game, List<Tournament> tournaments)
        {
            //Id = game.Id;
            Game = game;
            Tournaments = new List<TemplateTournamentVM>();
            foreach (var _tournament in tournaments)
                Tournaments.Add(new TemplateTournamentVM(_tournament));
        }
    }

    public class TemplateTournamentVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Plataform { get; set; }
        public bool SoloAvailable { get; set; }
        public bool TeamAvailable { get; set; }
        public eTournamentStatus Status { get; set; }

        public TemplateTournamentVM(Tournament tournament)
        {
            Id = tournament.Id;
            Name = tournament.Name;
            Plataform = tournament.Plataform;

            var _soloCount = tournament.Teams.Where(t => t.Active && t.Mode == eMode.Solo).ToList().Count;
            var _teamCount = tournament.Teams.Where(t => t.Active && t.Mode == eMode.Team).ToList().Count;

            SoloAvailable = tournament.Mode == eMode.Team ? false : tournament.Mode != eMode.Team && _soloCount == tournament.SubscryptionLimit ? false : true;
            TeamAvailable = tournament.Mode == eMode.Solo ? false : tournament.Mode != eMode.Solo && _teamCount == tournament.SubscryptionLimit ? false : true;

            if (tournament.StartSubscryption > DateTime.Now)
                Status = eTournamentStatus.Soon;
            else if (tournament.EndSubscryption < DateTime.Now ||
                    (!SoloAvailable && !TeamAvailable))
                Status = eTournamentStatus.Closed;
            else
                Status = eTournamentStatus.Open;
        }
    }

}
