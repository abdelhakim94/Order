using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Order.Shared.Constants;
using Order.Shared.Dto.Users;
using Order.Shared.Contracts;
using Order.Client.Constants;

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

        public async Task<SignUpResultDto> SignUp(SignUpDto userInfo)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<SignUpDto>("api/user/SignUp", userInfo);
                response.EnsureSuccessStatusCode();
                return JsonSerializer.Deserialize<SignUpResultDto>(
                    await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (HttpRequestException ex)
            {
                switch (ex.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        return new() { Successful = false, Error = HttpClientResponse.Unauthorized };
                    case HttpStatusCode.NotFound:
                        return new() { Successful = false, Error = HttpClientResponse.NotFound };
                    case HttpStatusCode.InternalServerError:
                        return new() { Successful = false, Error = HttpClientResponse.ServerError };
                    default:
                        throw;
                }
            }
            catch (TaskCanceledException)
            {
                return new() { Successful = false, Error = HttpClientResponse.RequestTimedOut };
            }
            catch (System.Exception)
            {
                return new() { Successful = false, Error = HttpClientResponse.InternalError };
            }
        }

        public async Task<SignInResultDto> SignIn(SignInDto userInfo)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<SignInDto>("api/user/SignIn", userInfo);
                response.EnsureSuccessStatusCode();
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
            catch (HttpRequestException ex)
            {
                switch (ex.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        return new() { Successful = false, AdditionalError = HttpClientResponse.Unauthorized };
                    case HttpStatusCode.NotFound:
                        return new() { Successful = false, AdditionalError = HttpClientResponse.NotFound };
                    case HttpStatusCode.InternalServerError:
                        return new() { Successful = false, AdditionalError = HttpClientResponse.ServerError };
                    default:
                        throw;
                }
            }
            catch (TaskCanceledException)
            {
                return new() { Successful = false, AdditionalError = HttpClientResponse.RequestTimedOut };
            }
            catch (System.Exception)
            {
                return new() { Successful = false, AdditionalError = HttpClientResponse.InternalError };
            }
        }

        public async Task SignOut()
        {
            try
            {
                await httpClient.GetAsync("api/user/SignOut");
            }
            finally
            {
                await authenticationStateProvider.MarkUserAsSignedOut();
            }
        }

        public async Task RefreshTokens(RefreshTokensDto refreshToken)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<RefreshTokensDto>("api/user/RefreshTokens", refreshToken);
                response.EnsureSuccessStatusCode();
                var tokenPair = JsonSerializer.Deserialize<TokenPairDto>(
                    await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                await authenticationStateProvider.MarkUserAsSignedIn(tokenPair.AccessToken, tokenPair.RefreshToken);
            }
            catch (TaskCanceledException)
            {
                return;
            }
            catch (System.Exception)
            {
                await authenticationStateProvider.MarkUserAsSignedOut();
            }
        }

        public async Task<string> RequestResetPassword(RequestResetPasswordDto userEmail)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<RequestResetPasswordDto>("api/user/RequestResetPassword", userEmail);
                response.EnsureSuccessStatusCode();
                return HttpClientResponse.Success;
            }
            catch (HttpRequestException ex)
            {
                switch (ex.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        return HttpClientResponse.Unauthorized;
                    case HttpStatusCode.NotFound:
                        return HttpClientResponse.NotFound;
                    case HttpStatusCode.InternalServerError:
                        return HttpClientResponse.ServerError;
                    default:
                        throw;
                }
            }
            catch (TaskCanceledException)
            {
                return HttpClientResponse.RequestTimedOut;
            }
            catch (System.Exception)
            {
                return HttpClientResponse.InternalError;
            }
        }

        public async Task<ResetPasswordResultDto> ResetPassword(ResetPasswordDto password)
        {
            try
            {
                var result = await httpClient.PostAsJsonAsync<ResetPasswordDto>("api/user/ResetPassword", password);
                result.EnsureSuccessStatusCode();
                return JsonSerializer.Deserialize<ResetPasswordResultDto>(
                    await result.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
            }
            catch (HttpRequestException ex)
            {
                switch (ex.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        return new() { Successful = false, Error = HttpClientResponse.Unauthorized };
                    case HttpStatusCode.NotFound:
                        return new() { Successful = false, Error = HttpClientResponse.NotFound };
                    case HttpStatusCode.InternalServerError:
                        return new() { Successful = false, Error = HttpClientResponse.ServerError };
                    default:
                        throw;
                }
            }
            catch (TaskCanceledException)
            {
                return new() { Successful = false, Error = HttpClientResponse.RequestTimedOut };
            }
            catch (System.Exception)
            {
                return new() { Successful = false, Error = HttpClientResponse.InternalError };
            }
        }
    }
}
