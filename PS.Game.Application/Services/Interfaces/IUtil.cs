using Domain.Entities;
using Microsoft.AspNetCore.Http;
using PS.Game.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PS.Game.Application.Services.Interfaces
{
    public interface IUtil
    {
        string HashPassword(string password);
        bool VerifyPassword(string hasehdPassword, string password);
        string UploadFile(IFormFile file, string name, string virtualPath);
        string GetFileName(string name);
        List<Match> GenerateLeague(List<Team> _teams, Tournament _tournament, eMode _mode);
        List<Match> GenerateSwitching(List<Team> _teams, Tournament _tournament, eMode _mode);
    }
}
