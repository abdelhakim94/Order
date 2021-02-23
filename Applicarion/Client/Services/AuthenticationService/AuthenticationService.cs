using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using IdentityModel.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Order.Application.Shared.Dto.Users;

namespace Order.Application.Client.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient httpClient;
        private readonly AuthenticationStateProvider authenticationStateProvider;
        private readonly ILocalStorageService localStorage;
        private readonly IConfiguration configuration;

        public DiscoveryDocumentResponse DiscoveryDocument { get; set; }

        public AuthenticationService(
            HttpClient httpClient,
            AuthenticationStateProvider authenticationStateProvider,
            ILocalStorageService localStorage,
            IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.authenticationStateProvider = authenticationStateProvider;
            this.localStorage = localStorage;
            this.configuration = configuration;
        }

        public async Task<SignUpResultDto> SignUp(UserSignUpDto userInfo)
        {
            var response = await httpClient.PostAsJsonAsync<UserSignUpDto>("api/account/signup", userInfo);
            return JsonSerializer.Deserialize<SignUpResultDto>(
                await response.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<SignInResultDto> SignIn(UserSignInDto userInfo, DiscoveryDocumentResponse discoveryDocument)
        {
            PasswordTokenRequest passwordTokenRequest = new PasswordTokenRequest()
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = configuration.GetValue<string>("ClientId"),
                ClientSecret = configuration.GetValue<string>("ClientSecret"),
                GrantType = configuration.GetValue<string>("GrantType"),
                Scope = configuration.GetValue<string>("Scope"),
                UserName = userInfo.Email,
                Password = userInfo.Password,
            };

            var tokenResponse = await httpClient.RequestPasswordTokenAsync(passwordTokenRequest);

            if (tokenResponse.IsError)
            {
                return new SignInResultDto { Successful = false, Error = tokenResponse.Error };
            }

            await localStorage.SetItemAsync("authToken", tokenResponse.AccessToken);
            await ((OrderAuthenticationStateProvider)authenticationStateProvider).MarkUserAsAuthenticated();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", tokenResponse.AccessToken);

            return new SignInResultDto { Successful = true, Token = tokenResponse.AccessToken };
        }

        public async Task SignOut()
        {
            await localStorage.RemoveItemAsync("authToken");
            ((OrderAuthenticationStateProvider)authenticationStateProvider).MarkUserAsLoggedOut();
            httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
