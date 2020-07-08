using System;
using System.Collections.Generic;
using System.Text;
using Domain.ViewModels;
using MediatR;

namespace Application.SubscryptionConfigurationContext.Queries
{
    public class ListSubscryptionsQuery : IRequest<List<GetSubscryptionVM>>
    {
    }
}
