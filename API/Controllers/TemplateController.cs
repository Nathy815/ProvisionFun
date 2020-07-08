using Application.TemplateContext.Commands.UpdateSetup;
using Application.TemplateContext.Queries;
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
    [Route("api/template")]
    public class TemplateController : BaseController
    {
        public TemplateController(IMediator mediator) : base(mediator) { }

        [HttpGet("tournaments")]
        public async Task<List<TemplateGameVM>> GetAllTournaments()
        {
            return await _mediator.Send(new ListTemplateTournamentsQuery());
        }

        [HttpGet("address")]
        public async Task<GetCondominiumQueryVM> Address([FromBody] GetAddressQuery request)
        {
            return await _mediator.Send(request);
        }

        [HttpGet("condominiums")]
        public async Task<List<GetCondominiumQueryVM>> Condominiums()
        {
            return await _mediator.Send(new ListCondominiumsQuery());
        }

        [HttpGet]
        public async Task<GetHomeQueryVM> Home()
        {
            return await _mediator.Send(new GetHomeQuery());
        }

        [HttpGet("setups")]
        public async Task<GetSetupQueryVM> Setups()
        {
            return await _mediator.Send(new ListSetupsQuery());
        }

        [HttpPost, DisableRequestSizeLimit]
        [Authorize(Roles = "Administrador")]
        public async Task<bool> UpdateSetup([FromForm] UpdateSetupCommand request)
        {
            request.virtualPath = GetVirtualPath();
            return await _mediator.Send(request);
        }
    }
}
