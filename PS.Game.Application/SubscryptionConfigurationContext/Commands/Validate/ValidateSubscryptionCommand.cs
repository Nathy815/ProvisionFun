using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.SubscryptionConfigurationContext.Commands.Validate
{
    public class ValidateSubscryptionCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Condominium { get; set; }
    }
}
