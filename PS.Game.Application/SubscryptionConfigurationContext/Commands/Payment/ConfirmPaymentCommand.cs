using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.SubscryptionConfigurationContext.Commands.Payment
{
    public class ConfirmPaymentCommand : IRequest<int?>
    {
        public IFormFile file { get; set; }
    }
}
