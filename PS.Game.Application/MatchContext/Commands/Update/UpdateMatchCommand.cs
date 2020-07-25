using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.MatchContext.Commands.Update
{
    public class UpdateMatchCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public DateTime? Date { get; set; }
        public Guid? AuditorID { get; set; }
        public Guid? Winner { get; set; }
        public double? ScorePlayer1 { get; set; }
        public double? ScorePlayer2 { get; set; }
        public string Comments { get; set; }
    }
}
