using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace PS.Game.Application.SubscryptionConfigurationContext.Commands.BankSlip
{
    public class BankSlipCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public BankSlipCommand(Guid id)
        {
            Id = id;
        }
    }
}
