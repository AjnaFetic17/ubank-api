using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ubank_api.Data.Models.Entities;

namespace ubank_api.Data.Models.In
{
    public class ClientIn
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(255)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Address { get; set; } = string.Empty;

        [Required]
        public Guid CityId { get; set; }

        [Required]
        [Phone]
        [StringLength(15)]
        public string Phone { get; set; } = string.Empty;

        [Phone]
        [StringLength(15)]
        public string? Fax { get; set; } = string.Empty;

        [Required]
        public bool Gender { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public double Salary { get; set; }

        public ClientIn() { }
    }
}
