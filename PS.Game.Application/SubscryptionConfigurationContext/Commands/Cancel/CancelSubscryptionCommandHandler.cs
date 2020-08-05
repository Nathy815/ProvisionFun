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

namespace PS.Game.Application.SubscryptionConfigurationContext.Commands.Cancel
{
    public class CancelSubscryptionCommandHandler : IRequestHandler<CancelSubscryptionCommand, bool>
    {
        private readonly MySqlContext _sqlContext;
        private readonly IEmail _email;

        public CancelSubscryptionCommandHandler(MySqlContext sqlContext, IEmail email)
        {
            _sqlContext = sqlContext;
            _email = email;
        }

        public async Task<bool> Handle(CancelSubscryptionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var _team = await _sqlContext.Set<Team>()
                                        .Include(t => t.Players)
                                            .ThenInclude(p => p.Player)
                                        .Where(t => t.Id == request.TeamID)
                                        .FirstOrDefaultAsync();

                _team.Active = false;
                _team.Status = Domain.Enums.eStatus.Cancelled;
                _team.CancellationComments = request.Comments;

                var _player = _team.Players.Where(p => p.IsPrincipal).FirstOrDefault().Player;

                _team.CancellationSent = await _email.SendEmail(_team, Domain.Enums.eStatus.Cancelled);

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
