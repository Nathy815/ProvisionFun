using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModels
{
    public class GetSubscryptionVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Tournament { get; set; }
        public int Players { get; set; }
        public PS.Game.Domain.Enums.eStatus Status { get; set; }

        public GetSubscryptionVM(Entities.Team team)
        {
            Id = team.Id;
            Name = team.Name;
            Tournament = team.Tournament.Name;
            Players = team.Players.Count;
            Status = team.Status;
        }
    }
}
