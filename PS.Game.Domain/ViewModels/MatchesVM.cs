using Domain.Entities;
using PS.Game.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PS.Game.Domain.ViewModels
{
    public class MatchesVM
    {
        public List<MatchVM> history { get; set; }
        public List<MatchVM> scheduled { get; set; }
        public List<MatchVM> pending { get; set; }

        public MatchesVM(List<Match> matches)
        {
            history = new List<MatchVM>();
            scheduled = new List<MatchVM>();
            pending = new List<MatchVM>();

            foreach(var match in matches)
            {
                if (match.Winner.HasValue)
                    history.Add(new MatchVM(match));
                else
                {
                    if (match.Date.HasValue)
                        scheduled.Add(new MatchVM(match));
                    else
                        pending.Add(new MatchVM(match));
                }
            }
        }
    }

    public class MatchVM
    {
        public Guid Id { get; set; }
        public eGame Game { get; set; }
        public string Tournament { get; set; }
        public PlayerVM Player1 { get; set; }
        public PlayerVM Player2 { get; set; }
        public DateTime? Date { get; set; }
        public Guid? Winner { get; set; }
        public double Player1Score { get; set; }
        public double Player2Score { get; set; }
        public Guid AuditorID { get; set; }
        public string Auditor { get; set; }
        public string Comments { get; set; }
        
        public MatchVM(Match match)
        {
            Id = match.Id;
            Game = match.Tournament.Game;
            Tournament = match.Tournament.Name;
            Player1 = new PlayerVM(match.Player1);
            Player2 = new PlayerVM(match.Player2);
            Date = match.Date;
            Winner = match.Winner;
            Player1Score = match.Player1Score;
            Player2Score = match.Player2Score;
            if (match.Auditor != null)
            {
                AuditorID = match.AuditorID.Value;
                Auditor = match.Auditor.Name;
            }
            Comments = match.Comments;
        }
    }

    public class PlayerVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Player { get; set; }
        public string CPF { get; set; }

        public PlayerVM(Team player)
        {
            Id = player.Id;
            Name = player.Name;
            var _player = player.Players.Where(p => p.IsPrincipal).FirstOrDefault();
            if (_player != null)
            {
                Player = _player.Player.Name;
                CPF = _player.Player.CPF;
            }
        }
    }
}
