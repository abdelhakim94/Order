using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
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
                if (response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<SignUpResultDto>(
                        await response.Content.ReadAsStringAsync(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        UIMessages.HttpBadRequestError = await response.Content.ReadAsStringAsync();
                        return new() { Successful = false, Error = HttpClientResponse.BadRequest };
                    case HttpStatusCode.Unauthorized:
                        return new() { Successful = false, Error = HttpClientResponse.Unauthorized };
                    case HttpStatusCode.NotFound:
                        return new() { Successful = false, Error = HttpClientResponse.NotFound };
                    case HttpStatusCode.InternalServerError:
                        return new() { Successful = false, Error = HttpClientResponse.ServerError };
                    case HttpStatusCode.RequestTimeout:
                        return new() { Successful = false, Error = HttpClientResponse.RequestTimedOut };
                    default:
                        throw new Exception();
                }
            }
            catch (System.Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
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
                if (response.IsSuccessStatusCode)
                {
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
                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        UIMessages.HttpBadRequestError = await response.Content.ReadAsStringAsync();
                        return new() { Successful = false, AdditionalError = HttpClientResponse.BadRequest };
                    case HttpStatusCode.Unauthorized:
                        return new() { Successful = false, AdditionalError = HttpClientResponse.Unauthorized };
                    case HttpStatusCode.NotFound:
                        return new() { Successful = false, AdditionalError = HttpClientResponse.NotFound };
                    case HttpStatusCode.InternalServerError:
                        return new() { Successful = false, AdditionalError = HttpClientResponse.ServerError };
                    case HttpStatusCode.RequestTimeout:
                        return new() { Successful = false, AdditionalError = HttpClientResponse.RequestTimedOut };
                    default:
                        throw new Exception();
                }
            }
            catch (System.Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
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
                if (response.IsSuccessStatusCode)
                {
                    var tokenPair = JsonSerializer.Deserialize<TokenPairDto>(
                        await response.Content.ReadAsStringAsync(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    await authenticationStateProvider.MarkUserAsSignedIn(tokenPair.AccessToken, tokenPair.RefreshToken);
                }
                else
                {
                    await authenticationStateProvider.MarkUserAsSignedOut();
                }
            }
            catch (System.Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
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
                if (response.IsSuccessStatusCode)
                {
                    return HttpClientResponse.Success;
                }
                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        UIMessages.HttpBadRequestError = await response.Content.ReadAsStringAsync();
                        return HttpClientResponse.BadRequest;
                    case HttpStatusCode.Unauthorized:
                        return HttpClientResponse.Unauthorized;
                    case HttpStatusCode.NotFound:
                        return HttpClientResponse.NotFound;
                    case HttpStatusCode.InternalServerError:
                        return HttpClientResponse.ServerError;
                    case HttpStatusCode.RequestTimeout:
                        return HttpClientResponse.RequestTimedOut;
                    default:
                        throw new Exception();
                }
            }
            catch (System.Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
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
                var response = await httpClient.PostAsJsonAsync<ResetPasswordDto>("api/user/ResetPassword", password);
                if (response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<ResetPasswordResultDto>(
                        await response.Content.ReadAsStringAsync(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );
                }
                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        return new() { Successful = false, Error = HttpClientResponse.BadRequest };
                    case HttpStatusCode.Unauthorized:
                        return new() { Successful = false, Error = HttpClientResponse.Unauthorized };
                    case HttpStatusCode.NotFound:
                        return new() { Successful = false, Error = HttpClientResponse.NotFound };
                    case HttpStatusCode.InternalServerError:
                        return new() { Successful = false, Error = HttpClientResponse.ServerError };
                    case HttpStatusCode.RequestTimeout:
                        return new() { Successful = false, Error = HttpClientResponse.RequestTimedOut };
                    default:
                        throw new Exception();
                }
            }
            catch (System.Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
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
