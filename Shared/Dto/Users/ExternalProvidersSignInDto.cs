namespace Order.Shared.Dto.Users
{
    public class ExternalProviderSignInDto
    {
        public string Provider { get; set; }
        public string ReturnUrl { get; set; }
    }
}
