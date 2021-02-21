using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Order.DomainModel;
using Order.IdentityServer.Persistence;

namespace Order.IdentityServer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ISContext>(options => options.UseNpgsql(
                Configuration.GetConnectionString("dev_db_order"),
                npgsql => npgsql.MigrationsHistoryTable("__IdentityServerMigrationsHistory"))
            );

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<ISContext>();

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddConfigurationStore<ISContext>(options =>
                {
                    options.ResolveDbContextOptions = ResolveDbContextOptions();
                })
               .AddOperationalStore<ISContext>(options =>
                {
                    options.ResolveDbContextOptions = ResolveDbContextOptions();
                    options.EnableTokenCleanup = true;
                })
                .AddAspNetIdentity<User>();

            Action<System.IServiceProvider, DbContextOptionsBuilder> ResolveDbContextOptions()
                => (provider, builder) => builder.UseNpgsql(
                    provider.GetService<ISContext>().Database.GetDbConnection().ConnectionString);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
        }
    }
}
