namespace Order.Server.Dto.Users
{
    public class OAuthCredentials
    {
        public Credentials GoogleCredentials { get; set; }
        public Credentials FacebookCredentials { get; set; }
        public Credentials LinkedInCredentials { get; set; }
    }

    public class Credentials
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
