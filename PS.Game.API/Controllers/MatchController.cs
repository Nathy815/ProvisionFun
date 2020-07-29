using API.Controllers;
using Application.MatchContext.Commands.Update;
using Application.MatchContext.Queries;
using Domain.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PS.Game.Application.MatchContext.Queries;
using PS.Game.Application.SubscryptionConfigurationContext.Queries;
using PS.Game.Domain.ViewModels;
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

        [HttpGet("{id}")]
        [Authorize]
        public async Task<MatchVM> Get([FromRoute] Guid id)
        {
            return await _mediator.Send(new GetMatchQuery(id));
        }

        [HttpGet]
        [Authorize]
        public async Task<MatchesVM> List()
        {
            return await _mediator.Send(new ListMatchesQuery());
        }

        [HttpPost("search")]
        public async Task<List<GetMatchQueryVM>> Search([FromBody] SearchMatchQuery request)
        {
            return await _mediator.Send(request);
        }

        [HttpGet("top")]
        public async Task<List<GetMatchQueryVM>> TopTournaments()
        {
            return await _mediator.Send(new GetTopMatchesQuery());
        }

        [HttpPatch("update")]
        [Authorize]
        public async Task<bool> Update([FromBody] UpdateMatchCommand request)
        {
            return await _mediator.Send(request);
        }
    }
}
