using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.MatchContext.Commands.Validate
{
    public class ValidateMatchCommandValidator : AbstractValidator<ValidateMatchCommand>
    {
        public ValidateMatchCommandValidator()
        {
            RuleFor(r => r.MatchID)
                .NotEmpty()
                    .WithMessage("Por favor, selecione a partida.");

            RuleFor(r => r.Winner)
                .NotEmpty()
                    .When(r => !r.Player1Score.HasValue ||
                               !r.Player2Score.HasValue)
                    .WithMessage("Por favor, informe o vencedor e/ou o placar da partida.");

            RuleFor(r => r.Player1Score)
                .NotEmpty()
                    .When(r => !r.Winner.HasValue)
                    .WithMessage("Por favor, informe o vencedor e/ou o placar da partida.");

            RuleFor(r => r.Player2Score)
                .NotEmpty()
                    .When(r => !r.Winner.HasValue)
                    .WithMessage("Por favor, informe o vencedor e/ou o placar da partida.");
        }
    }
}
