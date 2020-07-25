using Application.Services.Interfaces;
using Domain.Entities;
using iText.Html2pdf;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.IO;
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

                _result = await _email.SendEmail(_player.Email, PS.Game.Domain.Enums.eStatus.Payment, _boleto_bancario);

                _team.PaymentSent = _result;
                
                if (_result)
                {
                    _team.ValidatedDate = DateTime.Now;
                    _team.Status = PS.Game.Domain.Enums.eStatus.Payment;
                }

                var _ids = new List<Guid>();

                if (!_team.Condominium.Validated)
                {
                    _team.Condominium.Validated = true;

                    if (string.IsNullOrEmpty(_team.Condominium.Name))
                        _team.Condominium.Name = request.Condominium;
                    else
                    {
                        _team.Condominium.ZipCode = request.ZipCode;
                        _team.Condominium.Address = request.Address;
                        _team.Condominium.Number = request.Number;
                        _team.Condominium.District = request.District;
                        _team.Condominium.City = request.City;
                        _team.Condominium.State = request.State;
                    }

                    var _dbTeams = await _sqlContext.Set<Team>()
                                            .Include(t => t.Condominium)
                                            .Where(t => t.Condominium.ZipCode.Equals(_team.Condominium.ZipCode) &&
                                                        t.Condominium.Number.Equals(_team.Condominium.Number) &&
                                                        t.CondominiumID != _team.CondominiumID)
                                            .ToListAsync();
                    
                    foreach (var _db in _dbTeams)
                    {
                        _ids.Add(_db.CondominiumID);
                        _db.CondominiumID = _team.CondominiumID;
                    }

                    _sqlContext.Teams.UpdateRange(_dbTeams);
                }

                _sqlContext.Teams.Update(_team);

                await _sqlContext.SaveChangesAsync(cancellationToken);

                // Limpar condomínios iguais não validados (CEP e Número iguais)
                if (_ids.Count > 0)
                {
                    var _condominiums = await _sqlContext.Set<Condominium>()
                                                    .Where(c => _ids.Any(id => id == c.Id))
                                                    .ToListAsync();

                    _sqlContext.Condominiums.RemoveRange(_condominiums);

                    await _sqlContext.SaveChangesAsync(cancellationToken);
                }

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
