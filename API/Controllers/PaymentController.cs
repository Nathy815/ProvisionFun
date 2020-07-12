using Application.PaymentContext.Commands.Confirm;
using Application.PaymentContext.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("api/payment")]
    public class PaymentController : BaseController
    {
        public PaymentController(IMediator mediator) : base(mediator) { }

        [HttpPatch("confirm")]
        public async Task<bool> Confirm([FromForm] ConfirmPaymentCommand request)
        {
            request.virtualPath = GetVirtualPath();
            return await _mediator.Send(request);
        }

        [HttpGet("{id}")]
        public async Task<string> Get([FromRoute] Guid? id)
        {
            return await _mediator.Send(new GetShippingFileQuery(id, GetVirtualPath()));
        }
    }
}
