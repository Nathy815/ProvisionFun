using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.TournamentContext.Commands.UpdateMatch
{
    public class UpdateMatchCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public Guid AuditorID { get; set; }
    }
}
