using Application.Services.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SubscryptionConfigurationContext.Commands.Payment
{
    public class ConfirmPaymentCommandHandler : IRequestHandler<ConfirmPaymentCommand, int?>
    {
        private readonly IBoleto _boleto;
        
        public ConfirmPaymentCommandHandler(IBoleto boleto)
        {
            _boleto = boleto;
        }

        public async Task<int?> Handle(ConfirmPaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var _result = await _boleto.ImportReturn(request.file);

                return _result;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}