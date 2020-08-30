using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Application.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Persistence.Contexts;

namespace PS.Game.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //BuildWebHost(args).Run();
            CreateWebHostBuilder(args).Build().Run();
        }

        //public static IWebHost BuildWebHost(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .CaptureStartupErrors(true)
        //        .UseSetting("detailedErrors", "true")
        //        .UseStartup<Startup>()
        //        .UseDefaultServiceProvider(options => options.ValidateScopes = false)
        //        .ConfigureServices((context, services) =>
        //        {
        //            services.AddHostedService<BackgroundService>();
        //        })
        //        .Build();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}