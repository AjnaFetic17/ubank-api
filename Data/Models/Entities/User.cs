using System.ComponentModel.DataAnnotations;
using ubank_api.Data.Helpers.AuthHelpers;

namespace ubank_api.Data.Models.Entities
{
    public enum RoleEnum
    {
        User,
        Admin
    }

    public class User : BaseEntity
    {
        [Required]
        [StringLength(255)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string LastName { get; set; } = string.Empty;

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        [StringLength(maximumLength: 255, ErrorMessage = "Email provided is too long.")]
        public string Email { get; set; } = string.Empty;

        public byte[] PasswordHash { get; set; } = new byte[64];

        public byte[] PasswordSalt { get; set; } = new byte[64];

        public RoleEnum Role { get; set; } = RoleEnum.User;

        public Client? Client { get; set; }
        public User() { }

        public User(UserRegister userIn, byte[] passwordHash, byte[] passwordSalt)
        {
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            Email = userIn.Email;
        }
    }
}
