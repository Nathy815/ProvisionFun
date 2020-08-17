using MediatR;
using PS.Game.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PS.Game.Application.SubscryptionConfigurationContext.Queries
{
    public class GetShippingQuery : IRequest<ShippingVM>
    {
        //public List<Guid> teams { get; set; }
    }
}