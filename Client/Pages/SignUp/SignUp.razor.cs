using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Order.Client.Services;
using Order.Shared.Dto.Users;
using Order.Client.Constants;

namespace Order.Client.Pages
{
    public partial class SignUp : ComponentBase
    {
        private bool isLoading { get; set; }
        private string pageClass
        {
            get => isLoading ? CSSCLasses.PageBlur : string.Empty;
        }

        public UserSignUpDto SignUpData { get; set; } = new UserSignUpDto();

        [Inject]
        public IAuthenticationService authenticationService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public async Task HandleFormSubmition(EditContext context)
        {
            isLoading = true;
            var result = await authenticationService.SignUp(context.Model as UserSignUpDto);
            isLoading = false;
            // use the error codes to handle errors.
            NavigationManager.NavigateTo("/");
        }
    }
}
