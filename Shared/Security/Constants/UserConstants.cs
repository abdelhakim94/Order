using System;

namespace Order.Shared.Security.Constants
{
    public class UserConstants
    {
        public static int MAX_FAILED_SIGNIN = 5;
        public static TimeSpan LOCKOUT_TIME = TimeSpan.FromMinutes(5);
    }
}
