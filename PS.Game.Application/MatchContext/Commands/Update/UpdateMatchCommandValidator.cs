using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.MatchContext.Commands.Update
{
    public class UpdateMatchCommandValidator : AbstractValidator<UpdateMatchCommand>
    {
        private readonly MySqlContext _sqlContext;

        public UpdateMatchCommandValidator(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;

            RuleFor(m => m.Id)
                .NotEmpty()
                    .WithMessage("Por favor, informe o id da partida.");

            RuleFor(m => m.Date)
                .NotEmpty()
                    .When(m => !IsMatchScheduled(m.Id))
                    .WithMessage("Por favor, informe a data da partida.");

            RuleFor(m => m.AuditorID)
                .NotEmpty()
                    .When(m => !IsMatchScheduled(m.Id))
                    .WithMessage("Por favor, informe o auditor da partida.");

            RuleFor(m => m.Winner)
                .NotEmpty()
                    .When(m => IsMatchScheduled(m.Id))
                    .WithMessage("Por favor, informe o vencedor da partida.");
        }

        private bool IsMatchScheduled(Guid id)
        {
            var _match = _sqlContext.Set<Match>()
                                .Where(m => m.Id == id)
                                .FirstOrDefault();

            return _match.Date.HasValue ? true : false;
        }
    }
}
