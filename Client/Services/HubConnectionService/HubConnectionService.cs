using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Order.Client.Constants;
using Order.Shared.Contracts;

namespace Order.Client.Services
{
    public class HubConnectionService : IHubConnectionService, ISingletonService, IAsyncDisposable
    {
        private readonly IWebAssemblyHostEnvironment hostEnvironment;
        private HubConnection hubConnection;
        public event EventHandler<Exception> OnReconnecting;

        public HubConnectionService(IWebAssemblyHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }

        public async Task StartNew(string accessToken)
        {
            await ShutDown();
            hubConnection = new HubConnectionBuilder()
                // .WithUrl($"{hostEnvironment.BaseAddress.Replace("/app/", "")}/AppHub", options => options.Headers.Add("Authorization", $"Bearer {accessToken}"))
                .WithUrl($"{hostEnvironment.BaseAddress.Replace("/app/", "")}/AppHub", options => options.AccessTokenProvider = () => Task.FromResult(accessToken))
                .WithAutomaticReconnect(new HubConnectionRetryPolicy())
                .AddMessagePackProtocol()
                .Build();
            await hubConnection.StartAsync();
        }

        public async Task ShutDown()
        {
            if (hubConnection is not null)
            {
                if (hubConnection.State != HubConnectionState.Disconnected)
                {
                    await hubConnection.StopAsync();
                }
                await hubConnection.DisposeAsync();
                hubConnection = null;
            }
        }

        public async Task<T> Invoke<T>(string methodName)
        {
            // Change this:
            // See if we can know why an invocation failed.
            // If the reason is authorization, mark the user as signed out.
            // Hook an event handler (to the reconnection event or the 
            // disconected event ot the reconnecting method during connection
            // build) to see if it is an authorization problem that caused
            // the deconnection, and if so, mark the user as signed out.
            if (hubConnection is not null && hubConnection.State != HubConnectionState.Disconnected)
            {
                return await hubConnection.InvokeAsync<T>(methodName);
            }
            throw new ApplicationException(UIMessages.DefaultSignalRInvocationError);
        }

        public async Task<T> Invoke<T, U>(string methodName, U arg1)
        {
            if (hubConnection is not null && hubConnection.State != HubConnectionState.Disconnected)
            {
                return await hubConnection.InvokeAsync<T>(methodName, arg1);
            }
            throw new ApplicationException(UIMessages.DefaultSignalRInvocationError);
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            await hubConnection.DisposeAsync();
        }
    }
}
