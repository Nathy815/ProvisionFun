using Application.Services.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.PaymentContext.Commands.Confirm
{
    public class ConfirmPaymentCommandHandler : IRequestHandler<ConfirmPaymentCommand, bool>
    {
        private readonly IBoleto _boleto;
        
        public ConfirmPaymentCommandHandler(IBoleto boleto)
        {
            _boleto = boleto;
        }

        public async Task<bool> Handle(ConfirmPaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var _result = await _boleto.ImportReturn(request.file, request.virtualPath);

                return _result;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
