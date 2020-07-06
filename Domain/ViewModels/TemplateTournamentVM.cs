using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.ViewModels
{
    public class TemplateGameVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<TemplateTournamentVM> Tournaments { get; set; }

        public TemplateGameVM(Game game)
        {
            Id = game.Id;
            Name = game.Name;

            Tournaments = new List<TemplateTournamentVM>();
            foreach (var _tournament in game.Tournaments)
                if (_tournament.Active) Tournaments.Add(new TemplateTournamentVM(_tournament));
        }
    }

    public class TemplateTournamentVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Plataform { get; set; }
        public eMode Mode { get; set; }
        public eTournamentStatus Status { get; set; }

        public TemplateTournamentVM(Tournament tournament)
        {
            Id = tournament.Id;
            Name = tournament.Name;
            Plataform = tournament.Plataform;
            Mode = tournament.Mode;

            var _soloCount = tournament.Teams.Where(t => t.Active && t.Mode == eMode.Solo).ToList().Count;
            var _teamCount = tournament.Teams.Where(t => t.Active && t.Mode == eMode.Team).ToList().Count;

            if (tournament.StartSubscryption < DateTime.Now)
                Status = eTournamentStatus.Soon;
            else if (tournament.EndSubscryption > DateTime.Now ||
                    (Mode == eMode.Solo && tournament.SubscryptionLimit == _soloCount) ||
                    (Mode == eMode.Team && tournament.SubscryptionLimit == _teamCount) ||
                    (Mode == eMode.Both && tournament.SubscryptionLimit == _soloCount && tournament.SubscryptionLimit == _teamCount))
                Status = eTournamentStatus.Closed;
            else
                Status = eTournamentStatus.Open;
        }
    }

}
