using Domain.Entities;
using FluentValidation;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.SystemContext.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly MySqlContext _sqlContext;

        public CreateUserCommandValidator(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;

            RuleFor(c => c.Name)
                .NotEmpty()
                    .WithMessage("Por favor, informe o nome do usuário.");

            RuleFor(c => c.Email)
                .NotEmpty()
                    .WithMessage("Por favor, informe o e-mail do usuário.")
                .EmailAddress()
                    .WithMessage("Por favor, informe um e-mail válido.")
                .Custom((el, validator) =>
                {
                    var _exists = _sqlContext.Set<User>()
                                       .Where(u => u.Active && u.Email.Equals(el))
                                       .FirstOrDefault();

                    if (_exists != null) validator.AddFailure("E-mail já cadastrado.");
                });

            RuleFor(c => c.RoleID)
                .NotEmpty()
                    .WithMessage("Por favor, informe o tipo de usuário.");
        }
    }
}
