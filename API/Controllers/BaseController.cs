using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class BaseController : Controller
    {
        protected readonly IMediator _mediator;

        public BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected Guid LoggedUserID => new Guid(User.Claims.FirstOrDefault().Value);

        protected string GetVirtualPath()
        {
            var prefix = "http://";
            if (Request.IsHttps)
                prefix = "https://";
            var virtualPath = prefix + Request.Host.ToString() + "/fileserver_api/resources";

            return virtualPath;
        }
    }
}
