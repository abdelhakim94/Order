using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Order.Client.Constants;
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
            try
            {
                var response = await httpClient.PostAsJsonAsync<UserSignUpDto>("api/user/SignUp", userInfo);
                return JsonSerializer.Deserialize<SignUpResultDto>(
                    await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (System.Exception)
            {
                return new() { Successful = false, Error = Errors.ServerError };
            }
        }

        public async Task<SignInResultDto> SignIn(UserSignInDto userInfo)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<UserSignInDto>("api/user/SignIn", userInfo);

                var signInResult = JsonSerializer.Deserialize<SignInResultDto>(
                    await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (!signInResult.Successful)
                {
                    return signInResult;
                }

                await authenticationStateProvider.MarkUserAsSignedIn(signInResult.TokenPair.AccessToken, signInResult.TokenPair.RefreshToken);
                return signInResult;
            }
            catch (System.Exception)
            {
                return new() { Successful = false };
            }
        }

        public async Task SignOut()
        {
            try
            {
                await httpClient.GetAsync("api/user/SignOut");
            }
            catch (System.Exception) { }
            finally
            {
                await authenticationStateProvider.MarkUserAsSignedOut();
            }
        }

        public async Task RefreshTokens(string refreshToken)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<string>("api/user/RefreshTokens", refreshToken);
                var tokenPair = JsonSerializer.Deserialize<TokenPairDto>(
                    await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                await authenticationStateProvider.MarkUserAsSignedIn(tokenPair.AccessToken, tokenPair.RefreshToken);
            }
            catch (System.Exception)
            {
                await authenticationStateProvider.MarkUserAsSignedOut();
            }
        }
    }
}
