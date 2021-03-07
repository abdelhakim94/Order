namespace Order.Server.Dto.Users
{
    public class EmailConfirmationDto
    {
        public string ConfirmationToken { get; set; }
        public string UserEmail { get; set; }
    }
}
