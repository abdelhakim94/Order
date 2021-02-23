using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Order.DomainModel;
using Order.IdentityServer.Persistence;
using IdentityServer.Controllers;
using IdentityServer4.Services;
using IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using IdentityServer4.Configuration;

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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "IdentityServer", Version = "v1" });
            });

            services.AddDbContext<ISContext>(options => options.UseNpgsql(
                Configuration.GetConnectionString("dev_db_order"),
                npgsql => npgsql.MigrationsHistoryTable("__IdentityServerMigrationsHistory"))
            );

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<ISContext>()
                .AddDefaultTokenProviders();

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
                .AddAspNetIdentity<User>()
                .AddProfileService<ProfileService>();

            Action<System.IServiceProvider, DbContextOptionsBuilder> ResolveDbContextOptions()
                => (provider, builder) => builder.UseNpgsql(
                    provider.GetService<ISContext>().Database.GetDbConnection().ConnectionString);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IdentityServer v1"));
            }

            app.UseRouting();

            app.UseIdentityServer();

            app.UseEndpoints(builder =>
            {
                builder.MapControllers();
            });
        }
    }
}
