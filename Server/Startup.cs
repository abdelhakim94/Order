using System;
using System.Reflection;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MediatR;
using Order.DomainModel;
using Order.Server.Persistence;
using Order.Shared.Interfaces;
using Order.Server.Dto.Users;
using Order.Shared.Security.Policies;
using Order.Shared.Security.Constants;

namespace Order.Server
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
                builder.UseNpgsql(Configuration.GetConnectionString("dev_db_order_antony")
            ));

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.Scan(scan => scan
               .FromCallingAssembly()
               .AddClasses(classes => classes.AssignableTo<IService>())
               .AsImplementedInterfaces()
               .WithScopedLifetime());

            #region Identity
            services.AddDatabaseDeveloperPageExceptionFilter();

            // Since "AddEntityFrameworkStores" depends on a concrete implementation
            // of DbContext, we need to add "OrderContext" in this way.
            services.AddScoped<OrderContext>(provider =>
                provider.GetRequiredService<IOrderContext>() as OrderContext);

            var jwtTokenConfig = Configuration.GetSection("JwtTokenConfig").Get<JwtTokenConfigDto>();
            var emailBoxConfig = Configuration.GetSection("EmailBox").Get<EmailBox>();
            services.AddSingleton(jwtTokenConfig);
            services.AddSingleton(emailBoxConfig);

            services.AddIdentity<User, Role>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.SignIn.RequireConfirmedEmail = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 8;
                    options.Lockout.AllowedForNewUsers = true;
                    options.Lockout.MaxFailedAccessAttempts = UserConstants.MAX_FAILED_SIGNIN;
                    options.Lockout.DefaultLockoutTimeSpan = UserConstants.LOCKOUT_TIME;
                })
                .AddEntityFrameworkStores<OrderContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtTokenConfig.Issuer,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenConfig.secret)),
                        ValidateAudience = true,
                        ValidAudience = jwtTokenConfig.Audience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(1),
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(IsGuest.Name, IsGuest.Policy);
            });
            #endregion

            services.AddSwaggerGen(setup =>
            {
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });

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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order");
                });
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
            // app.Use(options => options.Request);
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers()
                    .RequireAuthorization(IsGuest.Name);

                // Map hubs here
                // endpoints.MapHub<AccountHub>("/Account").RequireAuthorization(IsGuest.Name);

                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
