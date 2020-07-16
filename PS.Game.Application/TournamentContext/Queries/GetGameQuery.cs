using Domain.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.TournamentContext.Queries
{
    public class GetGameQuery : IRequest<GetGameQueryVM>
    {
        public Guid gameID { get; set; }

        public GetGameQuery(Guid Id)
        {
            gameID = Id;
        }
    }
}
