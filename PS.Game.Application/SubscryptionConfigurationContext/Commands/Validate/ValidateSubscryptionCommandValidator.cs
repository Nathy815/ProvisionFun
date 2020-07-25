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
                                                !string.IsNullOrEmpty(t.Condominium.Name) &&
                                                t.Condominium.Validated)
                                    .FirstOrDefault() == null)
                    .WithMessage("Por favor, informe o nome do condomínio para validação.");

            RuleFor(r => r.ZipCode)
                .NotEmpty()
                    .When(r => IsAddressMandatory(r.Id))
                    .WithMessage("Por favor, informe o CEP do condomínio.");

            RuleFor(r => r.Address)
                .NotEmpty()
                    .When(r => IsAddressMandatory(r.Id))
                    .WithMessage("Por favor, informe o endereço do condomínio.");

            RuleFor(r => r.Number)
                .NotEmpty()
                    .When(r => IsAddressMandatory(r.Id))
                    .WithMessage("Por favor, informe o número do condomínio.");

            RuleFor(r => r.District)
                .NotEmpty()
                    .When(r => IsAddressMandatory(r.Id))
                    .WithMessage("Por favor, informe o bairro do condomínio.");

            RuleFor(r => r.City)
                .NotEmpty()
                    .When(r => IsAddressMandatory(r.Id))
                    .WithMessage("Por favor, informe a cidade do condomínio.");

            RuleFor(r => r.State)
                .NotEmpty()
                    .When(r => IsAddressMandatory(r.Id))
                    .WithMessage("Por favor, informe o Estado do condomínio.");
        }

        public bool IsAddressMandatory(Guid id)
        {
            var _team = _sqlContext.Set<Team>()
                            .Include(t => t.Condominium)
                            .Where(t => t.Id == id)
                            .FirstOrDefault();

            return _team.Condominium.Validated ? false : !string.IsNullOrEmpty(_team.Condominium.Name) ? true : false;
        }
    }
}
