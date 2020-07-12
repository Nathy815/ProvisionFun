using Application.TournamentContext.Commands.CreateTournament;
using Application.TournamentContext.Commands.UpdateTournament;
using Application.TournamentContext.Commands.Validate;
using Application.TournamentContext.Queries;
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
    [Route("api")]
    public class TournamentController : BaseController
    {
        public TournamentController(IMediator mediator) : base(mediator) { }

        [HttpGet("/game/{gameID}")]
        [Authorize(Roles = "Administrador")]
        public async Task<GetGameQueryVM> GetGame([FromRoute] Guid gameID)
        {
            return await _mediator.Send(new GetGameQuery(gameID));
        }

        [HttpGet("/game")]
        [Authorize(Roles = "Administrador")]
        public async Task<List<GetGameQueryVM>> ListGames()
        {
            return await _mediator.Send(new ListGamesQuery());
        }

        [HttpPost("/tournament/create"), DisableRequestSizeLimit]
        [Authorize(Roles = "Administrador")]
        public async Task<bool> CreateTournament([FromForm] CreateTournamentCommand request)
        {
            return await _mediator.Send(request);
        }

        [HttpGet("/tournament/{tournamentID}")]
        [Authorize(Roles = "Administrador")]
        public async Task<GetTournamentQueryVM> GetTournament([FromRoute] Guid tournamentID)
        {
            return await _mediator.Send(new GetTournamentQuery(tournamentID));
        }

        [HttpGet("/tournament/top")]
        public async Task<List<GetMatchQueryVM>> TopTournaments()
        {
            return await _mediator.Send(new GetTopMatchesQuery());
        }

        [HttpGet("/tournament")]
        [Authorize(Roles = "Administrador")]
        public async Task<List<GetTournamentQueryVM>> ListTournaments()
        {
            return await _mediator.Send(new ListTournamentsQuery());
        }

        [HttpGet("/tournament/auditors")]
        [Authorize]
        public async Task<List<GetAuditorsQueryVM>> Auditors()
        {
            return await _mediator.Send(new ListAuditorsQuery());
        }

        [HttpGet("/tournament/search")]
        public async Task<List<GetMatchQueryVM>> Search([FromBody] SearchMatchQuery request)
        {
            return await _mediator.Send(request);
        }

        [HttpPatch("/tournament/update")]
        [Authorize(Roles = "Administrador")]
        public async Task<bool> UpdateTournament(UpdateTournamentCommand request)
        {
            return await _mediator.Send(request);
        }

        [HttpPatch("/match/validate")]
        [Authorize]
        public async Task<bool> ValidateMatch([FromBody] ValidateMatchCommand request)
        {
            return await _mediator.Send(request);
        }
    }
}
