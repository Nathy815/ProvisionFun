using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IBoleto
    {
        Task<byte[]> GeneratePayment(Team team);
        Task<string> GenerateShipping(List<Team> teams, string virtualPath);
        Task<bool> ImportReturn(IFormFile file, string virtualPath);
    }
}