using Domain.Enums;
using Domain.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.SubscryptionConfigurationContext.Commands.Create
{
    public class CreateSubscryptionCommand : IRequest<bool>
    {
        public Guid TournamentId { get; set; }
        public eMode Mode { get; set; }
        public int Color { get; set; }
        public int Icon { get; set; }
        public string Nickname { get; set; }
        public Guid? CondominiumID { get; set; }
        public TemplateCondominiumVM Condominium { get; set; }
        public TemplatePlayerVM Player { get; set; }
        public List<TemplatePlayerVM> Team { get; set; }
        public string virtualPath { get; set; }
    }
}
