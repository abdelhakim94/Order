using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Order.Client.Components.Misc;
using Order.Client.Constants;
using Order.Shared.Contracts;

namespace Order.Client.Services
{
    public class HubConnectionService : IHubConnectionService, ISingletonService
    {
        private readonly IWebAssemblyHostEnvironment hostEnvironment;
        private HubConnection hubConnection;
        public bool IsConnected { get => !string.IsNullOrWhiteSpace(LastAccessToken); }
        public string LastAccessToken { get; private set; }

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
            LastAccessToken = accessToken;
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
            LastAccessToken = null;
        }

        public async Task<T> Invoke<T>(string methodName, Toast toast = default(Toast))
        {
            if (hubConnection is not null && hubConnection.State != HubConnectionState.Disconnected)
            {
                try
                {
                    var response = await hubConnection.InvokeAsync<T>(methodName);
                    return response;
                }
                catch (System.Exception)
                {
                    if (toast != default(Toast))
                    {
                        toast.ShowError(UIMessages.DefaultSignalRInvocationError);
                    }
                    return default(T);
                }
            }
            if (toast != default(Toast))
            {
                toast.ShowError(UIMessages.DefaultSignalRInvocationError);
            }
            return default(T);
        }
    }
}
