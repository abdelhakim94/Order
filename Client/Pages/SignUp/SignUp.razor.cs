using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Forms;
using Order.Client.Services;
using Order.Shared.Dto.Users;
using Order.Client.Constants;
using Order.Client.Components.Misc;

namespace Order.Client.Pages
{
    public partial class SignUp : ComponentBase
    {
        private bool isLoading { get; set; }
        private string disabled { get => isLoading ? CSSCLasses.PageDisabled : string.Empty; }

        public SignUpDto SignUpData { get; set; } = new SignUpDto();

        [CascadingParameter]
        public NotificationModal NotificationModal { get; set; }

        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IdentityErrorDescriber ErrorDescriber { get; set; }

        public async Task HandleFormSubmition(EditContext context)
        {
            isLoading = true;
            SignUpResultDto result;

            try
            {
                result = await AuthenticationService.SignUp(context.Model as SignUpDto, NotificationModal);
                if (result is null) return;
            }
            catch (System.Exception)
            {
                NotificationModal.ShowError(UIMessages.DefaultSignUpErrorMessage);
                return;
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }

            if (result.Successful)
            {
                NotificationModal.Show(UIMessages.SignUpSuccess);
                NavigationManager.NavigateTo("SignIn/");
                return;
            }
            else if (result.Error == ErrorDescriber.DuplicateEmail(SignUpData.Email).Code
                  || result.Error == ErrorDescriber.DuplicateUserName(SignUpData.Email).Code
                  || result.Error == ErrorDescriber.LoginAlreadyAssociated().Code)
            {
                NotificationModal.ShowError(UIMessages.EmailAlreadyHasAccount);
            }
            else if (result.Error == ErrorDescriber.InvalidUserName(SignUpData.Email).Code
                  || result.Error == ErrorDescriber.InvalidEmail(SignUpData.Email).Code)
            {
                NotificationModal.ShowError(UIMessages.InvalidEmailAdress);
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
                NotificationModal.ShowError(UIMessages.DefaultSignUpErrorMessage);
            }
        }
    }
}
