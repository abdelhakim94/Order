using System;

namespace Order.Shared.Exceptions
{
    public class ApplicationException : Exception
    {
        public ApplicationException() : base() { }
        public ApplicationException(string message) : base(message) { }
    }
}
