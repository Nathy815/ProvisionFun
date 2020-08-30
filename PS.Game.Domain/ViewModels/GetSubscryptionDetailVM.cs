using System;
using System.Collections.Generic;
using System.Linq;
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
            var _players = team.Players.OrderBy(p => p.IsPrincipal);
            foreach (var _player in _players)
                Players.Add(new GetSubscryptionPlayerVM(_player.Player, _player.IsPrincipal));
        }
    }

    public class GetSubscryptionPlayerVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Cellphone { get; set; }
        public DateTime BirthDate { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public string Document { get; set; }

        public GetSubscryptionPlayerVM(Player player, bool isPrincipal)
        {
            Id = player.Id;
            Name = player.Name + (isPrincipal ? " *" : "");
            Cellphone = player.Cellphone;
            BirthDate = player.BirthDate;
            CPF = player.CPF;
            Email = player.Email;
            Document = player.Document;
        }
    }
}
