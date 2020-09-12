using Application.SubscryptionConfigurationContext.Commands;
using Application.SubscryptionConfigurationContext.Commands.Create;
using Application.SubscryptionConfigurationContext.Commands.Payment;
using Application.SubscryptionConfigurationContext.Commands.Validate;
using Application.SubscryptionConfigurationContext.Queries;
using Domain.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PS.Game.Application.SubscryptionConfigurationContext.Commands.BankSlip;
using PS.Game.Application.SubscryptionConfigurationContext.Commands.Cancel;
using PS.Game.Application.SubscryptionConfigurationContext.Queries;
using PS.Game.Domain.ViewModels;
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

        [HttpPost("bankslip/{id}")]
        [Authorize]
        public async Task<bool> BankSlip([FromRoute] Guid Id)
        {
            return await _mediator.Send(new BankSlipCommand(Id));
        }

        [HttpPost("cancel")]
        [Authorize]
        public async Task<bool> Cancel([FromBody] CancelSubscryptionCommand request)
        {
            return await _mediator.Send(request);
        }

        [HttpPatch("confirm/{id}")]
        [Authorize]
        public async Task<int?> Confirm([FromForm] ConfirmPaymentCommand request)
        {
            return await _mediator.Send(request);
        }

        [HttpPost("create"), DisableRequestSizeLimit]
        public async Task<bool> Create([FromForm] CreateSubscryptionCommand request)
        {
            return await _mediator.Send(request);
        }

        [HttpPatch("payment"), DisableRequestSizeLimit]
        public async Task<int?> Payment([FromForm] ConfirmPaymentCommand request)
        {
            return await _mediator.Send(request);
        }

        [HttpGet("shipping")]
        [Authorize]
        public async Task<ShippingVM> Shipping()
        {
            return await _mediator.Send(new GetShippingQuery());
        }

        [HttpGet]
        [Authorize]
        public async Task<ListSubscryptionsQueryVM> Subscryptions()
        {
            return await _mediator.Send(new ListSubscryptionsQuery());
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<GetSubscryptionDetailVM> SubscryptionDetail([FromRoute] Guid id)
        {
            return await _mediator.Send(new GetSubscryptionQuery(id));
        }

        [HttpGet("teste")]
        public async Task<bool> Teste()
        {
            return await _mediator.Send(new TesteQuery());
        }
        
        [HttpPatch("validate")]
        [Authorize]
        public async Task<bool> Validate([FromBody] ValidateSubscryptionCommand request)
        {
            return await _mediator.Send(request);
        }
    }
}