using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Order.Shared.Dto.Users
{
    public class UserSignUpDto
    {
        [Required(ErrorMessage = "Un nom est requis")]
        [RegularExpression("^([A-Za-z]).{2,}", ErrorMessage = "Nécessite au moin 3 caractères commençant par une lettre")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Un prénom est requis")]
        [RegularExpression("^([A-Za-z]).{2,}", ErrorMessage = "Nécessite au moin 3 caractères commençant par une lettre")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Une adresse mail est requise")]
        [EmailAddress(ErrorMessage = "Adresse mail invalide")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Un mot de passe est requis")]
        [PasswordPropertyText(true)]
        [RegularExpression("^(?=.*?[A-Za-z])(?=.*?[0-9]).{8,}$",
            ErrorMessage = "Nécessite au moin 8 caractères dont une lettre et un chiffre")]
        public string Password { get; set; }

        // should have a validator to ensure that it is equal to the password
        [Required(ErrorMessage = "Un mot de passe est requis")]
        [PasswordPropertyText(true)]
        public string ConfirmPassword { get; set; }
    }
}
