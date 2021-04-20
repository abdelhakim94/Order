using System;

namespace Order.Shared.Dto.Account
{
    public class SignInResultDto
    {
        public bool Successful { get; set; }
        public TokenPairDto TokenPair { get; set; }

        public bool IsNotAllowed { get; set; }
        public bool IsLockedOut { get; set; }
        public DateTimeOffset? LockoutEndDate { get; set; }
        public bool IsEmailOrPasswordIncorrect { get; set; }
    }
}
