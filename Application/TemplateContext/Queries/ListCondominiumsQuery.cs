using Domain.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.TemplateContext.Queries
{
    public class ListCondominuimsQuery : IRequest<List<GetCondominiumQueryVM>>
    {
    }
}
