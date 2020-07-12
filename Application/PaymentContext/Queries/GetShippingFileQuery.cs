using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.PaymentContext.Queries
{
    public class GetShippingFileQuery : IRequest<string>
    {
        public Guid? teamID { get; set; }
        public string virtualPath { get; set; }

        public GetShippingFileQuery(Guid? teamID, string virtualPath)
        {
            this.teamID = teamID;
            this.virtualPath = virtualPath;
        }
    }
}
