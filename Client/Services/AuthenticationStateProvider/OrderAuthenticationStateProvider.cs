using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using Order.Shared.Contracts;
using Order.Shared.Dto.Account;
using Order.Shared.Security;
using System.Net.Http.Json;
using System.Text.Json;
using Order.Shared.Exceptions;

namespace Order.Client.Services
{
    public class OrderAuthenticationStateProvider : AuthenticationStateProvider, IOrderAuthenticationStateProvider, IScopedService
    {
        private readonly HttpClient httpClient;
        private readonly IHubConnectionService hubConnectionService;
        private readonly ILocalStorageService localStorage;

        public OrderAuthenticationStateProvider(
            HttpClient httpClient,
            IHubConnectionService hubConnectionService,
            ILocalStorageService localStorage)
        {
            this.httpClient = httpClient;
            this.hubConnectionService = hubConnectionService;
            this.localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var accessToken = await localStorage.GetItemAsync<string>(nameof(SignInResultDto.TokenPair.AccessToken));
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                await DeleteTokenFromConnectionClients();
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            try
            {
                var claims = accessToken.ParseClaimsFromJwt();
                await ProvideTokenToConnectionClients(accessToken);
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")));
            }
            catch (System.Exception)
            {
                await DeleteTokenFromConnectionClients();
                await localStorage.RemoveItemAsync(nameof(SignInResultDto.TokenPair.AccessToken));
                await localStorage.RemoveItemAsync(nameof(SignInResultDto.TokenPair.RefreshToken));
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        public async Task MarkUserAsSignedIn(string accessToken, string refreshToken)
        {
            await localStorage.SetItemAsync(nameof(SignInResultDto.TokenPair.AccessToken), accessToken);
            await localStorage.SetItemAsync(nameof(SignInResultDto.TokenPair.RefreshToken), refreshToken);
            await ProvideTokenToConnectionClients(accessToken);
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(
                accessToken.ParseClaimsFromJwt(),
                "jwt"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public async Task MarkUserAsSignedOut()
        {
            await localStorage.RemoveItemAsync(nameof(SignInResultDto.TokenPair.AccessToken));
            await localStorage.RemoveItemAsync(nameof(SignInResultDto.TokenPair.RefreshToken));
            await DeleteTokenFromConnectionClients();
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            NotifyAuthenticationStateChanged(authState);
        }

        private async Task DeleteTokenFromConnectionClients()
        {
            httpClient.DefaultRequestHeaders.Authorization = null;
            await hubConnectionService.ShutDown();
        }

        private async Task ProvideTokenToConnectionClients(string accessToken)
        {
            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
                await hubConnectionService.StartNew(accessToken);
            }
            // Catching any exception and try to refresh tokens assume that the SignalR connection couldn't be
            // established because of an expired access token. This is wrong. The SignalR connection could fail because
            // of other reasons. Fix this later.
            catch (System.Exception)
            {
                var newTokens = await UpdateExpiredTokens();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", newTokens.AccessToken);
                await hubConnectionService.StartNew(newTokens.AccessToken);
            }
        }

        private async Task<TokenPairDto> UpdateExpiredTokens()
        {
            var currentAccessToken = await localStorage.GetItemAsync<string>(nameof(SignInResultDto.TokenPair.AccessToken));
            var currentRefreshToken = await localStorage.GetItemAsync<string>(nameof(SignInResultDto.TokenPair.RefreshToken));

            var response = await httpClient.PostAsJsonAsync<TokenPairDto>("api/user/RefreshExpiredTokens", new()
            {
                AccessToken = currentAccessToken,
                RefreshToken = currentRefreshToken,
            });

            response.EnsureSuccessStatusCode();
            var tokens = JsonSerializer.Deserialize<TokenPairDto>(
                        await response.Content.ReadAsStringAsync(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (tokens is null)
            {
                throw new ApplicationException();
            }

            await localStorage.SetItemAsync(nameof(SignInResultDto.TokenPair.AccessToken), tokens.AccessToken);
            await localStorage.SetItemAsync(nameof(SignInResultDto.TokenPair.RefreshToken), tokens.RefreshToken);
            return tokens;
        }
    }
}
