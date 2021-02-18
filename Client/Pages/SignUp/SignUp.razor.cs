using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Order.Client.Services;
using Order.Shared.Dto.Users;

namespace Order.Client.Pages
{
    public partial class SignUp
    {
        public UserSignUpDto SignUpData { get; set; } = new UserSignUpDto();

        [Inject]
        public IAuthenticationService authenticationService { get; set; }

        public async Task HandleFormSubmition(EditContext context)
        {
            await authenticationService.SignUp(context.Model as UserSignUpDto);
        }
    }
}
