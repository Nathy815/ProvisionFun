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

namespace Application.SubscryptionConfigurationContext.Commands.Payment
{
    public class ConfirmPaymentCommandHandler : IRequestHandler<ConfirmPaymentCommand, bool>
    {
        private readonly MySqlContext _sqlContext;
        private readonly IEmail _email;

        public ConfirmPaymentCommandHandler(MySqlContext sqlContext, IEmail email)
        {
            _sqlContext = sqlContext;
            _email = email;
        }

        public async Task<bool> Handle(ConfirmPaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var _team = await _sqlContext.Set<Team>()
                                        .Include(t => t.Players)
                                            .ThenInclude(p => p.Player)
                                        .Where(t => t.Id == request.Id)
                                        .FirstOrDefaultAsync();

                var _player = _team.Players.Where(p => p.IsPrincipal).Select(p => p.Player).FirstOrDefault();

                _team.PaymentDate = DateTime.Now;
                _team.PaymentSent = await _email.SendEmail(_player.Email, Domain.Enums.eStatus.Finished);

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
