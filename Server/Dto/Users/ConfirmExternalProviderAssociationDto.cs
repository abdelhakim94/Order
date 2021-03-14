namespace Order.Server.Dto.Users
{
    public class ConfirmExternalProviderAssociationDto
    {
        public string UserEmail { get; set; }
        public string ConfirmationToken { get; set; }
        public string LoginProvider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderKey { get; set; }
    }
}
