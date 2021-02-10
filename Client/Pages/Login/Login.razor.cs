using System.Net.Http;
using System.Threading.Tasks;
using MessagePack;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Order.Shared.Dto.Users;
using M = Order.Client.Pages.Messages;

namespace Order.Client.Pages
{
    public partial class Login
    {
        /// <summary>
        /// If true, the user is in the Login page. Else, he/she is
        /// in the signup page.
        /// </summary>
        public bool IsLoggingIn { get; set; } = true;

        public UserLoginDto LoginData { get; set; } = new UserLoginDto();

        public string PageHeader
        {
            get => IsLoggingIn ? M.SignIn : M.SignUp;
        }

        public string SubmitButtonLabel
        {
            get => IsLoggingIn ? M.Login : M.Register;
        }

        public string FooterAsk
        {
            get => IsLoggingIn ? M.AskForAccountAbsence : M.AskForAccountExistance;
        }

        public string FooterRedirect
        {
            get => IsLoggingIn ? M.SignUpRedirect : M.SignInRedirect;
        }

        public string SocialSpritePath
        {
            get => "/icons/social-media-sprite.png";
        }

        public void ToggleIsLoggingIn() => IsLoggingIn = !IsLoggingIn;

        // Trying Hub connections

        // public string Foo { get; set; } = "No data yet";
        // public async Task OnClick()
        // {
        //     Foo = (await hubConnection.InvokeAsync<ComplexType>("SendMessage", "user", "data")).Data;
        // }

        // [Inject]
        // public NavigationManager NavigationManager { get; set; }
        // private HubConnection hubConnection;

        // protected override async Task OnInitializedAsync()
        // {
        //     hubConnection = new HubConnectionBuilder()
        //         .WithUrl(NavigationManager.ToAbsoluteUri("/Foo"))
        //         .AddMessagePackProtocol()
        //         .Build();

        //     hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
        //     {
        //         Foo = message;
        //         StateHasChanged();
        //     });
        //     await hubConnection.StartAsync();
        // }

        // public async ValueTask DisposeAsync()
        // {
        //     await hubConnection.DisposeAsync();
        // }
    }
}
