using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Order.Client.Constants;
using Order.Client.Services;
using Order.Shared.Dto.Users;

namespace Order.Client.Pages
{
    public partial class SignIn
    {
        public UserSignInDto UserSignInData { get; set; } = new UserSignInDto();

        [Inject]
        public IAuthenticationService authenticationService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public string SocialSpritePath
        {
            get => "/icons/social-media-sprite.png";
        }

        private bool isLoading { get; set; }
        private string pageClass
        {
            get => isLoading ? CSSCLasses.PageBlur : "";
        }

        public async Task HandleFormSubmition(EditContext context)
        {
            isLoading = true;
            var result = await authenticationService.SignIn(context.Model as UserSignInDto);
            isLoading = false;
            // improve: use the errors to guide the user with what went wrong
            if (result.Successful)
            {
                NavigationManager.NavigateTo("/logout/");
            }
        }
    }
}
