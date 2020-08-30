using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace PS.Game.Application.TournamentContext.Commands.DeleteTournament
{
    public class DeleteTournamentCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteTournamentCommand(Guid id)
        {
            Id = id;
        }
    }
}
