using Application.SubscryptionConfigurationContext.Commands;
using Application.SubscryptionConfigurationContext.Commands.Create;
using Application.SubscryptionConfigurationContext.Commands.Payment;
using Application.SubscryptionConfigurationContext.Commands.Validate;
using Application.SubscryptionConfigurationContext.Queries;
using Domain.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PS.Game.Application.SubscryptionConfigurationContext.Commands.Cancel;
using PS.Game.Application.SubscryptionConfigurationContext.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscryptionController : BaseController
    {
        public SubscryptionController(IMediator mediator) : base(mediator) { }

        [HttpDelete("cancel")]
        [Authorize(Roles = "Administrador")]
        public async Task<bool> Cancel([FromBody] CancelSubscryptionCommand request)
        {
            return await _mediator.Send(request);
        }

        [HttpPatch("confirm/{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<bool> Confirm([FromForm] ConfirmPaymentCommand request)
        {
            return await _mediator.Send(request);
        }

        [HttpPost("create"), DisableRequestSizeLimit]
        public async Task<bool> Create([FromForm] CreateSubscryptionCommand request)
        {
            return await _mediator.Send(request);
        }

        [HttpPatch("payment"), DisableRequestSizeLimit]
        public async Task<bool> Payment([FromForm] ConfirmPaymentCommand request)
        {
            return await _mediator.Send(request);
        }

        [HttpPost("shipping")]
        [Authorize(Roles = "Administrador")]
        public async Task<string> Shipping([FromBody] GetShippingQuery request)
        {
            return await _mediator.Send(request);
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<List<GetSubscryptionVM>> Subscryptions()
        {
            return await _mediator.Send(new ListSubscryptionsQuery());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<GetSubscryptionDetailVM> SubscryptionDetail([FromRoute] Guid id)
        {
            return await _mediator.Send(new GetSubscryptionQuery(id));
        }
        
        [HttpPatch("validate")]
        [Authorize(Roles = "Administrador")]
        public async Task<bool> Validate([FromBody] ValidateSubscryptionCommand request)
        {
            return await _mediator.Send(request);
        }
    }
}