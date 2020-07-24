using Application.Services;
using MediatR;
using Persistence.Contexts;
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
        private readonly MySqlContext _sqlContext;
        private readonly IUtil _util;

        public GenerateMatchesQueryHandler(MySqlContext sqlContext, IUtil util)
        {
            _sqlContext = sqlContext;
            _util = util;
        }

        public async Task<bool> Handle(GenerateMatchesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                /*var _job = new BackgroundService();

                var _result = await _job.GerarPartidas(new CancellationToken());*/

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
