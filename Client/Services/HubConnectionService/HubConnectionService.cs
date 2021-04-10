using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Order.Shared.Contracts;

namespace Order.Client.Services
{
    public class HubConnectionService : IHubConnectionService, ISingletonService
    {
        private HubConnection hubConnection;
        public bool IsConnected { get => !string.IsNullOrWhiteSpace(LastAccessToken); }
        public string LastAccessToken { get; private set; }

        public async Task StartNew(string accessToken)
        {
            await ShutDown();
            hubConnection = new HubConnectionBuilder()
                .WithUrl("/AppHub", options => options.Headers.Add("Authorization", $"Bearer {accessToken}"))
                .WithAutomaticReconnect(new HubConnectionRetryPolicy())
                .Build();
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
    }
}
