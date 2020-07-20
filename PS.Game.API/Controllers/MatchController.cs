using API.Controllers;
using Application.TournamentContext.Commands.Validate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PS.Game.Application.SubscryptionConfigurationContext.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PS.Game.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : BaseController
    {
        public MatchController(IMediator mediator) : base(mediator) { }

        [HttpGet("generate")]
        public async Task<bool> Generate()
        {
            return await _mediator.Send(new GenerateMatchesQuery());
        }

        [HttpPatch("validate")]
        [Authorize]
        public async Task<bool> ValidateMatch([FromBody] ValidateMatchCommand request)
        {
            return await _mediator.Send(request);
        }
    }
}
