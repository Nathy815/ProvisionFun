using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.TournamentContext.Commands.Validate
{
    public class ValidateMatchCommand : IRequest<bool>
    {
        public Guid MatchID { get; set; }
        public Guid? Winner { get; set; }
        public double? Player1Score { get; set; }
        public double? Player2Score { get; set; }
        public string Comments { get; set; }
    }
}
