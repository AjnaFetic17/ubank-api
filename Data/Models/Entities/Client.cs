using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ubank_api.Data.Helpers.AuthHelpers;
using ubank_api.Data.Models.In;

namespace ubank_api.Data.Models.Entities
{
    public class Client : BaseEntity
    {
        [Key]
        [ForeignKey("User")]
        new public Guid Id { get; set; }

        public User? User { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; } = string.Empty;

        [ForeignKey("City")]
        public Guid CityId { get; set; }

        public virtual City? City { get; set; }

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

        public virtual List<Account>? Accounts { get; set; }

        public Client() { }

        public Client(ClientIn clientIn)
        {
            Id = clientIn.Id;
            Address= clientIn.Address;
            CityId = clientIn.CityId;
            Phone = clientIn.Phone;
            Fax = clientIn.Fax;
            Gender = clientIn.Gender;
            DateOfBirth= clientIn.DateOfBirth;
            Salary = clientIn.Salary;
        }

    }
}
