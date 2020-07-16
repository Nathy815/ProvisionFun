using Domain.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.TournamentContext.Queries
{
    public class ListAuditorsQuery : IRequest<List<GetAuditorsQueryVM>>
    {
    }
}
