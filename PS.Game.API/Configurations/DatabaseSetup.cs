using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PS.Game.API.Configurations
{
    public static class DatabaseSetup
    {
        public static void AddDatabaseSetup(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MySqlContext>(options => options.UseMySql(
                configuration.GetConnectionString("MySqlConnection")));

            /*services.AddDbContext<MySqlContext>(options => options.UseSqlServer(
                configuration.GetConnectionString("SqlServerConnection")));*/

            services.AddScoped<MySqlContext>();
        }
    }
}
