using System.ComponentModel.DataAnnotations;
using ubank_api.Data.Models.Entities;

namespace ubank_api.Data.Models.In
{
    public class AccountIn
    {
        public Guid? Id { get; set; }

        [Required]
        public string AccountNumber { get; set; } = string.Empty;

        [Required]
        public string Type { get; set; } = string.Empty;

        [Required]
        public float Balance { get; set; }

        [Required]
        public Guid ClientId { get; set; }
    }
}
