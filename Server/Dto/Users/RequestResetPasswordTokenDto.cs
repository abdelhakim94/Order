namespace Order.Server.Dto.Users
{
    public class RequestResetPasswordTokenDto
    {
        public string UserEmail { get; set; }
        public string ResetPasswordToken { get; set; }
    }
}
