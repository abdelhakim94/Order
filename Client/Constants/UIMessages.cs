using System;
using Order.Shared.Security.Constants;

namespace Order.Client.Constants
{
    public static class UIMessages
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

        public static string ContinueWith { get => "Continuer avec"; }

        public static string LastName { get => "Nom"; }
        public static string FirstName { get => "Prénom"; }

        public static string EmailAlreadyHasAccount { get => "L'email fourni est déjà associé à un compte. Considérez de vous connecter avec ce compte ou de fournir un autre email"; }
        public static string InvalidEmailAdress { get => "L'email fourni n'est pas valide. Veuillez fournir une adresse email valide"; }
        public static string ServerErrorDuringSignUp { get => "Ouups! Il semble que le serveur n'est pas accessible. Veuillez vérifier votre connexion internet ou réessayer plus tard."; }
        public static string PasswordNotSecure { get => "Le mot de passe ne répond pas aux exigences de sécurité. veuillez envisager de l'allonger ou d'utiliser des caractères spéciaux"; }
        public static string DefaultSignUpErrorMessage { get => "Ouups! Il semble que le compte n'a pas pu être créé. Veuillez réessayer plus tard"; }

        public static string EmailNotConfirmed { get => "Votre email n'a pas été confirmé. Veuillez suivre le lien reçu par email pour confirmer l'adresse"; }
        public static string AccountLockedOut(DateTimeOffset? remainning) => $"Votre compte est bloqué car {UserConstants.MAX_FAILED_SIGNIN} tentatives infructueuses de connexion. Veuillez réessayer dans {remainning.Value.ToLocalTime().Subtract(DateTime.Now).Minutes + 1} minutes";
        public static string WrongEmailOrPassword { get => "Email ou mot de passe incorrect, veuillez réessayer"; }
        public static string DefaultSignInErrorMessage { get => "impossible de vous connecter. Veuillez envisager de créer un compte ou de continuer avec un compte de réseau social"; }
    }
}
