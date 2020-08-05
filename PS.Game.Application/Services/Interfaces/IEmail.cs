using Domain.Entities;
using PS.Game.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IEmail
    {
        Task<bool> SendEmail(Team team, eStatus status, string attach = null, Match match = null, bool? alter = null);

        Task<bool> SendLog(string title, string message);
    }
}