using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Blazored.LocalStorage;
using Order.Client.Services;
using Order.Shared.Interfaces;
using Microsoft.Extensions.Options;
using Order.Shared.Security.Policies;

namespace Order.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddSingleton<HttpClient>(sp => new HttpClient
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            });

            builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
                sp.GetRequiredService<IOrderAuthenticationStateProvider>() as AuthenticationStateProvider);

            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore(options =>
            {
                options.AddPolicy(IsGuest.Name, IsGuest.Policy);
            });

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.Scan(scan => scan
               .FromCallingAssembly()
               .AddClasses(classes => classes.AssignableTo<IService>())
               .AsImplementedInterfaces()
               .WithScopedLifetime());

            await builder.Build().RunAsync();
        }
    }
}
