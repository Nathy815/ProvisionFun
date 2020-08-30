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
        Task<string> GeneratePayment(Team team);
        Task<string> GenerateShipping(List<Team> teams);
        Task<int?> ImportReturn(IFormFile file);
    }
}