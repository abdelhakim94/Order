using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Order.Shared.Dto.Users;

namespace Order.Client.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly AuthenticationStateProvider authenticationStateProvider;
        private readonly NavigationManager navigationManager;
        private readonly ILocalStorageService localStorage;
        private HubConnection hubConnection;

        public AuthenticationService(
            AuthenticationStateProvider authenticationStateProvider,
            ILocalStorageService localStorage,
            NavigationManager navigationManager)
        {
            this.authenticationStateProvider = authenticationStateProvider;
            this.localStorage = localStorage;
            this.navigationManager = navigationManager;

            hubConnection = new HubConnectionBuilder()
                .WithUrl(this.navigationManager.ToAbsoluteUri("/Account"))
                .AddMessagePackProtocol()
                .Build();
            hubConnection.StartAsync();
        }

        public async Task<SignUpResultDto> SignUp(UserSignUpDto userSignUpData)
        {
            var result = await hubConnection.InvokeAsync<SignUpResultDto>("SignUp", userSignUpData);
            return result;
        }

        public async Task<SignInResultDto> SignIn(UserSignInDto userSignInData)
        {
            var response = await hubConnection.InvokeAsync<SignInResultDto>("SignIn", userSignInData);
            await localStorage.SetItemAsync("authToken", response.Token);

            ((ApiAuthenticationStateProvider)authenticationStateProvider).MarkUserAsAuthenticated(userSignInData.Email);
            await hubConnection.StopAsync();
            hubConnection = new HubConnectionBuilder()
               .WithUrl(this.navigationManager.ToAbsoluteUri("/Account"), options =>
                    options.Headers.Add("Authorization", new AuthenticationHeaderValue("Bearer", response.Token).ToString()))
               .AddMessagePackProtocol()
               .Build();
            await hubConnection.StartAsync();
            return response;
        }

        public async Task SignOut()
        {
            await localStorage.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)authenticationStateProvider).MarkUserAsLoggedOut();
            await hubConnection.StopAsync();
            hubConnection = new HubConnectionBuilder()
               .WithUrl(this.navigationManager.ToAbsoluteUri("/Account"), options =>
                    options.Headers.Add("Authorization", ""))
               .AddMessagePackProtocol()
               .Build();
            await hubConnection.StartAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await hubConnection.DisposeAsync();
        }
    }
}