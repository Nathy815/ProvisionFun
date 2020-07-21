//using Hangfire;
//using Hangfire.MySql.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PS.Game.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PS.Game.API.Configurations
{
    public static class HangfireSetup
    {
        /*public static void AddHangfire(this IServiceCollection services, IConfiguration _configuration)
        {
            var _options = new MySqlStorageOptions()
            {
                TransactionIsolationLevel = IsolationLevel.ReadCommitted,
                QueuePollInterval = TimeSpan.FromSeconds(15),
                JobExpirationCheckInterval = TimeSpan.FromHours(1),
                CountersAggregateInterval = TimeSpan.FromMinutes(5),
                PrepareSchemaIfNecessary = true,
                DashboardJobListLimit = 50000,
                TransactionTimeout = TimeSpan.FromMinutes(1)
            };

            services.AddHangfire(configuration => configuration
                .UseStorage(
                    new MySqlStorage(
                        _configuration.GetConnectionString("HangfireConnection"), 
                        _options           
                    )
                )
            );

            services.AddHangfireServer();

            #region ScheduleJobs

            var provider = services.BuildServiceProvider();
            var hangfireJobs = provider.GetService<IHangfire>();

            JobStorage.Current = new MySqlStorage(_configuration.GetConnectionString("HangfireConnection"), _options);

            RecurringJob.AddOrUpdate(() => hangfireJobs.GerarPartidas(), Cron.Daily());

            #endregion
        }

        public static void UseHangfire(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                DashboardTitle = "Provision Fun - Hangfire"
            });
        }*/
    }
}
