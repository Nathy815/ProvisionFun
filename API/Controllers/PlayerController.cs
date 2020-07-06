using Application.SubscryptionConfigurationContext.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("api/player")]
    public class PlayerController : BaseController
    {
        public PlayerController(IMediator mediator) : base(mediator) { }

        [HttpPost("subscryption"), DisableRequestSizeLimit]
        public async Task<bool> Subscryption([FromForm] CreateSubscryptionCommand request)
        {
            return await _mediator.Send(request);
        }
    }
}
