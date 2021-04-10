using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Order.Client.Components.Misc;
using Order.Client.Constants;
using Order.Shared.Contracts;

namespace Order.Client.Services
{
    public class HttpClientService : IHttpClientService, IScopedService
    {
        private readonly HttpClient httpClient;
        private readonly IOrderAuthenticationStateProvider authenticationStateProvider;
        private readonly NavigationManager navigationManager;

        public HttpClientService(
            HttpClient httpClient,
            IOrderAuthenticationStateProvider authenticationStateProvider,
            NavigationManager navigationManager)
        {
            this.httpClient = httpClient;
            this.authenticationStateProvider = authenticationStateProvider;
            this.navigationManager = navigationManager;
        }

        public async Task<bool> Get(string url, Toast toast = default(Toast))
        {
            try
            {
                var response = await httpClient.GetAsync(url);
                if (response is not null && response.IsSuccessStatusCode) return true;
                await HandleHttpError(response, toast);
                return false;
            }
            catch (System.Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
            {
                toast?.ShowError(UIMessages.DefaultHttpRequestTimedOut);
                return false;
            }
            catch (System.Exception)
            {
                toast?.ShowError(UIMessages.DefaultInternalError);
                return false;
            }
        }


        public async Task<T> Get<T>(string url, Toast toast = default(Toast))
        {
            try
            {
                var response = await httpClient.GetAsync(url);
                if (response is not null && response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<T>(
                        await response.Content.ReadAsStringAsync(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                await HandleHttpError(response, toast);
                return default(T);
            }
            catch (System.Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
            {
                toast?.ShowError(UIMessages.DefaultHttpRequestTimedOut);
                return default(T);
            }
            catch (System.Exception)
            {
                toast?.ShowError(UIMessages.DefaultInternalError);
                return default(T);
            }
        }

        public async Task<bool> Post<T>(string url, T toSend, Toast toast = default(Toast))
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<T>(url, toSend);
                if (response is not null && response.IsSuccessStatusCode) return true;
                await HandleHttpError(response, toast);
                return false;
            }
            catch (System.Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
            {
                toast?.ShowError(UIMessages.DefaultHttpRequestTimedOut);
                return false;
            }
            catch (System.Exception)
            {
                toast?.ShowError(UIMessages.DefaultInternalError);
                return false;
            }
        }

        public async Task<U> Post<T, U>(string url, T toSend, Toast toast = default(Toast))
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<T>(url, toSend);
                if (response is not null && response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<U>(
                        await response.Content.ReadAsStringAsync(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                await HandleHttpError(response, toast);
                return default(U);
            }
            catch (System.Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
            {
                toast?.ShowError(UIMessages.DefaultHttpRequestTimedOut);
                return default(U);
            }
            catch (System.Exception)
            {
                toast?.ShowError(UIMessages.DefaultInternalError);
                return default(U);
            }
        }

        private async Task HandleHttpError(HttpResponseMessage response, Toast toast)
        {
            var errorMessage = await response?.Content?.ReadAsStringAsync();
            switch (response?.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    toast?.ShowError(!string.IsNullOrWhiteSpace(errorMessage)
                        ? errorMessage
                        : UIMessages.DefaultHttpBadRequestError);
                    return;
                case HttpStatusCode.Unauthorized:
                    await authenticationStateProvider.MarkUserAsSignedOut();
                    navigationManager.NavigateTo("SignIn/");
                    toast?.ShowError(!string.IsNullOrWhiteSpace(errorMessage)
                        ? errorMessage
                        : UIMessages.DefaultHttpUnauthorizedError);
                    return;
                case HttpStatusCode.NotFound:
                    toast?.ShowError(!string.IsNullOrWhiteSpace(errorMessage)
                        ? errorMessage
                        : UIMessages.DefaultHttpNotFoundError);
                    return;
                case HttpStatusCode.InternalServerError:
                    toast?.ShowError(!string.IsNullOrWhiteSpace(errorMessage)
                        ? errorMessage
                        : UIMessages.DefaultHttpServerError);
                    return;
                case HttpStatusCode.RequestTimeout:
                    toast?.ShowError(UIMessages.DefaultHttpRequestTimedOut);
                    return;
                default:
                    throw new System.Exception();
            }
        }
    }
}
