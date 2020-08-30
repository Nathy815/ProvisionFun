using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace PS.Game.Application.MatchContext.Queries
{
    public class GenerateMatchesQuery : IRequest<bool>
    {
        public Guid Id { get; set; }

        public GenerateMatchesQuery(Guid id)
        {
            Id = id;
        }
    }
}
