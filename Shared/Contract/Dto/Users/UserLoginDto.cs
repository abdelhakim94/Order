using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Order.Shared.Contract.Dto.Users
{
    public class UserLoginDto
    {
        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Une adresse mail est obligatoire")]
        [EmailAddress(ErrorMessage = "Adresse mail invalide")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Un mot de passe est obligatoire")]
        [PasswordPropertyText(true)]
        [RegularExpression("^(?=.*?[A-Za-z])(?=.*?[0-9]).{8,}$",
            ErrorMessage = "Le mot de passe doit contenir au moin 8 caractères, une lettre et un chiffre")]
        public string Password { get; set; }

        // should have a validator to ensure that it is equal to the password
        [Required]
        public string ConfirmPassword { get; set; }
    }
}