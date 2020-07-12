using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.SubscryptionConfigurationContext.Commands.Payment
{
    public class ConfirmPaymentCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public ConfirmPaymentCommand(Guid id)
        {
            Id = id;
        }
    }
}
