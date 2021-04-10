using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using Order.Shared.Contracts;
using Order.Shared.Dto.Users;
using System.Linq;

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

            IEnumerable<Claim> claims;
            try
            {
                claims = ParseClaimsFromJwt(accessToken);
            }
            catch (System.Exception)
            {
                await DeleteTokenFromConnectionClients();
                await localStorage.RemoveItemAsync(nameof(SignInResultDto.TokenPair.AccessToken));
                await localStorage.RemoveItemAsync(nameof(SignInResultDto.TokenPair.RefreshToken));
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            await ProvideTokenToConnectionClients(accessToken);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")));
        }

        public async Task MarkUserAsSignedIn(string accessToken, string refreshToken)
        {
            await localStorage.SetItemAsync(nameof(SignInResultDto.TokenPair.AccessToken), accessToken);
            await localStorage.SetItemAsync(nameof(SignInResultDto.TokenPair.RefreshToken), refreshToken);
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(
                ParseClaimsFromJwt(accessToken),
                "jwt"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            await ProvideTokenToConnectionClients(accessToken);
            NotifyAuthenticationStateChanged(authState);
        }

        public async Task MarkUserAsSignedOut()
        {
            await localStorage.RemoveItemAsync(nameof(SignInResultDto.TokenPair.AccessToken));
            await localStorage.RemoveItemAsync(nameof(SignInResultDto.TokenPair.RefreshToken));
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            await DeleteTokenFromConnectionClients();
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
            if (!string.IsNullOrWhiteSpace(httpClient.DefaultRequestHeaders.Authorization?.Parameter))
                httpClient.DefaultRequestHeaders.Authorization = null;
            if (hubConnectionService.IsConnected)
                await hubConnectionService.ShutDown();
        }

        private async Task ProvideTokenToConnectionClients(string accessToken)
        {
            if (httpClient.DefaultRequestHeaders.Authorization?.Parameter != accessToken)
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
            if (hubConnectionService.LastAccessToken != accessToken)
                await hubConnectionService.StartNew(accessToken);
        }
    }
}
