using Application.Services;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.SystemContext.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        private readonly MySqlContext _sqlContext;
        private readonly Util _util;

        public LoginCommandValidator(MySqlContext sqlContext, Util util)
        {
            _sqlContext = sqlContext;
            _util = util;

            RuleFor(c => c.Email)
                .NotEmpty()
                    .WithMessage("Por favor, informe o e-mail do usuário.")
                .EmailAddress()
                    .WithMessage("Por favor, informe um e-mail válido.")
                .Custom((el, validator) =>
                {
                    var _exists = _sqlContext.Set<User>()
                                       .Where(us => us.Email.Equals(el))
                                       .FirstOrDefault();

                    if (_exists == null) validator.AddFailure("E-mail e/ou senha incorreto(s).");
                });

            RuleFor(c => c.Password)
                .NotEmpty()
                    .WithMessage("Por favor, informe a senha do usuário.")
                .Must((model, p) => _sqlContext.Set<User>()
                                        .Where(u => u.Email.Equals(model.Email) &&
                                                    _util.VerifyPassword(u.Password, p))
                                        .FirstOrDefault() != null)
                    .WithMessage("E-mail e/ou senha incorreto(s).");
        }
    }
}
