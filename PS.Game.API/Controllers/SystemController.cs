using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.SystemContext.Commands.CreateUser;
using Application.SystemContext.Commands.DeleteUser;
using Application.SystemContext.Commands.Login;
using Application.SystemContext.Commands.UpdateUser;
using Application.SystemContext.Queries;
using Domain.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PS.Game.Domain.ViewModels;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : BaseController
    {
        public SystemController(IMediator mediator) : base(mediator) { }

        [HttpPost("create")]
        [Authorize(Roles = "Administrador")]
        public async Task<bool> CreateUser([FromBody] CreateUserCommand request)
        {
            return await _mediator.Send(request);
        }

        [HttpDelete("delete/{userID}")]
        [Authorize(Roles = "Administrador")]
        public async Task<bool> DeleteUser([FromRoute] Guid userID)
        {
            return await _mediator.Send(new DeleteUserCommand(userID));
        }

        [HttpPost("login")]
        public async Task<LoginVM> Login([FromBody] LoginCommand request)
        {
            return await _mediator.Send(request);
        }

        [HttpPatch("update")]
        [Authorize(Roles = "Administrador")]
        public async Task<bool> UpdateUser([FromBody] UpdateUserCommand request)
        {
            return await _mediator.Send(request);
        }
        
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<List<GetUserQueryVM>> GetUsers()
        {
            return await _mediator.Send(new ListUsersQuery());
        }

        [HttpGet("{userID}")]
        [Authorize(Roles = "Administrador")]
        public async Task<GetUserQueryVM> GetUser([FromRoute] Guid userID)
        {
            return await _mediator.Send(new GetUserQuery(userID));
        }

        [HttpGet("roles")]
        [Authorize(Roles = "Administrador")]
        public async Task<List<GetRolesQueryVM>> GetRoles()
        {
            return await _mediator.Send(new ListRolesQuery());
        }
    }
}