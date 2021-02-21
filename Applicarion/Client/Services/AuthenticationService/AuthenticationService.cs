using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Order.Application.Shared.Dto.Users;

namespace Order.Application.Client.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient httpClient;
        private readonly AuthenticationStateProvider authenticationStateProvider;
        private readonly ILocalStorageService localStorage;

        public AuthenticationService(
            HttpClient httpClient,
            AuthenticationStateProvider authenticationStateProvider,
            ILocalStorageService localStorage)
        {
            this.httpClient = httpClient;
            this.authenticationStateProvider = authenticationStateProvider;
            this.localStorage = localStorage;
        }

        public async Task<SignUpResultDto> SignUp(UserSignUpDto userInfo)
        {
            var response = await httpClient.PostAsJsonAsync<UserSignUpDto>("api/account/signup", userInfo);
            return JsonSerializer.Deserialize<SignUpResultDto>(
                await response.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<SignInResultDto> SignIn(UserSignInDto userInfo)
        {
            var userInfoAsJson = JsonSerializer.Serialize(userInfo);
            var response = await httpClient.PostAsync("api/account/signin", new StringContent(userInfoAsJson, Encoding.UTF8, "application/json"));
            var signInResult = JsonSerializer.Deserialize<SignInResultDto>(
                await response.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (!response.IsSuccessStatusCode)
            {
                return signInResult;
            }

            await localStorage.SetItemAsync("authToken", signInResult.Token);
            await ((OrderAuthenticationStateProvider)authenticationStateProvider).MarkUserAsAuthenticated();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", signInResult.Token);

            return signInResult;
        }

        public async Task SignOut()
        {
            await localStorage.RemoveItemAsync("authToken");
            ((OrderAuthenticationStateProvider)authenticationStateProvider).MarkUserAsLoggedOut();
            httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
