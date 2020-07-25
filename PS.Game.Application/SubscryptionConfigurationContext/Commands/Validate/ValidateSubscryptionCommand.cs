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
        public string ZipCode { get; set; }
        public string Address { get; set; }
        public string Number { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
