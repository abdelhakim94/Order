using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Order.Application.Client.Services;
using Order.Application.Shared.Dto.Users;

namespace Order.Application.Client.Pages
{
    public partial class SignUp
    {
        public UserSignUpDto SignUpData { get; set; } = new UserSignUpDto();

        [Inject]
        public IAuthenticationService authenticationService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public async Task HandleFormSubmition(EditContext context)
        {
            await authenticationService.SignUp(context.Model as UserSignUpDto);
            NavigationManager.NavigateTo("/");
        }
    }
}
