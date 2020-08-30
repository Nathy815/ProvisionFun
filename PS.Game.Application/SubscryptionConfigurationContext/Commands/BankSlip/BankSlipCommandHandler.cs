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

namespace PS.Game.Application.SubscryptionConfigurationContext.Commands.BankSlip
{
    public class BankSlipCommandHandler : IRequestHandler<BankSlipCommand, bool>
    {
        private readonly MySqlContext _sqlContext;
        private readonly IEmail _email;
        private readonly IBoleto _boleto;

        public BankSlipCommandHandler(MySqlContext sqlContext, IEmail email, IBoleto boleto)
        {
            _sqlContext = sqlContext;
            _email = email;
            _boleto = boleto;
        }

        public async Task<bool> Handle(BankSlipCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var _team = await _sqlContext.Set<Team>()
                                        .Include(t => t.Payments)
                                        .Include(t => t.Players)
                                            .ThenInclude(tp => tp.Player)
                                        .Include(t => t.Condominium)
                                        .Include(t => t.Tournament)
                                        .Where(t => t.Id == request.Id)
                                        .FirstOrDefaultAsync();

                foreach (var _payment in _team.Payments)
                    _payment.Active = false;

                var _price = 20D;
                var _startDate = _team.Tournament.StartSubscryption.Date;
                if ((_team.Mode == Domain.Enums.eMode.Solo && _team.CreatedDate.Date > _startDate.AddDays(21)) ||
                    (_team.Mode == Domain.Enums.eMode.Team && _team.CreatedDate.Date > _startDate.AddDays(14)))
                    _price = 30D;
                _team.Price = _price;

                bool _result = false;
                var _boleto_bancario = await _boleto.GeneratePayment(_team);

                _result = await _email.SendEmail(_team, Domain.Enums.eStatus.Payment, _boleto_bancario);

                _team.PaymentSent = _result;

                _sqlContext.Teams.Update(_team);

                await _sqlContext.SaveChangesAsync();

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
