using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Order.Client.Components;
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

        private bool isPasswordHidden = true;
        private string rightIcon { get => isPasswordHidden ? "icons/show-password.png" : "icons/hide-password.png"; }
        private void TogglePasswordHide() => isPasswordHidden = !isPasswordHidden;

        public SignInDto UserSignInData { get; set; } = new SignInDto();
        public RequestResetPasswordDto RequestResetPassword { get; set; } = new RequestResetPasswordDto();

        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [CascadingParameter]
        public Toast Toast { get; set; }

        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationState { get; set; }

        [CascadingParameter]
        public Spinner Spinner { get; set; }

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
                NavigationManager.NavigateTo("search/");
                return;
            }

            if (!string.IsNullOrWhiteSpace(AccessToken) && !(string.IsNullOrWhiteSpace(RefreshToken)))
            {
                try
                {
                    await AuthenticationService.MarkUserAsSignedIn(AccessToken, RefreshToken);
                    NavigationManager.NavigateTo("search/");
                }
                catch (System.Exception)
                {
                    Toast.ShowError(UIMessages.CannotSignInWithSocialProvider("la méthode sélectionnée"));
                }
            }
        }

        public async Task HandleSignInFormSubmition(EditContext context)
        {
            isLoading = true;
            Spinner.Show();
            SignInResultDto result;

            try
            {
                var userInfo = context.Model as SignInDto;
                userInfo.Trim();
                result = await AuthenticationService.SignIn(context.Model as SignInDto, Toast);
                if (result is null) return;
            }
            catch (System.Exception)
            {
                Toast.ShowError(UIMessages.DefaultSignInErrorMessage);
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
                NavigationManager.NavigateTo("search/");
                return;
            }
            else if (result.IsNotAllowed)
            {
                Toast.ShowError(UIMessages.EmailNotConfirmed);
            }
            else if (result.IsLockedOut)
            {
                Toast.ShowError(UIMessages.AccountLockedOut(result.LockoutEndDate));
            }
            else if (result.IsEmailOrPasswordIncorrect)
            {
                Toast.ShowError(UIMessages.WrongEmailOrPassword);
            }
            else
            {
                Toast.ShowError(UIMessages.DefaultSignInErrorMessage);
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
            Spinner.Show();
            bool result;
            try
            {
                var info = context.Model as RequestResetPasswordDto;
                info.Trim();

                result = await AuthenticationService.RequestResetPassword(
                    info,
                    Toast);
                if (!result) return;
            }
            catch (System.Exception)
            {
                Toast.ShowError(UIMessages.CannotRequestResetPassword);
                return;
            }
            finally
            {
                isLoading = false;
                Spinner.Hide();
                isResetingPassword = false;
                RequestResetPassword.Email = string.Empty;
                await resetPasswordModal.Close();
                StateHasChanged();
            }

            Toast.ShowSuccess(UIMessages.FollowResetPasswordLink);
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
            Spinner.Show();
            ValueWrapperDto<string> result;
            try
            {
                result = await AuthenticationService.GetConsentScreenUrl(provider, Toast);
                if (result is null) return;
            }
            catch (System.Exception)
            {
                Toast.ShowError(UIMessages.CannotSignInWithSocialProvider(provider.Value));
                return;
            }
            finally
            {
                isLoading = false;
                Spinner.Hide();
            }

            NavigationManager.NavigateTo(result.Value);
        }
    }
}
