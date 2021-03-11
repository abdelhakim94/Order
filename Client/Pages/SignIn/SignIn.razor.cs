using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Order.Client.Components.Misc;
using Order.Client.Constants;
using Order.Client.Extensions;
using Order.Client.Services;
using Order.Shared.Dto.Users;
using Order.Shared.Security.Claims;

namespace Order.Client.Pages
{
    public partial class SignIn : ComponentBase
    {
        private bool isLoading { get; set; }
        private string disabledPage { get => isLoading ? CSSCLasses.PageDisabled : string.Empty; }

        private bool isResetingPassword { get; set; }
        private string bluredPage { get => isResetingPassword ? CSSCLasses.PageBlured : string.Empty; }

        private Modal resetPasswordModal { get; set; }

        public SignInDto UserSignInData { get; set; } = new SignInDto();
        public RequestResetPasswordDto RequestResetPassword { get; set; } = new RequestResetPasswordDto();

        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [CascadingParameter]
        public NotificationModal NotificationModal { get; set; }

        [CascadingParameter]
        public HttpErrorNotifier HttpErrorNotifier { get; set; }

        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationState { get; set; }

        public string SocialSpritePath
        {
            get => "/icons/social-media-sprite.png";
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            await base.SetParametersAsync(parameters);
            var state = await AuthenticationState;
            if (
                state.User.Identity.IsAuthenticated &&
                state.User.Claims.Any(c => c.Type == nameof(Profile) && c.Value == nameof(Profile.GUEST)))
            {
                NavigationManager.NavigateTo("/home");
            }

        }

        public async Task HandleSignInFormSubmition(EditContext context)
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
                NavigationManager.NavigateTo("/home/");
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

        // ==================================== Reset password ========================================== //

        public void ResetPasswordModalShow()
        {
            isResetingPassword = true;
            resetPasswordModal.Show();
        }

        public async Task HandleResetPasswordFormSubmit(EditContext context)
        {
            isLoading = true;
            string result;
            try
            {
                result = await AuthenticationService.RequestResetPassword(context.Model as RequestResetPasswordDto);
            }
            catch (System.Exception)
            {
                NotificationModal.ShowError(UIMessages.CannotRequestPwRecover);
                isLoading = false;
                isResetingPassword = false;
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
            isResetingPassword = false;
        }

        public async Task HandleResetPasswordCanceled()
        {
            await resetPasswordModal.Close();
            isResetingPassword = false;
        }
    }
}
