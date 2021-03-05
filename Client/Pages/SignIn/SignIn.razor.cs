using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Order.Client.Components.Misc;
using Order.Client.Constants;
using Order.Client.Services;
using Order.Shared.Dto.Users;

namespace Order.Client.Pages
{
    public partial class SignIn : ComponentBase
    {
        private bool isLoading { get; set; }
        private bool isHidden { get; set; }
        private Modal errorModal { get; set; }
        private Modal pwRecoveryModal { get; set; }
        private string errorMessage { get; set; }

        private string disabled
        {
            get => isLoading ? CSSCLasses.PageDisabled : string.Empty;
        }

        private string hidden
        {
            get => isHidden ? CSSCLasses.PageBlured : string.Empty;
        }

        public UserSignInDto UserSignInData { get; set; } = new UserSignInDto();

        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public string SocialSpritePath
        {
            get => "/icons/social-media-sprite.png";
        }

        public void PasswordRecoveryModalShow()
        {
            isHidden = true;
            pwRecoveryModal.Show();
        }

        public async Task OnPasswordRecoverySend()
        {
            var result = await AuthenticationService.RequestRecoverPassword(UserSignInData.Email);
            if (!result)
            {
                errorMessage = UIMessages.ServerUnreachable;
                errorModal.Show();
            }
            await pwRecoveryModal.Close();
            isHidden = false;
        }

        public async Task OnPasswordRecoveryCancel()
        {
            await pwRecoveryModal.Close();
            isHidden = false;
        }

        public async Task HandleFormSubmition(EditContext context)
        {
            isLoading = true;
            var result = await AuthenticationService.SignIn(context.Model as UserSignInDto);
            isLoading = false;

            if (result.Successful)
            {
                NavigationManager.NavigateTo("/logout/");
            }
            else if (result.IsNotAllowed)
            {
                errorMessage = UIMessages.EmailNotConfirmed;
            }
            else if (result.IsLockedOut)
            {
                errorMessage = UIMessages.AccountLockedOut(result.LockoutEndDate);
            }
            else if (result.IsEmailOrPasswordIncorrect)
            {
                errorMessage = UIMessages.WrongEmailOrPassword;
            }
            else
            {
                errorMessage = UIMessages.DefaultSignInErrorMessage;
            }

            errorModal.Show();
        }
    }
}
