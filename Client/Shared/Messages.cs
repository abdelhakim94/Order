namespace Order.Client.Shared
{
    public static class Messages
    {
        public static string Email { get => "Email"; }
        public static string Password { get => "Mot de passe"; }
        public static string ConfirmPassword { get => "Confirmer mot de passe"; }
        public static string ForgotPassword { get => "Mot de passe oublié?"; }

        public static string SignIn { get => "Connection"; }
        public static string SignUp { get => "Inscription"; }

        public static string Login { get => "Se connecter"; }
        public static string Register { get => "S'inscrire"; }

        public static string AskForAccountExistance { get => "Vous avez déja un compte? "; }
        public static string SignInRedirect { get => "Connectez vous"; }
        public static string AskForAccountAbsence { get => "Vous n'avez pas de compte?"; }
        public static string SignUpRedirect { get => "Rejoignez nous"; }

        public static string Otherwise { get => "Sinon"; }
    }
}