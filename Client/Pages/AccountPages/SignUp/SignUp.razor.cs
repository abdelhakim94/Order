using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Forms;
using Order.Client.Services;
using Order.Shared.Dto.Users;
using Order.Client.Constants;
using Order.Client.Components.Misc;
using Order.Client.Components;

namespace Order.Client.Pages
{
    public partial class SignUp : ComponentBase
    {
        private bool isLoading { get; set; }
        private string disabled { get => isLoading ? CSSCLasses.PageDisabled : string.Empty; }

        private bool isPasswordHidden = true;
        private string passwordRightIcon { get => isPasswordHidden ? "icons/show-password.png" : "icons/hide-password.png"; }
        private void TogglePasswordHide() => isPasswordHidden = !isPasswordHidden;

        private bool isConfirmPasswordHidden = true;
        private string confirmPasswordRightIcon { get => isConfirmPasswordHidden ? "icons/show-password.png" : "icons/hide-password.png"; }
        private void ToggleConfirmPasswordHide() => isConfirmPasswordHidden = !isConfirmPasswordHidden;

        public SignUpDto SignUpData { get; set; } = new SignUpDto();

        [CascadingParameter]
        public Toast Toast { get; set; }

        [CascadingParameter]
        public Spinner Spinner { get; set; }

        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IdentityErrorDescriber ErrorDescriber { get; set; }

        public async Task HandleFormSubmition(EditContext context)
        {
            isLoading = true;
            Spinner.Show();
            SignUpResultDto result;

            try
            {
                var userInfo = context.Model as SignUpDto;
                userInfo.Trim();
                result = await AuthenticationService.SignUp(context.Model as SignUpDto, Toast);
                if (result is null) return;
            }
            catch (System.Exception)
            {
                Toast.ShowError(UIMessages.DefaultSignUpErrorMessage);
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
                Toast.ShowSuccess(UIMessages.SignUpSuccess);
                NavigationManager.NavigateTo("SignIn/");
                return;
            }
            else if (result.Error == ErrorDescriber.DuplicateEmail(SignUpData.Email).Code
                  || result.Error == ErrorDescriber.DuplicateUserName(SignUpData.Email).Code
                  || result.Error == ErrorDescriber.LoginAlreadyAssociated().Code)
            {
                Toast.ShowError(UIMessages.EmailAlreadyHasAccount);
            }
            else if (result.Error == ErrorDescriber.InvalidUserName(SignUpData.Email).Code
                  || result.Error == ErrorDescriber.InvalidEmail(SignUpData.Email).Code)
            {
                Toast.ShowError(UIMessages.InvalidEmailAdress);
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
            else
            {
                Toast.ShowError(UIMessages.DefaultSignUpErrorMessage);
            }
        }
    }
}
