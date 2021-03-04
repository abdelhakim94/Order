using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
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
        private Modal modalRef { get; set; }
        private string errorMessage { get; set; }

        private string pageClass
        {
            get => isLoading ? CSSCLasses.PageBlur : string.Empty;
        }

        public UserSignInDto UserSignInData { get; set; } = new UserSignInDto();

        [Inject]
        public IAuthenticationService authenticationService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public string SocialSpritePath
        {
            get => "/icons/social-media-sprite.png";
        }

        public async Task HandleFormSubmition(EditContext context)
        {
            isLoading = true;
            var result = await authenticationService.SignIn(context.Model as UserSignInDto);
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

            modalRef.Show();
        }
    }
}
