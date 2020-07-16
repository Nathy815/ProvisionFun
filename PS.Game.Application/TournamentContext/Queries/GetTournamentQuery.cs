using Domain.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.TournamentContext.Queries
{
    public class GetTournamentQuery : IRequest<GetTournamentQueryVM>
    {
        public Guid tournamentID { get; set; }
        public GetTournamentQuery(Guid Id)
        {
            tournamentID = Id;
        }
    }
}
