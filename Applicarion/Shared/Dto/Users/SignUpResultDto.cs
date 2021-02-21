using System.Collections.Generic;

namespace Order.Application.Shared.Dto.Users
{
    public class SignUpResultDto
    {
        public bool Successful { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
