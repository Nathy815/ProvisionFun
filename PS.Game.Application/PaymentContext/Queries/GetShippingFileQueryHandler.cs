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

namespace Application.PaymentContext.Queries
{
    public class GetShippingFileQueryHandler : IRequestHandler<GetShippingFileQuery, string>
    {
        private readonly IBoleto _boleto;
        private readonly MySqlContext _sqlContext;

        public GetShippingFileQueryHandler(IBoleto boleto, MySqlContext sqlContext)
        {
            _boleto = boleto;
            _sqlContext = sqlContext;
        }

        public async Task<string> Handle(GetShippingFileQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var _list = new List<Team>();

                if (request.teamID.HasValue)
                {
                    var _team = await _sqlContext.Set<Team>()
                                        .Include(t => t.Players)
                                            .ThenInclude(p => p.Player)
                                        .Include(t => t.Condominium)
                                        .Include(t => t.Payments)
                                        .Where(t => t.Id == request.teamID.Value)
                                        .FirstOrDefaultAsync();

                    _list.Add(_team);
                }
                else
                {
                    _list = await _sqlContext.Set<Team>()
                                        .Include(t => t.Players)
                                            .ThenInclude(p => p.Player)
                                        .Include(t => t.Condominium)
                                        .Include(t => t.Payments)
                                        .Where(t => t.Active &&
                                                    t.Status == Domain.Enums.eStatus.Payment)
                                        .ToListAsync();
                }

                var _return = await _boleto.GenerateShipping(_list, request.virtualPath);

                return _return;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
