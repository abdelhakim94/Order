using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Order.Client.Components.Misc;
using Order.Client.Constants;
using Order.Client.Extensions;
using Order.Client.Services;
using Order.Shared.Dto.Users;

namespace Order.Client.Pages
{
    public partial class SignIn : ComponentBase
    {
        private bool isLoading { get; set; }
        private string disabled { get => isLoading ? CSSCLasses.PageDisabled : string.Empty; }

        private bool isHidden { get; set; }
        private string hidden { get => isHidden ? CSSCLasses.PageBlured : string.Empty; }

        private Modal resetPasswordModal { get; set; }

        public SignInDto UserSignInData { get; set; } = new SignInDto();

        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [CascadingParameter]
        public NotificationModal NotificationModal { get; set; }

        [CascadingParameter]
        public HttpErrorNotifier HttpErrorNotifier { get; set; }

        public string SocialSpritePath
        {
            get => "/icons/social-media-sprite.png";
        }

        public void ResetPasswordModalShow()
        {
            isHidden = true;
            resetPasswordModal.Show();
        }

        public async Task OnResetPasswordSend()
        {
            isLoading = true;
            string result;
            try
            {
                result = await AuthenticationService.RequestResetPassword(new RequestResetPasswordDto
                {
                    Email = UserSignInData.Email
                });
            }
            catch (System.Exception)
            {
                NotificationModal.ShowError(UIMessages.CannotRequestPwRecover);
                isLoading = false;
                isHidden = false;
                return;
            }

            if (!result.IsHttpClientSuccessful())
            {
                if (result.IsHttpClientError())
                {
                    HttpErrorNotifier.Notify(result);
                }
                else
                {
                    NotificationModal.ShowError(UIMessages.CannotRequestPwRecover);
                }
            }
            else
            {
                NotificationModal.Show(UIMessages.FollowResetPasswordLink);
            }

            await resetPasswordModal.Close();
            isLoading = false;
            isHidden = false;
        }

        public async Task OnPasswordRecoveryCancel()
        {
            await resetPasswordModal.Close();
            isHidden = false;
        }

        public async Task HandleFormSubmition(EditContext context)
        {
            isLoading = true;
            SignInResultDto result;
            try
            {
                result = await AuthenticationService.SignIn(context.Model as SignInDto);
            }
            catch (System.Exception)
            {
                NotificationModal.ShowError(UIMessages.DefaultSignInErrorMessage);
                isLoading = false;
                return;
            }
            isLoading = false;

            if (result.Successful)
            {
                NavigationManager.NavigateTo("/logout/");
            }
            else if (result.IsNotAllowed)
            {
                NotificationModal.ShowError(UIMessages.EmailNotConfirmed);
            }
            else if (result.IsLockedOut)
            {
                NotificationModal.ShowError(UIMessages.AccountLockedOut(result.LockoutEndDate));
            }
            else if (result.IsEmailOrPasswordIncorrect)
            {
                NotificationModal.ShowError(UIMessages.WrongEmailOrPassword);
            }
            else if (result.AdditionalError is not null && result.AdditionalError.IsHttpClientError())
            {
                HttpErrorNotifier.Notify(result.AdditionalError);
            }
            else
            {
                NotificationModal.ShowError(UIMessages.DefaultSignInErrorMessage);
            }
        }
    }
}
