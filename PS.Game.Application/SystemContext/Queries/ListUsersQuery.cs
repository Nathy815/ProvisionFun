using Domain.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.SystemContext.Queries
{
    public class ListUsersQuery : IRequest<List<GetUserQueryVM>>
    {
    }
}
