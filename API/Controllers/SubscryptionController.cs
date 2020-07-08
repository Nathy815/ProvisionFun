using Application.SubscryptionConfigurationContext.Commands;
using Application.SubscryptionConfigurationContext.Commands.Create;
using Application.SubscryptionConfigurationContext.Commands.Validate;
using Application.SubscryptionConfigurationContext.Queries;
using Domain.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("api/team")]
    public class SubscryptionController : BaseController
    {
        public SubscryptionController(IMediator mediator) : base(mediator) { }

        [HttpPost("create")]
        public async Task<bool> Create([FromForm] CreateSubscryptionCommand request)
        {
            return await _mediator.Send(request);
        }

        [HttpGet("subscryption")]
        [Authorize]
        public async Task<List<GetSubscryptionVM>> Subscryptions()
        {
            return await _mediator.Send(new ListSubscryptionsQuery());
        }

        [HttpGet("subscryption/{id}")]
        [Authorize]
        public async Task<GetSubscryptionDetailVM> SubscryptionDetail([FromRoute] Guid id)
        {
            return await _mediator.Send(new GetSubscryptionQuery(id));
        }

        [HttpPatch("validate")]
        [Authorize]
        public async Task<bool> Validate([FromBody] ValidateSubscryptionCommand request)
        {
            return await _mediator.Send(request);
        }
    }
}
