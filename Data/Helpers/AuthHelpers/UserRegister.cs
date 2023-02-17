using System.ComponentModel.DataAnnotations;

namespace ubank_api.Data.Helpers.AuthHelpers
{
    public class UserRegister
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(maximumLength: 255, ErrorMessage = "First name provided is too long.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(maximumLength: 255, ErrorMessage = "Last name provided is too long.")]
        public string LastName { get; set; } = string.Empty;

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        [StringLength(maximumLength: 255, ErrorMessage = "Email provided is too long.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$",
         ErrorMessage = "Password must contain at least one lowercase letter, one uppercase letter, one number and one special character and be min 8 characters long and max 32.")]
        public string Password { get; set; } = string.Empty;
    }
}
