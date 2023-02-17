using System.ComponentModel.DataAnnotations;
using ubank_api.Data.Models.In;

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
        public City(CityIn cityIn)
        {
            CityName = cityIn.CityName;
            Region = cityIn.Region;
            PostalCode = cityIn.PostalCode;
            Country = cityIn.Country;
        }
    }
}
