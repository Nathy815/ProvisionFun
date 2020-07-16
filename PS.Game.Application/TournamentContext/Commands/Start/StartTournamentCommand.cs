using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.TournamentContext.Commands.Start
{
    public class StartTournamentCommand : IRequest<bool>
    {
        public Guid TournamentID { get; set; }

        public StartTournamentCommand(Guid id)
        {
            TournamentID = id;
        }
    }
}
