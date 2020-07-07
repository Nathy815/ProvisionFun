using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.Interfaces
{
    public interface IBoleto
    {
        byte[] GeneratePayment(Team team);
    }
}
