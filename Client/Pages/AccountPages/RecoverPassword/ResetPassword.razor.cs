using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Order.Client.Components;
using Order.Client.Components.Misc;
using Order.Client.Constants;
using Order.Client.Services;
using Order.Shared.Dto.Users;

namespace Order.Client.Pages
{
    public partial class ResetPassword : ComponentBase
    {
        private bool isPasswordHidden = true;
        private string passwordRightIcon { get => isPasswordHidden ? "icons/show-password.png" : "icons/hide-password.png"; }
        private void TogglePasswordHide() => isPasswordHidden = !isPasswordHidden;

        private bool isConfirmPasswordHidden = true;
        private string confirmPasswordRightIcon { get => isConfirmPasswordHidden ? "icons/show-password.png" : "icons/hide-password.png"; }
        private void ToggleConfirmPasswordHide() => isConfirmPasswordHidden = !isConfirmPasswordHidden;

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
        public Toast Toast { get; set; }

        [CascadingParameter]
        public Spinner Spinner { get; set; }

        public ResetPasswordDto PasswordReset { get; set; } = new ResetPasswordDto();


        private bool isLoading { get; set; }
        private string disabled { get => isLoading ? "page-disabled" : string.Empty; }

        public async Task HandleFormSubmition(EditContext context)
        {

            isLoading = true;
            Spinner.Show();
            ResetPasswordResultDto result;
            try
            {
                var recoverPwDto = context.Model as ResetPasswordDto;
                recoverPwDto.Email = UserEmail;
                recoverPwDto.ResetToken = ResetPasswordToken;
                recoverPwDto.Trim();

                result = await AuthenticationService.ResetPassword(recoverPwDto, Toast);
                if (result is null) return;
            }
            catch (System.Exception)
            {
                Toast.ShowError(UIMessages.DefaultResetPasswordFailed);
                return;
            }
            finally
            {
                isLoading = false;
                Spinner.Hide();
                StateHasChanged();
            }

            if (result.Successful)
            {
                Toast.ShowSuccess(UIMessages.ResetPasswordsuccess);
                NavigationManager.NavigateTo("SignIn/");
                return;
            }
            else if (result.Error == ErrorDescriber.PasswordTooShort(default(int)).Code
                  || result.Error == ErrorDescriber.PasswordRequiresUniqueChars(default(int)).Code
                  || result.Error == ErrorDescriber.PasswordRequiresNonAlphanumeric().Code
                  || result.Error == ErrorDescriber.PasswordRequiresDigit().Code
                  || result.Error == ErrorDescriber.PasswordRequiresLower().Code
                  || result.Error == ErrorDescriber.PasswordRequiresUpper().Code)
            {
                Toast.ShowError(UIMessages.PasswordNotSecure);
            }
            else if (result.Error == ErrorDescriber.PasswordMismatch().Code)
            {
                Toast.ShowError(UIMessages.PasswordMismatch);
            }
            else if (result.Error == ErrorDescriber.InvalidToken().Code)
            {
                Toast.ShowError(UIMessages.ResetPasswordInvalidToken);
            }
            else
            {
                Toast.ShowError(UIMessages.DefaultResetPasswordFailed);
            }
        }
    }
}
