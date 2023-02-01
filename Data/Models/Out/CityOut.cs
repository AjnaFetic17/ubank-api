using System.ComponentModel.DataAnnotations;
using ubank_api.Data.Models.Entities;

namespace ubank_api.Data.Models.Out
{
    public class CityOut : City
    {
        public CityOut(City city)
        {
            Id = city.Id;
            CityName = city.CityName;
            PostalCode = city.PostalCode;
            Country = city.Country;

            if (city.Region != null)
            {
                Region = city.Region;
            }
        }

    }
}
