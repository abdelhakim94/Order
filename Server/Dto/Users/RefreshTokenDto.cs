using System;

namespace Order.Server.Dto.Users
{
    public class RefreshTokenDto
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpireAt { get; set; }
    }
}
