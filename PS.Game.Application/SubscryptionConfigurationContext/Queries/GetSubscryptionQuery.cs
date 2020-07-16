using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Domain.ViewModels;

namespace Application.SubscryptionConfigurationContext.Queries
{
    public class GetSubscryptionQuery : IRequest<GetSubscryptionDetailVM>
    {
        public Guid Id { get; set; }

        public GetSubscryptionQuery(Guid id)
        {
            Id = id;
        }
    }
}
