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

namespace Application.TournamentContext.Commands.Validate
{
    public class ValidateMatchCommandHandler : IRequestHandler<ValidateMatchCommand, bool>
    {
        private readonly MySqlContext _sqlContext;

        public ValidateMatchCommandHandler(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<bool> Handle(ValidateMatchCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var _match = await _sqlContext.Set<Match>()
                                        .Where(m => m.Id == request.MatchID)
                                        .FirstOrDefaultAsync();

                _match.Comments = request.Comments;

                if (request.Player1Score.HasValue)
                {
                    _match.Player1Score = request.Player1Score.Value;
                    _match.Player2Score = request.Player2Score.Value;
                }

                if (request.Winner.HasValue)
                    _match.Winner = request.Winner.Value;
                else
                {
                    if (_match.Player1Score > _match.Player2Score)
                        _match.Winner = _match.Player1ID;
                    else
                        _match.Winner = _match.Player2ID;
                }

                _sqlContext.Matches.Update(_match);

                await _sqlContext.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
