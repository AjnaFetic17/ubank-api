using System.ComponentModel.DataAnnotations;

namespace ubank_api.Data.Models.In
{
    public class CityIn
    {
        public Guid? Id { get; set; }

        [Required]
        [StringLength(255)]
        public string CityName { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Region { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string PostalCode { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Country { get; set; } = string.Empty;

    }
}
