using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;

namespace Domain.ViewModels
{
    public class GetSubscryptionDetailVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Tournament { get; set; }
        public PS.Game.Domain.Enums.eStatus Status { get; set; }
        public GetCondominiumQueryVM Condominium { get; set; }
        public List<GetSubscryptionPlayerVM> Players { get; set; }

        public GetSubscryptionDetailVM(Team team)
        {
            Id = team.Id;
            Name = team.Name;
            Tournament = team.Tournament.Name;
            Status = team.Status;

            Condominium = new GetCondominiumQueryVM(team.Condominium);

            Players = new List<GetSubscryptionPlayerVM>();
            foreach (var _player in team.Players)
                Players.Add(new GetSubscryptionPlayerVM(_player.Player));
        }
    }

    public class GetSubscryptionPlayerVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public string Document { get; set; }

        public GetSubscryptionPlayerVM(Player player)
        {
            Id = player.Id;
            Name = player.Name;
            BirthDate = player.BirthDate;
            CPF = player.CPF;
            Email = player.Email;
            Document = player.Document;
        }
    }
}
