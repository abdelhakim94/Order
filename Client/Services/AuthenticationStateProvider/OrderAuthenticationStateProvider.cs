using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using Order.Shared.Contracts;
using Order.Shared.Dto.Account;

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
                var claims = ParseClaimsFromJwt(accessToken);
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
                ParseClaimsFromJwt(accessToken),
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

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadJwtToken(jwt);
            return securityToken.Claims;
        }

        private async Task DeleteTokenFromConnectionClients()
        {
            httpClient.DefaultRequestHeaders.Authorization = null;
            await hubConnectionService.ShutDown();
        }

        private async Task ProvideTokenToConnectionClients(string accessToken)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
            await hubConnectionService.StartNew(accessToken);
        }
    }
}
