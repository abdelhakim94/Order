namespace Order.Server.Dto.Users
{
    public class JwtTokenConfigDto
    {
        public string secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessTokenExpiration { get; set; }
        public int RefreshTokenExpiration { get; set; }
    }
}
