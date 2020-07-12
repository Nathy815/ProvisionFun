using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModels
{
    public class GetMatchQueryVM
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Tournament { get; set; }
        public string Game { get; set; }
        public GetMatchQueryPlayerVM Player1 { get; set; }
        public GetMatchQueryPlayerVM Player2 { get; set; }

        public GetMatchQueryVM(Match match)
        {
            Id = match.Id;
            Date = match.Date.Value;
            Tournament = match.Tournament.Name;
            Game = match.Tournament.Game.Name;
            Player1 = new GetMatchQueryPlayerVM(match.Player1);
            Player2 = new GetMatchQueryPlayerVM(match.Player2);
        }
    }

    public class GetMatchQueryPlayerVM
    {
        public Guid Id { get; set; }
        public int Color { get; set; }
        public int Icon { get; set; }
        public string Name { get; set; }

        public GetMatchQueryPlayerVM(Team team)
        {
            Id = team.Id;
            Color = team.Color;
            Icon = team.Icon;
            Name = team.Name;
        }
    }
}
