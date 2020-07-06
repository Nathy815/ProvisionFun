using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.TournamentContext.Commands.UpdateTournament
{
    public class UpdateTournamentCommandValidator : AbstractValidator<UpdateTournamentCommand>
    {
        public UpdateTournamentCommandValidator()
        {
            RuleFor(t => t.Id)
                .NotEmpty()
                    .WithMessage("Por favor, informe o id do campeonato.");

            RuleFor(t => t.Name)
                .NotEmpty()
                    .WithMessage("Por favor, informe o nome do campeonato.");

            RuleFor(t => t.StartSubscryption)
                .NotEmpty()
                    .WithMessage("Por favor, informe a data de início de inscrições do campeonato.")
                .GreaterThanOrEqualTo(DateTime.Now)
                    .WithMessage("Por favor, informe uma data atual.");

            RuleFor(t => t.EndSubscryption)
                .NotEmpty()
                    .WithMessage("Por favor, informe a data de encerramento das incrições do campeonato.")
                .GreaterThan(t => t.StartSubscryption)
                    .WithMessage("A data de encerramento deve ser maior que a de início.");

            RuleFor(t => t.Mode)
                .NotEmpty()
                    .WithMessage("Por favor, informe o modo de jogo disponível no campeonato.")
                .IsInEnum()
                    .WithMessage("Por favor, informe um modo de jogo válida.");

            RuleFor(t => t.GameID)
                .NotEmpty()
                    .When(t => string.IsNullOrEmpty(t.Game))
                    .WithMessage("Por favor, selecione ou cadastre um jogo.");

            RuleFor(t => t.Game)
                .NotEmpty()
                    .When(t => !t.GameID.HasValue)
                    .WithMessage("Por favor, selecione ou cadastre um jogo.");
        }
    }
}
