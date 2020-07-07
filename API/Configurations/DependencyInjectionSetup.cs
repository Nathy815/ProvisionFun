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

namespace API.Configurations
{
    public static class DependencyInjectionSetup
    {
        public static void AddDependencyInjection(this IServiceCollection services)
        {
            #region Services

            services.AddTransient<Util>();

            #endregion

            #region SystemContext

            services.AddTransient<IRequestHandler<CreateUserCommand, bool>, CreateUserCommandHandler>()
                    .AddTransient<IRequestHandler<DeleteUserCommand, bool>, DeleteUserCommandHandler>()
                    .AddTransient<IRequestHandler<LoginCommand, string>, LoginCommandHandler>()
                    .AddTransient<IRequestHandler<UpdateUserCommand, bool>, UpdateUserCommandHandler>();

            services.AddTransient<IRequestHandler<ListRolesQuery, List<GetRolesQueryVM>>, ListRolesQueryHandler>()
                    .AddTransient<IRequestHandler<GetUserQuery, GetUserQueryVM>, GetUserQueryHandler>()
                    .AddTransient<IRequestHandler<ListUsersQuery, List<GetUserQueryVM>>, ListUsersQueryHandler>();

            services.AddScoped<IValidator<CreateUserCommand>, CreateUserCommandValidator>()
                    .AddScoped<IValidator<LoginCommand>, LoginCommandValidator>()
                    .AddScoped<IValidator<UpdateUserCommand>, UpdateUserCommandValidator>();

            #endregion

            #region TemplateContext

            services.AddTransient<IRequestHandler<UpdateSetupCommand, bool>, UpdateSetupCommandHandler>();

            services.AddTransient<IRequestHandler<ListTemplateTournamentsQuery, List<TemplateGameVM>>, ListTemplateTournamentsQueryHandler>()
                    .AddTransient<IRequestHandler<GetHomeQuery, GetHomeQueryVM>, GetHomeQueryHandler>()
                    .AddTransient<IRequestHandler<ListSetupsQuery, GetSetupQueryVM>, ListSetupsQueryHandler>();

            #endregion

            #region Tournament

            services.AddTransient<IRequestHandler<CreateTournamentCommand, bool>, CreateTournamentCommandHandler>()
                    .AddTransient<IRequestHandler<UpdateTournamentCommand, bool>, UpdateTournamentCommandHandler>();

            services.AddTransient<IRequestHandler<GetGameQuery, GetGameQueryVM>, GetGameQueryHandler>()
                    .AddTransient<IRequestHandler<GetTournamentQuery, GetTournamentQueryVM>, GetTournamentQueryHandler>()
                    .AddTransient<IRequestHandler<ListGamesQuery, List<GetGameQueryVM>>, ListGamesQueryHandler>()
                    .AddTransient<IRequestHandler<ListTournamentsQuery, List<GetTournamentQueryVM>>, ListTournamentQueryHandler>();

            services.AddScoped<IValidator<CreateTournamentCommand>, CreateTournamentCommandValidator>()
                    .AddScoped<IValidator<UpdateTournamentCommand>, UpdateTournamentCommandValidator>();

            #endregion

        }
    }
}
