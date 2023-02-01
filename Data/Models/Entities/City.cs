using System.ComponentModel.DataAnnotations;

namespace ubank_api.Data.Models.Entities
{
    public class City : BaseEntity
    {
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

        public City() { }
    }
}
