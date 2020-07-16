using API.Controllers;
using Application.TournamentContext.Commands.Validate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPatch("validate")]
        [Authorize]
        public async Task<bool> ValidateMatch([FromBody] ValidateMatchCommand request)
        {
            return await _mediator.Send(request);
        }
    }
}
