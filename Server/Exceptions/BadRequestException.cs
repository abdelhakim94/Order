using System;

namespace Order.Server.Middlewares
{
    public class BadRequestException : Exception
    {

        public BadRequestException(string message) : base(message) { }
        public BadRequestException(string message, Exception exception) : base(message, exception) { }
    }
}
