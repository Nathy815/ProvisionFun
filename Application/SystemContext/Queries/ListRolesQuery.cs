using Domain.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.SystemContext.Queries
{
    public class ListRolesQuery : IRequest<List<GetRolesQueryVM>>
    {
    }
}
