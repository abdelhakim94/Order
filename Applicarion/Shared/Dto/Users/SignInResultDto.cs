namespace Order.Application.Shared.Dto.Users
{
    public class SignInResultDto
    {
        public bool Successful { get; set; }
        public string Error { get; set; }
        public string Token { get; set; }
    }
}
