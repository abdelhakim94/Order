using Order.Client.Constants;

namespace Order.Client.Extensions
{
    public static class StringExtensions
    {
        public static bool IsHttpClientError(this string source)
        {
            return source == HttpClientResponse.BadRequest
                || source == HttpClientResponse.Unauthorized
                || source == HttpClientResponse.NotFound
                || source == HttpClientResponse.ServerError
                || source == HttpClientResponse.RequestTimedOut
                || source == HttpClientResponse.InternalError;
        }

        public static bool IsHttpClientSuccessful(this string source)
        {
            return source == HttpClientResponse.Success;
        }
    }
}
