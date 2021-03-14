using System;
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
    public class HttpClientService : IHttpClientService, IService
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

        public async Task<bool> Get(string url, NotificationModal notificationModal = default(NotificationModal))
        {
            try
            {
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode) return true;
                await HandleHttpError(response, notificationModal);
                return false;
            }
            catch (System.Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
            {
                notificationModal?.ShowError(UIMessages.DefaultHttpRequestTimedOut);
                return false;
            }
            catch (System.Exception)
            {
                notificationModal?.ShowError(UIMessages.DefaultInternalError);
                return false;
            }
        }


        public async Task<T> Get<T>(string url, NotificationModal notificationModal = default(NotificationModal))
        {
            try
            {
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<T>(
                        await response.Content.ReadAsStringAsync(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                await HandleHttpError(response, notificationModal);
                return default(T);
            }
            catch (System.Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
            {
                notificationModal?.ShowError(UIMessages.DefaultHttpRequestTimedOut);
                return default(T);
            }
            catch (System.Exception)
            {
                notificationModal?.ShowError(UIMessages.DefaultInternalError);
                return default(T);
            }
        }

        public async Task<bool> Post<T>(string url, T toSend, NotificationModal notificationModal = default(NotificationModal))
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<T>(url, toSend);
                if (response.IsSuccessStatusCode) return true;
                await HandleHttpError(response, notificationModal);
                return false;
            }
            catch (System.Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
            {
                notificationModal?.ShowError(UIMessages.DefaultHttpRequestTimedOut);
                return false;
            }
            catch (System.Exception)
            {
                notificationModal?.ShowError(UIMessages.DefaultInternalError);
                return false;
            }
        }

        public async Task<U> Post<T, U>(string url, T toSend, NotificationModal notificationModal = default(NotificationModal))
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<T>(url, toSend);
                if (response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<U>(
                        await response.Content.ReadAsStringAsync(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                await HandleHttpError(response, notificationModal);
                return default(U);
            }
            catch (System.Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
            {
                Console.WriteLine("inside time out exception");
                notificationModal?.ShowError(UIMessages.DefaultHttpRequestTimedOut);
                return default(U);
            }
            catch (System.Exception)
            {
                Console.WriteLine("inside general exception");
                notificationModal?.ShowError(UIMessages.DefaultInternalError);
                return default(U);
            }
        }

        private async Task HandleHttpError(HttpResponseMessage response, NotificationModal notificationModal)
        {
            var errorMessage = await response?.Content?.ReadAsStringAsync();
            Console.WriteLine("inside http error handler");
            switch (response?.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    notificationModal?.ShowError(!string.IsNullOrWhiteSpace(errorMessage)
                        ? errorMessage
                        : UIMessages.DefaultHttpBadRequestError);
                    return;
                case HttpStatusCode.Unauthorized:
                    await authenticationStateProvider.MarkUserAsSignedOut();
                    navigationManager.NavigateTo("/");
                    notificationModal?.ShowError(!string.IsNullOrWhiteSpace(errorMessage)
                        ? errorMessage
                        : UIMessages.DefaultHttpUnauthorizedError);
                    return;
                case HttpStatusCode.NotFound:
                    notificationModal?.ShowError(!string.IsNullOrWhiteSpace(errorMessage)
                        ? errorMessage
                        : UIMessages.DefaultHttpNotFoundError);
                    return;
                case HttpStatusCode.InternalServerError:
                    notificationModal?.ShowError(!string.IsNullOrWhiteSpace(errorMessage)
                        ? errorMessage
                        : UIMessages.DefaultInternalError);
                    return;
                case HttpStatusCode.RequestTimeout:
                    notificationModal?.ShowError(UIMessages.DefaultHttpRequestTimedOut);
                    return;
                default:
                    Console.WriteLine("made it to default case");
                    throw new System.Exception();
            }
        }
    }
}