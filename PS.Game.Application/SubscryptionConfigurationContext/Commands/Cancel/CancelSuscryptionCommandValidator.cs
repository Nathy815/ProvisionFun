using Domain.Entities;
using FluentValidation;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PS.Game.Application.SubscryptionConfigurationContext.Commands.Cancel
{
    public class CancelSuscryptionCommandValidator : AbstractValidator<CancelSubscryptionCommand>
    {
        private readonly MySqlContext _sqlContext;

        public CancelSuscryptionCommandValidator(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;

            RuleFor(r => r.TeamID)
                .NotEmpty()
                    .WithMessage("Por favor, informe o id da inscrição.")
                .Must((model, el) => _sqlContext.Set<Team>()
                                        .Where(t => t.Active && t.Id == el)
                                        .FirstOrDefault() != null)
                    .WithMessage("Por favor, informe uma inscrição válida.");

            RuleFor(r => r.Comments)
                .NotEmpty()
                    .WithMessage("Por favor, informe a razão do cancelamento.");
        }
    }
}
