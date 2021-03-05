using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Order.Client.Components.Misc;
using Order.Client.Constants;
using Order.Client.Services;
using Order.Shared.Constants;
using Order.Shared.Dto.Users;

namespace Order.Client.Pages
{
    public partial class RecoverPassword : ComponentBase
    {
        private Modal errorModal { get; set; }
        private string errorMessage { get; set; }
        private bool isLoading { get; set; }

        private string disabled
        {
            get => isLoading ? "page-disabled" : string.Empty;
        }

        public RecoverPasswordDto PasswordReset { get; set; } = new RecoverPasswordDto();

        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public async Task HandleFormSubmition(EditContext context)
        {
            isLoading = true;
            var result = await AuthenticationService.RecoverPassword(context.Model as RecoverPasswordDto);
            isLoading = false;

            if (result.Successful)
            {
                NavigationManager.NavigateTo("/");
            }
            else if (result.Error == SignUpErrors.ServerError)
            {
                errorMessage = UIMessages.ServerUnreachable;
            }
            else
            {
                errorMessage = UIMessages.DefaultSignUpErrorMessage;
            }

            errorModal.Show();
        }
    }
}
