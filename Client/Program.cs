using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using Order.Client.Services;
using Order.Shared.Contracts;
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
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress.Replace("app/", ""))
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

            builder.Services.AddSingleton<IdentityErrorDescriber>();

            await builder.Build().RunAsync();
        }
    }
}
