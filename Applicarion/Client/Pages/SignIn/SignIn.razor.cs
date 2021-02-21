using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Order.Application.Client.Services;
using Order.Application.Shared.Dto.Users;

namespace Order.Application.Client.Pages
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

        public async Task HandleFormSubmition(EditContext context)
        {
            var result = await authenticationService.SignIn(context.Model as UserSignInDto);
            if (result.Successful)
            {
                NavigationManager.NavigateTo("/logout/");
            }
        }
    }
}
