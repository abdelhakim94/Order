using System;

namespace Order.Server.Dto.Jwt
{
    public class RefreshTokenDto
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpireAt { get; set; }
    }
}
