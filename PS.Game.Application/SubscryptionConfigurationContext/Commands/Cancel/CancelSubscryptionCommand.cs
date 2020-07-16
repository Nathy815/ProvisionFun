using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace PS.Game.Application.SubscryptionConfigurationContext.Commands.Cancel
{
    public class CancelSubscryptionCommand : IRequest<bool>
    {
        public Guid TeamID { get; set; }
        public string Comments { get; set; }
    }
}
