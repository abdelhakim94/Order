using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Order.Shared.Dto.Users;
using Order.Shared.Interfaces;

namespace Order.Client.Services
{
    public class AuthenticationService : IAuthenticationService, IService
    {
        private readonly HttpClient httpClient;
        private readonly IOrderAuthenticationStateProvider authenticationStateProvider;

        public AuthenticationService(
            HttpClient httpClient,
            IOrderAuthenticationStateProvider authenticationStateProvider)
        {
            this.httpClient = httpClient;
            this.authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<SignUpResultDto> SignUp(UserSignUpDto userInfo)
        {
            var response = await httpClient.PostAsJsonAsync<UserSignUpDto>("api/user/signup", userInfo);
            return JsonSerializer.Deserialize<SignUpResultDto>(
                await response.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<SignInResultDto> SignIn(UserSignInDto userInfo)
        {
            var response = await httpClient.PostAsJsonAsync<UserSignInDto>("api/user/signin", userInfo);

            var signInResult = JsonSerializer.Deserialize<SignInResultDto>(
                await response.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (!response.IsSuccessStatusCode)
            {
                return signInResult;
            }

            await authenticationStateProvider.MarkUserAsSignedIn(signInResult.TokenPair.AccessToken, signInResult.TokenPair.RefreshToken);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", signInResult.TokenPair.AccessToken);
            return signInResult;
        }

        public async Task SignOut()
        {
            await authenticationStateProvider.MarkUserAsSignedOut();
            httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
