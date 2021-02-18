using System.Collections.Generic;

namespace Order.Shared.Dto.Users
{
    public class SignUpResultDto
    {
        public bool Successful { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
