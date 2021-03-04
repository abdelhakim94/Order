namespace Order.Server.Dto.Users
{
    public class EmailConfirmationDto
    {
        public string confirmationToken { get; set; }
        public string userEmail { get; set; }
    }
}
