using Domain.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.MatchContext.Queries
{
    public class GetTopMatchesQuery : IRequest<List<GetMatchQueryVM>>
    {
    }
}
