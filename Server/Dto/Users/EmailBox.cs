namespace Order.Server.Dto.Users
{
    public class EmailBox
    {
        public string Address { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
    }
}
