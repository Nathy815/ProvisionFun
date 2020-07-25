using MediatR;
using PS.Game.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PS.Game.Application.MatchContext.Queries
{
    public class GetMatchQuery : IRequest<MatchVM>
    {
        public Guid Id { get; set; }
        public GetMatchQuery(Guid id)
        {
            Id = id;
        }
    }
}
