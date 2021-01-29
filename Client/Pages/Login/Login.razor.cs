using Order.Shared.Contract.Dto.Users;
using M = Order.Client.Shared.Messages;

namespace Order.Client.Pages
{
    public partial class Login
    {
        /// <summary>
        /// If true, the user is in the Login page. Else, he/she is
        /// in the signup page.
        /// </summary>
        public bool IsLoggingIn { get; set; } = true;

        public string PageHeader { get => IsLoggingIn ? M.SignIn : M.SignUp; }
        public string SubmitButtonLabel { get => IsLoggingIn ? M.Login : M.Register; }
        public string FooterAsk { get => IsLoggingIn ? M.AskForAccountAbsence : M.AskForAccountExistance; }
        public string FooterRedirect { get => IsLoggingIn ? M.SignUpRedirect : M.SignInRedirect; }

        public UserLoginDto LoginData { get; set; } = new UserLoginDto();

        public void ToggleIsLoggingIn() => IsLoggingIn = !IsLoggingIn;
    }
}
