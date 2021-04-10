using System;
using Microsoft.AspNetCore.SignalR.Client;

namespace Order.Client.Services
{
    public class HubConnectionRetryPolicy : IRetryPolicy
    {
        public TimeSpan? NextRetryDelay(RetryContext retryContext)
        {
            return TimeSpan.FromSeconds(1);
        }
    }
}
