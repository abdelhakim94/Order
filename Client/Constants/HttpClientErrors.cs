namespace Order.Client.Constants
{
    public class HttpClientResponse
    {
        // Denotes a successful http request (status code < 299)
        public const string Success = nameof(Success);

        // Denotes a 400 bad request.
        public const string BadRequest = nameof(BadRequest);

        // Denotes a 401 unauthorized error, generally used to prompt the
        // user to reconnect.
        public const string Unauthorized = nameof(Unauthorized);

        // Denotes a 404 not found error, generally because the server
        // is unreachable.
        public const string NotFound = nameof(NotFound);

        // Denotes a 500 status code for a response.
        public const string ServerError = nameof(ServerError);

        // Denotes a timout when sending a request.
        public const string RequestTimedOut = nameof(RequestTimedOut);

        // Denotes an internal error in the client application, for 
        // example, serialization error before/after sending a request.
        public const string InternalError = nameof(InternalError);
    }
}
