using System.ComponentModel.DataAnnotations;

namespace ubank_api.Data.Models.In
{
    public class UserIn
    {
        public Guid? Id { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        [StringLength(maximumLength: 255, ErrorMessage = "Email provided is too long.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required")]
        [StringLength(maximumLength: 255, ErrorMessage = "First name provided is too long.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(maximumLength: 255, ErrorMessage = "Last name provided is too long.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username is required")]
        [StringLength(maximumLength: 255, ErrorMessage = "Username provided is too long.")]
        public string Username { get; set; } = string.Empty;
    }
}
