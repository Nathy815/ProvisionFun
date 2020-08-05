using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.TemplateContext.Commands.UpdateSetup
{
    public class UpdateSetupCommand : IRequest<bool>
    {
        public IFormFile HomeBanner { get; set; }
        public IFormFile HomeBanner2 { get; set; }
        public IFormFile HomeBanner3 { get; set; }
        public IFormFile RegistryBanner { get; set; }
        public string HomeTitle { get; set; }
        public string Regulation { get; set; }
        public IFormFile ResponsabilityTerm { get; set; }
    }
}