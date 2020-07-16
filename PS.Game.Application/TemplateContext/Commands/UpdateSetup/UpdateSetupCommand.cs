using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.TemplateContext.Commands.UpdateSetup
{
    public class UpdateSetupCommand : IRequest<bool>
    {
        public string HomeTitle { get; set; }
        public string Regulation { get; set; }
        public IFormFile ResponsabilityTerm { get; set; }
        public string virtualPath { get; set; }
    }
}