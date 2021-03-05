using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Forms;
using Order.Client.Services;
using Order.Shared.Dto.Users;
using Order.Client.Constants;
using Order.Client.Components.Misc;
using Order.Shared.Constants;

namespace Order.Client.Pages
{
    public partial class SignUp : ComponentBase
    {
        private bool isLoading { get; set; }
        private Modal errorModal { get; set; }
        private string errorMessage { get; set; }

        private string disabled
        {
            get => isLoading ? CSSCLasses.PageDisabled : string.Empty;
        }

        public UserSignUpDto SignUpData { get; set; } = new UserSignUpDto();

        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IdentityErrorDescriber ErrorDescriber { get; set; }

        public async Task HandleFormSubmition(EditContext context)
        {
            isLoading = true;
            var result = await AuthenticationService.SignUp(context.Model as UserSignUpDto);
            isLoading = false;

            if (result.Successful)
            {
                NavigationManager.NavigateTo("/");
            }
            else if (result.Error == ErrorDescriber.DuplicateEmail(SignUpData.Email).Code
                  || result.Error == ErrorDescriber.DuplicateUserName(SignUpData.Email).Code
                  || result.Error == ErrorDescriber.LoginAlreadyAssociated().Code)
            {
                errorMessage = UIMessages.EmailAlreadyHasAccount;
            }
            else if (result.Error == ErrorDescriber.InvalidUserName(SignUpData.Email).Code
                  || result.Error == ErrorDescriber.InvalidEmail(SignUpData.Email).Code)
            {
                errorMessage = UIMessages.InvalidEmailAdress;
            }
            else if (result.Error == SignUpErrors.FailureSendingEmail)
            {
                errorMessage = UIMessages.FailureSendingEmail;
            }
            else if (result.Error == SignUpErrors.ServerError)
            {
                errorMessage = UIMessages.ServerUnreachable;
            }
            // this should not be reachable because validation is done on form
            else if (result.Error == ErrorDescriber.PasswordTooShort(default(int)).Code
                  || result.Error == ErrorDescriber.PasswordRequiresUniqueChars(default(int)).Code
                  || result.Error == ErrorDescriber.PasswordRequiresNonAlphanumeric().Code
                  || result.Error == ErrorDescriber.PasswordRequiresDigit().Code
                  || result.Error == ErrorDescriber.PasswordRequiresLower().Code
                  || result.Error == ErrorDescriber.PasswordRequiresUpper().Code)
            {
                errorMessage = UIMessages.PasswordNotSecure;
            }
            else if (result.Error == ErrorDescriber.PasswordMismatch().Code)
            {
                errorMessage = UIMessages.PasswordMismatch;
            }
            else
            {
                errorMessage = UIMessages.DefaultSignUpErrorMessage;
            }

            errorModal.Show();
        }
    }
}
