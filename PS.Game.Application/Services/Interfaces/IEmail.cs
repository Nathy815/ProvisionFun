using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IEmail
    { 
        Task<bool> SendEmail(string email, eStatus status, string attach = null);
    }
}
