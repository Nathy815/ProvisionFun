using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.TournamentContext.Commands.UpdateTournament
{
    public class UpdateTournamentCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartSubscryption { get; set; }
        public DateTime EndSubscryption { get; set; }
        public int SubscryptionLimit { get; set; }
        public int PlayerLimit { get; set; }
        public eMode Mode { get; set; }
        public Guid? GameID { get; set; }
        public string Game { get; set; }
    }
}
