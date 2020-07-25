using MediatR;
using PS.Game.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PS.Game.Application.MatchContext.Queries
{
    public class ListMatchesQuery : IRequest<MatchesVM>
    {

    }
}
