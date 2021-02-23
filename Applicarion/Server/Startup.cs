using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.ResponseCompression;
using Order.Application.Server.Persistence;
using Order.DomainModel;
using Microsoft.AspNetCore.Identity;

namespace Order.Application.Server
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
            services.AddDbContextPool<IOrderContext, OrderContext>(builder =>
                builder.UseNpgsql(Configuration.GetConnectionString("dev_db_order")
            ));
            services.AddDbContextPool<OrderContext>(builder =>
                builder.UseNpgsql(Configuration.GetConnectionString("dev_db_order"),
                npgopt => npgopt.MigrationsHistoryTable("__ApplicationMigrationHistory")
            ));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<OrderContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Configuration.GetValue<string>("IdentityServerAdress");
                });

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddSignalR().AddMessagePackProtocol();
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();

                // Map hubs here
                // endpoints.MapHub<AccountHub>("/Account");

                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
