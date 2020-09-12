using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PS.Game.Application.SubscryptionConfigurationContext.Queries
{
    public class TesteQueryHandler : IRequestHandler<TesteQuery, bool>
    {
        public async Task<bool> Handle(TesteQuery request, CancellationToken cancellationToken)
        {
            try
            {
                throw new Exception();
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
