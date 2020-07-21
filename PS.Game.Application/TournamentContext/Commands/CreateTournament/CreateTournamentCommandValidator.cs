using Domain.Entities;
using FluentValidation;
using Persistence.Contexts;
using PS.Game.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.TournamentContext.Commands.CreateTournament
{
    public class CreateTournamentCommandValidator : AbstractValidator<CreateTournamentCommand>
    {
        private readonly MySqlContext _sqlContext;

        public CreateTournamentCommandValidator(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        
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
                /*.NotEmpty()
                    .WithMessage("Por favor, informe o modo de jogo disponível no campeonato.")*/
                .IsInEnum()
                    .WithMessage("Por favor, informe um modo de jogo válida.");

            RuleFor(t => t.Game)
                /*.NotEmpty()
                    .WithMessage("Por favor, informe um jogo.")*/
                .IsInEnum()
                    .WithMessage("Por favor, informe um jogo válido.");

            /*RuleFor(t => t.GameID)
                .NotEmpty()
                    .When(t => string.IsNullOrEmpty(t.Game))
                    .WithMessage("Por favor, selecione ou cadastre um jogo.")
                .Must((model, el) => _sqlContext.Set<Tournament>()
                                         .Where(t => t.GameID == el &&
                                                     t.Active &&
                                                     t.Mode == model.Mode)
                                         .FirstOrDefault() == null)
                    .WithMessage("Já existe um torneio ativo para este jogo e modo. Exclua-o antes de criar um novo.");

            RuleFor(t => t.Game)
                .NotEmpty()
                    .When(t => !t.GameID.HasValue)
                    .WithMessage("Por favor, selecione ou cadastre um jogo.");*/
        }
    }
}
