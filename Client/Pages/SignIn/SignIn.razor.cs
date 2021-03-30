using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Order.Client.Components.Misc;
using Order.Client.Constants;
using Order.Client.Services;
using Order.Shared.Dto;
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
        public Task<AuthenticationState> AuthenticationState { get; set; }

        [Parameter]
        public string AccessToken { get; set; }

        [Parameter]
        public string RefreshToken { get; set; }


        public string SocialSpritePath
        {
            get => "icons/social-media-sprite.png";
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            await base.SetParametersAsync(parameters);
            var state = await AuthenticationState;
            if (
                state.User.Identity.IsAuthenticated &&
                state.User.Claims.Any(c => c.Type == nameof(Profile) && c.Value == nameof(Profile.GUEST)))
            {
                NavigationManager.NavigateTo("home/");
            }

            if (!string.IsNullOrWhiteSpace(AccessToken) && !(string.IsNullOrWhiteSpace(RefreshToken)))
            {
                try
                {
                    await AuthenticationService.MarkUserAsSignedIn(AccessToken, RefreshToken);
                    NavigationManager.NavigateTo("home/");
                }
                catch (System.Exception)
                {
                    NotificationModal.ShowError(UIMessages.CannotSignInWithSocialProvider("la méthode sélectionnée"));
                }
            }
        }

        public async Task HandleSignInFormSubmition(EditContext context)
        {
            isLoading = true;
            SignInResultDto result;

            try
            {
                result = await AuthenticationService.SignIn(context.Model as SignInDto, NotificationModal);
                if (result is null) return;
            }
            catch (System.Exception)
            {
                NotificationModal.ShowError(UIMessages.DefaultSignInErrorMessage);
                return;
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }

            if (result.Successful)
            {
                NavigationManager.NavigateTo("home/");
                return;
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
            bool result;
            try
            {
                result = await AuthenticationService.RequestResetPassword(
                    context.Model as RequestResetPasswordDto,
                    NotificationModal);
                if (!result) return;
            }
            catch (System.Exception)
            {
                NotificationModal.ShowError(UIMessages.CannotRequestResetPassword);
                return;
            }
            finally
            {
                isLoading = false;
                isResetingPassword = false;
                RequestResetPassword.Email = string.Empty;
                await resetPasswordModal.Close();
                StateHasChanged();
            }

            NotificationModal.Show(UIMessages.FollowResetPasswordLink);
        }

        public async Task HandleResetPasswordCanceled()
        {
            await resetPasswordModal.Close();
            isResetingPassword = false;
        }

        // =================================== External identity providers ============================================= //

        public async Task CheckoutConsentScreen(ValueWrapperDto<string> provider)
        {
            isLoading = true;
            ValueWrapperDto<string> result;
            try
            {
                result = await AuthenticationService.GetConsentScreenUrl(provider, NotificationModal);
                if (result is null) return;
            }
            catch (System.Exception)
            {
                NotificationModal.ShowError(UIMessages.CannotSignInWithSocialProvider(provider.Value));
                return;
            }
            finally
            {
                isLoading = false;
            }

            NavigationManager.NavigateTo(result.Value);
        }
    }
}
