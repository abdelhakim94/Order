using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Order.Shared.Dto.Users
{
    public class SignInDto
    {
        [Required(ErrorMessage = "Une adresse mail est requise")]
        [EmailAddress(ErrorMessage = "Adresse mail invalide")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Un mot de passe est requis")]
        [PasswordPropertyText(true)]
        [RegularExpression("^(?=.*?[A-Za-z])(?=.*?[0-9]).{8,}$",
            ErrorMessage = "Nécessite au moin 8 caractères dont une lettre et un chiffre")]
        public string Password { get; set; }
    }
}
