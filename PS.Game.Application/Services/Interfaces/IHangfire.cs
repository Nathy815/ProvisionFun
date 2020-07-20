using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PS.Game.Application.Services.Interfaces
{
    public interface IHangfire
    {
        Task<bool> GerarPartidas();
    }
}
