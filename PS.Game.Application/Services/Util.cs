using Application.Services.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using PS.Game.Application.Services.Interfaces;
using PS.Game.Domain.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Application.Services
{ 
    public class Util : IUtil
    {
        private static string pathToSave { get; set; }
        private readonly string virtualPath = "http://provisionfun.com.br/cgi-bin/resources/";
        private readonly IEmail _email;

        public Util(IEmail email)
        {
            pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "Resources/");
            _email = email;
        }
        
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string hashedPassword, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        public string UploadFile(IFormFile file, string name)
        {
            try
            {
                var fileExtension = string.Empty;
                if (file.ContentType != null)
                    fileExtension = file.ContentType.Split("/")[1];

                else
                {
                    string[] split = file.FileName.Split(".");
                    fileExtension = split[split.Length - 1];
                }
                var filename = name + "." + fileExtension;
                var fullPath = Path.Combine(pathToSave, filename);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return virtualPath + filename;
            }
            catch(Exception ex)
            {
                var _message = ex.Message;
                if (ex.InnerException != null)
                    _message += " | Inner Exception: " + ex.InnerException;
                if (ex.StackTrace != null)
                    _message += " | Trace: " + ex.StackTrace;

                _email.SendLog("UploadFile", _message);

                return null;
            }
        }

        public string GetFileName(string name)
        {
            return string.Format("{0}_{1}-{2}-{3}_{4}{5}.REM",
                                  name,
                                  DateTime.Now.Year,
                                  DateTime.Now.Month.ToString().PadLeft(2, '0'),
                                  DateTime.Now.Day.ToString().PadLeft(2, '0'),
                                  DateTime.Now.Hour.ToString().PadLeft(2, '0'),
                                  DateTime.Now.Minute.ToString().PadLeft(2, '0'));
        }

        public List<Match> GenerateLeague(List<Team> _teams, Tournament _tournament, eMode _mode)
        {
            var _sequence = 1;
            var _list = new List<Match>();
            var _count = _teams.Count * (_teams.Count - 1) / 2;
            var _start = 0;
            var _end = _teams.Count - 1;
            var _player1 = _start;
            var _player2 = _end;

            while (_sequence < _count)
            {
                while (_player1 != _player2)
                {
                    var _match = new Match
                    {
                        Id = Guid.NewGuid(),
                        Player1ID = _teams.ElementAt(_player1).Id,
                        Player2ID = _teams.ElementAt(_player2).Id,
                        Round = _mode == eMode.Solo ? _tournament.RoundSolo : _tournament.RoundTeam,
                        TournamentID = _tournament.Id,
                        Sequence = _sequence
                    };

                    _list.Add(_match);

                    _sequence += 1;
                    _player1 = _player1 + 1 == _teams.Count ? 0 : _player1 + 1;
                    _player2 = _player2 - 1 < 0 ? _teams.Count - 1 : _player2 - 1;
                }

                _player1 = _start - 1 < 0 ? _teams.Count - 1 : _start - 1;
                _player2 = _end - 1 < 0 ? _teams.Count - 1 : _end - 1;
                _start = _player1;
                _end = _player2;
            }

            return _list;
        }

        public List<Match> GenerateSwitching(List<Team> _teams, Tournament _tournament, eMode _mode)
        {
            var _sequence = 1;
            var _player1 = 0;
            var _list = new List<Match>();
            _teams = _teams.OrderBy(t => t.PaymentDate).ToList();

            while (_player1 < _teams.Count)
            {
                var _player2 = _player1 + 1;

                var _match = new Match
                {
                    Id = Guid.NewGuid(),
                    Player1ID = _teams.ElementAt(_player1).Id,
                    Player2ID = _teams.ElementAt(_player2).Id,
                    Sequence = _sequence,
                    TournamentID = _tournament.Id,
                    Round = _mode == eMode.Solo ? _tournament.RoundSolo : _tournament.RoundTeam
                };

                _list.Add(_match);

                _player1 += 2;
                _sequence += 1;
            }

            return _list;
        }
    }
}
