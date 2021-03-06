using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Order.Shared.Dto.Account
{
    public class ResetPasswordDto
    {
        public string Email { get; set; }
        public string ResetToken { get; set; }

        [Required(ErrorMessage = "Un mot de passe est requis")]
        [PasswordPropertyText(true)]
        [RegularExpression("^(?=.*?[A-Za-z])(?=.*?[0-9]).{8,}$",
            ErrorMessage = "Nécessite au moin 8 caractères dont une lettre et un chiffre")]
        public string Password { get; set; }

        [PasswordPropertyText(true)]
        [Compare("Password", ErrorMessage = "Different du mot de passe")]
        public string ConfirmPassword { get; set; }

        public void Trim()
        {
            Password = Password.Trim();
            ConfirmPassword = ConfirmPassword.Trim();
        }
    }
}
