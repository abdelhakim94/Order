using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Order.Client.Components;
using Order.Client.Constants;
using Order.Shared.Contracts;

namespace Order.Client.Services
{
    public class HubConnectionService : IHubConnectionService, ISingletonService, IAsyncDisposable
    {
        private readonly IWebAssemblyHostEnvironment hostEnvironment;
        private HubConnection hubConnection;

        public HubConnectionService(IWebAssemblyHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }

        public async Task StartNew(string accessToken)
        {
            await ShutDown();
            hubConnection = new HubConnectionBuilder()
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

        public async Task<T> Invoke<T>(string methodName, Toast toast = default(Toast))
        {
            if (hubConnection is not null && hubConnection.State == HubConnectionState.Connected)
            {
                try
                {
                    return await hubConnection.InvokeAsync<T>(methodName);
                }
                catch (System.Exception)
                {
                    if (toast is not default(Toast))
                    {
                        LogInternalError(toast);
                        return default(T);
                    }
                    throw;
                }
            }
            if (toast is not default(Toast))
            {
                LogConnectionLost(toast);
                return default(T);
            }
            throw new ApplicationException(UIMessages.ConnectionLost);
        }

        public async Task<T> Invoke<T, U>(string methodName, U arg1, Toast toast = default(Toast))
        {
            if (hubConnection is not null && hubConnection.State == HubConnectionState.Connected)
            {
                try
                {
                    return await hubConnection.InvokeAsync<T>(methodName, arg1);
                }
                catch (System.Exception)
                {
                    if (toast is not default(Toast))
                    {
                        LogInternalError(toast);
                        return default(T);
                    }
                    throw;
                }
            }
            if (toast is not default(Toast))
            {
                LogConnectionLost(toast);
                return default(T);
            }
            throw new ApplicationException(UIMessages.ConnectionLost);
        }

        void LogConnectionLost(Toast toast)
        {
            toast.ShowError(UIMessages.ConnectionLost);
        }

        void LogInternalError(Toast toast)
        {
            toast.ShowError(UIMessages.DefaultInternalError);
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            await hubConnection.DisposeAsync();
        }
    }
}
