using System.ComponentModel.DataAnnotations;

namespace Order.Shared.Dto.Users
{
    public class RequestResetPasswordDto
    {
        [Required(ErrorMessage = "Une adresse mail est requise")]
        [EmailAddress(ErrorMessage = "Adresse mail invalide")]
        public string Email { get; set; }
    }
}
