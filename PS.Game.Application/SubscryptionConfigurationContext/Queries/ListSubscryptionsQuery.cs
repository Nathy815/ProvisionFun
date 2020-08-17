using System;
using System.Collections.Generic;
using System.Text;
using Domain.ViewModels;
using MediatR;
using PS.Game.Domain.ViewModels;

namespace Application.SubscryptionConfigurationContext.Queries
{
    public class ListSubscryptionsQuery : IRequest<ListSubscryptionsQueryVM>
    {
    }
}
