using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PS.Game.Application.Services.Interfaces
{
    public interface IMatchService
    {
        Task<List<Match>> GenerateRound1(Tournament _tournament, Domain.Enums.eMode _mode);

        Task<List<Match>> GenerateRound2(Tournament _tournament, Domain.Enums.eMode _mode);

        Task<List<Match>> GenerateRound3(Tournament _tournament, Domain.Enums.eMode _mode);

        Task<List<Match>> GenerateRound4(Tournament _tournament, Domain.Enums.eMode _mode);
    }
}
