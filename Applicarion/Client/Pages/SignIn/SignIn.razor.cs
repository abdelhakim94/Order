using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Order.Application.Client.Services;
using Order.Application.Shared.Dto.Users;

namespace Order.Application.Client.Pages
{
    public partial class SignIn
    {
        private readonly DiscoveryDocumentResponse discoveryDocument;
        public UserSignInDto UserSignInData { get; set; } = new UserSignInDto();

        [Inject]
        public IAuthenticationService authenticationService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public string SocialSpritePath
        {
            get => "/icons/social-media-sprite.png";
        }

        protected override async Task OnInitializedAsync()
        {
            authenticationService.
        }

        public async Task HandleFormSubmition(EditContext context)
        {
            var result = await authenticationService.SignIn(context.Model as UserSignInDto, null);
            if (result.Successful)
            {
                NavigationManager.NavigateTo("/logout/");
            }
        }
    }
}
