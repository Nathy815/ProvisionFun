using MediatR;
using PS.Game.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PS.Game.Application.SubscryptionConfigurationContext.Queries
{
    public class GenerateMatchesQueryHandler : IRequestHandler<GenerateMatchesQuery, bool>
    {
        private readonly IHangfire _hangfire;

        public GenerateMatchesQueryHandler(IHangfire hangfire)
        {
            _hangfire = hangfire;
        }

        public async Task<bool> Handle(GenerateMatchesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var _result = await _hangfire.GerarPartidas();

                return _result;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
