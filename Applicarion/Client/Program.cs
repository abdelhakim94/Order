using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR.Client;
using Order.Application.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using IdentityModel.Client;

namespace Order.Application.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<AuthenticationStateProvider, OrderAuthenticationStateProvider>();
            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddScoped<Task<DiscoveryDocumentResponse>>(provider => provider
                .GetRequiredService<HttpClient>()
                .GetDiscoveryDocumentAsync(provider
                    .GetService<IConfiguration>()
                    .GetValue<string>("IdentityServerAdress")));

            await builder.Build().RunAsync();
        }
    }
}
