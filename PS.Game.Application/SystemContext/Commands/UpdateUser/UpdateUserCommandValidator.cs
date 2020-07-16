using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.SystemContext.Commands.UpdateUser
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                    .WithMessage("Por favor, informe o id do usuário.");

            RuleFor(c => c.Name)
                .NotEmpty()
                    .WithMessage("Por favor, informe o nome do usuário.");

            RuleFor(c => c.RoleID)
                .NotEmpty()
                    .WithMessage("Por favor, informe o tipo de usuário.");
        }
    }
}
