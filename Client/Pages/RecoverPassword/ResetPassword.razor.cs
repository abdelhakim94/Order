using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Order.Client.Components.Misc;
using Order.Client.Constants;
using Order.Client.Services;
using Order.Shared.Dto.Users;

namespace Order.Client.Pages
{
    public partial class ResetPassword : ComponentBase
    {
        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IdentityErrorDescriber ErrorDescriber { get; set; }

        [Parameter]
        public string UserEmail { get; set; }

        [Parameter]
        public string ResetPasswordToken { get; set; }

        [CascadingParameter]
        public NotificationModal NotificationModal { get; set; }

        public ResetPasswordDto PasswordReset { get; set; } = new ResetPasswordDto();


        private bool isLoading { get; set; }
        private string disabled { get => isLoading ? "page-disabled" : string.Empty; }

        public async Task HandleFormSubmition(EditContext context)
        {

            isLoading = true;
            ResetPasswordResultDto result;
            try
            {
                var recoverPwDto = context.Model as ResetPasswordDto;
                recoverPwDto.Email = UserEmail;
                recoverPwDto.ResetToken = ResetPasswordToken;

                result = await AuthenticationService.ResetPassword(context.Model as ResetPasswordDto, NotificationModal);
                if (result is null) return;
            }
            catch (System.Exception)
            {
                NotificationModal.ShowError(UIMessages.DefaultResetPasswordFailed);
                return;
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }

            if (result.Successful)
            {
                NotificationModal.Show(UIMessages.ResetPasswordsuccess);
                NavigationManager.NavigateTo("/");
                return;
            }
            else if (result.Error == ErrorDescriber.PasswordTooShort(default(int)).Code
                  || result.Error == ErrorDescriber.PasswordRequiresUniqueChars(default(int)).Code
                  || result.Error == ErrorDescriber.PasswordRequiresNonAlphanumeric().Code
                  || result.Error == ErrorDescriber.PasswordRequiresDigit().Code
                  || result.Error == ErrorDescriber.PasswordRequiresLower().Code
                  || result.Error == ErrorDescriber.PasswordRequiresUpper().Code)
            {
                NotificationModal.ShowError(UIMessages.PasswordNotSecure);
            }
            else if (result.Error == ErrorDescriber.PasswordMismatch().Code)
            {
                NotificationModal.ShowError(UIMessages.PasswordMismatch);
            }
            else
            {
                NotificationModal.ShowError(UIMessages.DefaultResetPasswordFailed);
            }
        }
    }
}
