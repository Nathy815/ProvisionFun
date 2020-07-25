using Application.Services;
using Application.SystemContext.Commands.CreateUser;
using Application.SystemContext.Commands.DeleteUser;
using Application.SystemContext.Commands.Login;
using Application.SystemContext.Commands.UpdateUser;
using Application.SystemContext.Queries;
using Application.TemplateContext.Commands.UpdateSetup;
using Application.TemplateContext.Queries;
using Application.TournamentContext.Commands.CreateTournament;
using Application.TournamentContext.Commands.UpdateTournament;
using Application.TournamentContext.Queries;
using Domain.ViewModels;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using Application.SubscryptionConfigurationContext.Commands.Create;
using Application.SubscryptionConfigurationContext.Queries;
using Application.SubscryptionConfigurationContext.Commands.Validate;
using Application.SubscryptionConfigurationContext.Commands.Payment;
using PS.Game.Application.Services.Interfaces;
using PS.Game.Application.SubscryptionConfigurationContext.Commands.Cancel;
using PS.Game.Application.SubscryptionConfigurationContext.Queries;
using PS.Game.Domain.ViewModels;
using Application.MatchContext.Commands.Update;
using Application.MatchContext.Commands.Validate;
using PS.Game.Application.MatchContext.Queries;
using Application.MatchContext.Queries;

namespace API.Configurations
{
    public static class DependencyInjectionSetup
    {
        public static void AddDependencyInjection(this IServiceCollection services)
        {
            #region Match

            services.AddTransient<IRequestHandler<UpdateMatchCommand, bool>, UpdateMatchCommandHandler>()
                    .AddTransient<IRequestHandler<ValidateMatchCommand, bool>, ValidateMatchCommandHandler>();

            services.AddTransient<IRequestHandler<GetMatchQuery, MatchVM>, GetMatchQueryHandler>()
                    .AddTransient<IRequestHandler<ListMatchesQuery, MatchesVM>, ListMatchesQueryHandler>()
                    .AddTransient<IRequestHandler<SearchMatchQuery, List<GetMatchQueryVM>>, SearchMatchQueryHandler>();

            services.AddTransient<IValidator<ValidateMatchCommand>, ValidateMatchCommandValidator>();

            #endregion

            #region Subscryptions

            services.AddTransient<IRequestHandler<CreateSubscryptionCommand, bool>, CreateSubscryptionCommandHandler>()
                    .AddTransient<IRequestHandler<ValidateSubscryptionCommand, bool>, ValidateSubscryptionCommandHandler>()
                    .AddTransient<IRequestHandler<ConfirmPaymentCommand, bool>, ConfirmPaymentCommandHandler>()
                    .AddTransient<IRequestHandler<CancelSubscryptionCommand, bool>, CancelSubscryptionCommandHandler>();

            services.AddTransient<IRequestHandler<ListSubscryptionsQuery, List<GetSubscryptionVM>>, ListSubscryptionsQueryHandler>()
                    .AddTransient<IRequestHandler<GetSubscryptionQuery, GetSubscryptionDetailVM>, GetSubscryptionQueryHandler>()
                    .AddTransient<IRequestHandler<GetShippingQuery, string>, GetShippingQueryHandler>();

            services.AddTransient<IValidator<CreateSubscryptionCommand>, CreateSubscryptionCommandValidator>()
                    .AddTransient<IValidator<ValidateSubscryptionCommand>, ValidateSubscryptionCommandValidator>()
                    .AddTransient<IValidator<CancelSubscryptionCommand>, CancelSuscryptionCommandValidator>();

            #endregion

            #region SystemContext

            services.AddTransient<IRequestHandler<CreateUserCommand, bool>, CreateUserCommandHandler>()
                    .AddTransient<IRequestHandler<DeleteUserCommand, bool>, DeleteUserCommandHandler>()
                    .AddTransient<IRequestHandler<LoginCommand, LoginVM>, LoginCommandHandler>()
                    .AddTransient<IRequestHandler<UpdateUserCommand, bool>, UpdateUserCommandHandler>();

            services.AddTransient<IRequestHandler<ListRolesQuery, List<GetRolesQueryVM>>, ListRolesQueryHandler>()
                    .AddTransient<IRequestHandler<GetUserQuery, GetUserQueryVM>, GetUserQueryHandler>()
                    .AddTransient<IRequestHandler<ListUsersQuery, List<GetUserQueryVM>>, ListUsersQueryHandler>();

            services.AddTransient<IValidator<CreateUserCommand>, CreateUserCommandValidator>()
                    .AddTransient<IValidator<LoginCommand>, LoginCommandValidator>()
                    .AddTransient<IValidator<UpdateUserCommand>, UpdateUserCommandValidator>();

            #endregion

            #region TemplateContext

            services.AddTransient<IRequestHandler<UpdateSetupCommand, bool>, UpdateSetupCommandHandler>();

            services.AddTransient<IRequestHandler<ListTemplateTournamentsQuery, List<TemplateGameVM>>, ListTemplateTournamentsQueryHandler>()
                    .AddTransient<IRequestHandler<GetHomeQuery, GetHomeQueryVM>, GetHomeQueryHandler>()
                    .AddTransient<IRequestHandler<ListSetupsQuery, GetSetupQueryVM>, ListSetupsQueryHandler>()
                    .AddTransient<IRequestHandler<GetAddressQuery, GetCondominiumQueryVM>, GetAddressQueryHandler>()
                    .AddTransient<IRequestHandler<ListCondominiumsQuery, List<GetCondominiumQueryVM>>, ListCondominiumsQueryHandler>();

            #endregion

            #region Tournament

            services.AddTransient<IRequestHandler<CreateTournamentCommand, bool>, CreateTournamentCommandHandler>()
                    .AddTransient<IRequestHandler<UpdateTournamentCommand, bool>, UpdateTournamentCommandHandler>();

            services.AddTransient<IRequestHandler<GetGameQuery, GetGameQueryVM>, GetGameQueryHandler>()
                    .AddTransient<IRequestHandler<GetTopMatchesQuery, List<GetMatchQueryVM>>, GetTopMatchesQueryHandler>()
                    .AddTransient<IRequestHandler<GetTournamentQuery, GetTournamentQueryVM>, GetTournamentQueryHandler>()
                    .AddTransient<IRequestHandler<ListAuditorsQuery, List<GetAuditorsQueryVM>>, ListAuditorsQueryHandler>()
                    .AddTransient<IRequestHandler<ListGamesQuery, List<GetGameQueryVM>>, ListGamesQueryHandler>()
                    .AddTransient<IRequestHandler<ListTournamentsQuery, List<GetTournamentQueryVM>>, ListTournamentsQueryHandler>();

            services.AddTransient<IValidator<CreateTournamentCommand>, CreateTournamentCommandValidator>()
                    .AddTransient<IValidator<UpdateTournamentCommand>, UpdateTournamentCommandValidator>();

            #endregion

            #region Services

            services.AddTransient<IEmail, Email>()
                    .AddTransient<IBoleto, Boleto>()
                    .AddTransient<IUtil, Util>();

            #endregion
        }
    }
}
