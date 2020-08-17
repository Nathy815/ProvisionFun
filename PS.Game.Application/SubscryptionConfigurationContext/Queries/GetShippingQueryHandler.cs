using Application.Services.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using PS.Game.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PS.Game.Application.SubscryptionConfigurationContext.Queries
{
    public class GetShippingQueryHandler : IRequestHandler<GetShippingQuery, ShippingVM>
    {
        private readonly IBoleto _boleto;
        private readonly MySqlContext _sqlContext;

        public GetShippingQueryHandler(IBoleto boleto, MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
            _boleto = boleto;
        }

        public async Task<ShippingVM> Handle(GetShippingQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var _teams = await _sqlContext.Set<Team>()
                                        .Include(t => t.Payments)
                                        .Include(t => t.Condominium)
                                        .Include(t => t.Players)
                                            .ThenInclude(p => p.Player)
                                        .Where(t => t.Active && t.Status == Domain.Enums.eStatus.Payment)
                                        .ToListAsync();

                var _file = await _boleto.GenerateShipping(_teams);

                return new ShippingVM(_file);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
