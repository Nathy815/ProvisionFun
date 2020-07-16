using API.Controllers;
using Application.TournamentContext.Queries;
using Domain.ViewModels;
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
    public class GameController : BaseController
    {
        public GameController(IMediator mediator) : base(mediator) { }

        [HttpGet("{gameID}")]
        [Authorize(Roles = "Administrador")]
        public async Task<GetGameQueryVM> GetGame([FromRoute] Guid gameID)
        {
            return await _mediator.Send(new GetGameQuery(gameID));
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<List<GetGameQueryVM>> ListGames()
        {
            return await _mediator.Send(new ListGamesQuery());
        }
    }
}
