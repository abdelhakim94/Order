using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.ResponseCompression;
using Order.Server.Model;

namespace Order.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<IOrderContext, OrderContext>(builder =>
                builder.UseNpgsql(Configuration.GetConnectionString("dev_db_order")
            ));

            #region Identity
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddIdentity<User, Role>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<OrderContext>();

            services.AddIdentityServer()
                .AddConfigurationStore<OrderContext>(options =>
                {
                    options.ConfigureDbContext = (builder) => builder.UseNpgsql(
                        Configuration.GetConnectionString("dev_db_order"),
                        npgsql => npgsql
                            .MigrationsAssembly(Assembly
                                .GetExecutingAssembly()
                                .GetName()
                                .ToString())
                            .MigrationsHistoryTable("__EFMigrationsHistory"));
                })
                .AddOperationalStore<OrderContext>(options =>
                {
                    options.ConfigureDbContext = (builder) => builder.UseNpgsql(
                        Configuration.GetConnectionString("dev_db_order"),
                        npgsql => npgsql
                            .MigrationsAssembly(Assembly
                                .GetExecutingAssembly()
                                .GetName()
                                .ToString())
                            .MigrationsHistoryTable("__EFMigrationsHistory"));
                    options.EnableTokenCleanup = true;
                })
                .AddApiAuthorization<User, OrderContext>();

            services.AddAuthentication()
                .AddIdentityServerJwt();
            #endregion

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddSignalR().AddMessagePackProtocol();
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();

                // Map hubs here
                // endpoints.MapHub<FooHub>("/Foo");

                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
