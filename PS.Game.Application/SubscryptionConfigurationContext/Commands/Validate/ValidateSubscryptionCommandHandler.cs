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

namespace Application.SubscryptionConfigurationContext.Commands.Validate
{
    public class ValidateSubscryptionCommandHandler : IRequestHandler<ValidateSubscryptionCommand, bool>
    {
        private readonly MySqlContext _sqlContext;
        private readonly IEmail _email;
        private readonly IBoleto _boleto;

        public ValidateSubscryptionCommandHandler(MySqlContext sqlContext, IEmail email, IBoleto boleto)
        {
            _sqlContext = sqlContext;
            _email = email;
            _boleto = boleto;
        }

        public async Task<bool> Handle(ValidateSubscryptionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var _team = await _sqlContext.Set<Team>()
                                        .Include(t => t.Players)
                                            .ThenInclude(tp => tp.Player)
                                        .Include(t => t.Condominium)
                                        .Where(t => t.Id == request.Id)
                                        .FirstOrDefaultAsync();

                var _player = _team.Players.Where(tp => tp.IsPrincipal).Select(tp => tp.Player).FirstOrDefault();

                bool _result = false;
                var _boleto_bancario = await _boleto.GeneratePayment(_team);
                _result = await _email.SendEmail(_player.Email, Domain.Enums.eStatus.Payment, _boleto_bancario);

                _team.PaymentSent = _result;
                
                if (_result)
                {
                    _team.ValidatedDate = DateTime.Now;
                    _team.Status = Domain.Enums.eStatus.Payment;
                }

                if (!_team.Condominium.Validated)
                {
                    _team.Condominium.Name = request.Condominium;
                    _team.Condominium.Validated = true;
                }

                _sqlContext.Teams.Update(_team);

                await _sqlContext.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
