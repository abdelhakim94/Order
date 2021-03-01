using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using Order.Shared.Interfaces;
using Order.Shared.Dto.Users;

namespace Order.Client.Services
{
    public class OrderAuthenticationStateProvider : AuthenticationStateProvider, IOrderAuthenticationStateProvider, IService
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorage;

        public OrderAuthenticationStateProvider(
            HttpClient httpClient,
            ILocalStorageService localStorage)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var accessToken = await localStorage.GetItemAsync<string>(nameof(SignInResultDto.TokenPair.AccessToken));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(accessToken), "jwt")));
            }
            catch (System.Exception)
            {
                httpClient.DefaultRequestHeaders.Authorization = null;
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        public async Task MarkUserAsSignedIn(string accessToken, string refreshToken)
        {
            await localStorage.SetItemAsync(nameof(SignInResultDto.TokenPair.AccessToken), accessToken);
            await localStorage.SetItemAsync(nameof(SignInResultDto.TokenPair.RefreshToken), refreshToken);
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
    }
}
