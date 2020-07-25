using Application.TournamentContext.Commands.CreateTournament;
using Application.TournamentContext.Commands.UpdateTournament;
using Application.MatchContext.Commands.Validate;
using Application.TournamentContext.Queries;
using Domain.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.MatchContext.Queries;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentController : BaseController
    {
        public TournamentController(IMediator mediator) : base(mediator) { }

        [HttpPost("create")]
        [Authorize(Roles = "Administrador")]
        public async Task<bool> CreateTournament([FromBody] CreateTournamentCommand request)
        {
            return await _mediator.Send(request);
        }

        [HttpGet("{tournamentID}")]
        [Authorize(Roles = "Administrador")]
        public async Task<GetTournamentQueryVM> GetTournament([FromRoute] Guid tournamentID)
        {
            return await _mediator.Send(new GetTournamentQuery(tournamentID));
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<List<GetTournamentQueryVM>> ListTournaments()
        {
            return await _mediator.Send(new ListTournamentsQuery());
        }

        [HttpGet("auditors")]
        [Authorize]
        public async Task<List<GetAuditorsQueryVM>> Auditors()
        {
            return await _mediator.Send(new ListAuditorsQuery());
        }

        [HttpPatch("update")]
        [Authorize(Roles = "Administrador")]
        public async Task<bool> UpdateTournament([FromBody] UpdateTournamentCommand request)
        {
            return await _mediator.Send(request);
        }
    }
}
