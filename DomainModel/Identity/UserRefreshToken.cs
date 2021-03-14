using System;

namespace Order.DomainModel
{
    public class UserRefreshToken
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpireAt { get; set; }

        public virtual User User { get; set; }
    }
}
