using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Order.Application.Shared.Dto.Users
{
    public class UserSignInDto
    {
        [Required(ErrorMessage = "Une adresse mail est obligatoire")]
        [EmailAddress(ErrorMessage = "Adresse mail invalide")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Un mot de passe est obligatoire")]
        [PasswordPropertyText(true)]
        [RegularExpression("^(?=.*?[A-Za-z])(?=.*?[0-9]).{8,}$",
            ErrorMessage = "Le mot de passe doit contenir au moin 8 caractères, une lettre et un chiffre")]
        public string Password { get; set; }
    }
}
