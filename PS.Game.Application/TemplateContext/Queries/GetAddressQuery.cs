using Domain.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.TemplateContext.Queries
{
    public class GetAddressQuery : IRequest<GetCondominiumQueryVM>
    {
        public string ZipCode { get; set; }
        public string Number { get; set; }
    }
}
