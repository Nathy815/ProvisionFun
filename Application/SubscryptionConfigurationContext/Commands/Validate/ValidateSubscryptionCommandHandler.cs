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

        public ValidateSubscryptionCommandHandler(MySqlContext sqlContext, IEmail email)
        {
            _sqlContext = sqlContext;
            _email = email;
        }

        public async Task<bool> Handle(ValidateSubscryptionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var _team = await _sqlContext.Set<Team>()
                                        .Include(t => t.Players)
                                            .ThenInclude(tp => tp.Player)
                                        .Where(t => t.Id == request.Id)
                                        .FirstOrDefaultAsync();

                _team.Active = request.Validate;

                var _player = _team.Players.Where(tp => tp.IsPrincipal).Select(tp => tp.Player).FirstOrDefault();

                bool _result = false;
                if (request.Validate)
                    _result = await _email.SendEmail(_player.Email, Domain.Enums.eStatus.Payment);
                else
                    _result = await _email.SendEmail(_player.Email, Domain.Enums.eStatus.Cancelled);

                _team.PaymentSent = _result;
                
                if (_result)
                {
                    _team.ValidatedDate = DateTime.Now;
                    _team.Status = request.Validate ? Domain.Enums.eStatus.Payment : Domain.Enums.eStatus.Cancelled;
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
