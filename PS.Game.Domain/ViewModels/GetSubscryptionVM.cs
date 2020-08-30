using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public string Player { get; set; }
        public string CPF { get; set; }

        public GetSubscryptionVM(Entities.Team team)
        {
            Id = team.Id;
            Name = team.Name;
            Tournament = team.Tournament.Name;
            Players = team.Players.Count;
            Status = team.Status;
            var _player = team.Players.Where(p => p.IsPrincipal).Select(p => p.Player).FirstOrDefault();
            if (_player != null)
            {
                CPF = _player.CPF;
                Player = _player.Name;
            }
        }
    }
}
