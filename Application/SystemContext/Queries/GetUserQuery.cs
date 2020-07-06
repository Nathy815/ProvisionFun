using Domain.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.SystemContext.Queries
{
    public class GetUserQuery : IRequest<GetUserQueryVM>
    {
        public Guid userID { get; set; }

        public GetUserQuery(Guid Id)
        {
            userID = Id;
        }
    }
}
