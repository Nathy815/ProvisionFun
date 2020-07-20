using Application.Services.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PS.Game.Application.SubscryptionConfigurationContext.Queries
{
    public class GetShippingQueryHandler : IRequestHandler<GetShippingQuery, string>
    {
        private readonly IBoleto _boleto;
        private readonly MySqlContext _sqlContext;

        public GetShippingQueryHandler(IBoleto boleto, MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
            _boleto = boleto;
        }

        public async Task<string> Handle(GetShippingQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.teams == null || request.teams.Count == 0)
                    throw new Exception("Por favor informe ao menos uma inscrição.");

                var _teams = await _sqlContext.Set<Team>()
                                        .Include(t => t.Payments)
                                        .Include(t => t.Condominium)
                                        .Include(t => t.Players)
                                            .ThenInclude(p => p.Player)
                                        .Where(t => request.teams.Any(id => id == t.Id &&
                                                    t.Status == Domain.Enums.eStatus.Payment))
                                        .ToListAsync();
                
                var _file = await _boleto.GenerateShipping(_teams, request.virtualPath);

                return _file;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
