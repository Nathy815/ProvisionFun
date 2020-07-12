using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.SubscryptionConfigurationContext.Commands.Validate
{
    public class ValidateSubscryptionCommandValidator : AbstractValidator<ValidateSubscryptionCommand>
    {
        private readonly MySqlContext _sqlContext;

        public ValidateSubscryptionCommandValidator(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;

            RuleFor(r => r.Id)
                .NotEmpty()
                    .WithMessage("Por favor, informe o id da inscrição.");

            RuleFor(r => r.Condominium)
                .NotEmpty()
                    .When(r => _sqlContext.Set<Team>()
                                    .Include(t => t.Condominium)
                                    .Where(t => t.Id == r.Id &&
                                                t.Condominium.Validated)
                                    .FirstOrDefault() == null &&
                               r.Validate)
                    .WithMessage("Por favor, informe o nome do condomínio para validação.");
        }
    }
}
