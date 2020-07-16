using Domain.Entities;
using Microsoft.AspNetCore.Http;
using PS.Game.Application.Services.Interfaces;
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

        public Util()
        {
            pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "Resources");
        }
        
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string hashedPassword, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        public string UploadFile(IFormFile file, string name, string virtualPath)
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
            catch(Exception)
            {
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

        public List<Match> GenerateLeague(List<Team> _teams, Guid _tournamentID)
        {
            var _sequence = 1;
            var _list = new List<Match>();
            var _free = (_teams.Count + 1) / 2;

            while (_sequence < _teams.Count)
            {
                for (var i = 0; i < _free; i++) // 
                {
                    var _player1 = _free - 1 > 0 ? _free - 1 : _teams.Count - 1;
                    var _player2 = _free + 1 < _teams.Count ? _free + 1 : 0;

                    var _match = new Match
                    {
                        Id = Guid.NewGuid(),
                        Player1ID = _teams.ElementAt(_player1).Id,
                        Player2ID = _teams.ElementAt(_player2).Id,
                        Sequence = _sequence,
                        TournamentID = _tournamentID,
                        Type = Domain.Enums.eType.League
                    };

                    _list.Add(_match);
                }

                _free = _free - 1 > 0 ? _free - 1 : _teams.Count - 1;
                _sequence += 1;
            }

            return _list;
        }

        public List<Match> GenerateSwitching(List<Team> _teams, Guid _tournamentID)
        {
            var _sequence = 1;
            var _player1 = 0;
            var _list = new List<Match>();

            while (_player1 < _teams.Count)
            {
                var _player2 = _player1 + 1;

                var _match = new Match
                {
                    Id = Guid.NewGuid(),
                    Player1ID = _teams.ElementAt(_player1).Id,
                    Player2ID = _teams.ElementAt(_player2).Id,
                    Sequence = _sequence,
                    Type = Domain.Enums.eType.Tournament,
                    TournamentID = _tournamentID
                };

                _list.Add(_match);

                _player1 += 2;
                _sequence += 1;
            }

            return _list;
        }
    }
}
