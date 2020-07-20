using Domain.Entities;
using PS.Game.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModels
{
    public class GetTournamentQueryVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartSubscryption { get; set; }
        public DateTime EndSubscryption { get; set; }
        public int SubscryptionLimit { get; set; }
        public int PlayerLimit { get; set; }
        public string Plataform { get; set; }
        public eMode Mode { get; set; }
        public Guid GameID { get; set; }
        public string Game { get; set; }

        public GetTournamentQueryVM(Tournament tournament)
        {
            Id = tournament.Id;
            Name = tournament.Name;
            Plataform = tournament.Plataform;
            StartSubscryption = tournament.StartSubscryption;
            EndSubscryption = tournament.EndSubscryption;
            SubscryptionLimit = tournament.SubscryptionLimit;
            PlayerLimit = tournament.PlayerLimit;
            Mode = tournament.Mode;
            GameID = tournament.Game.Id;
            Game = tournament.Game.Name;
        }
    }
}
