using ubank_api.Data.Models.In;
using ubank_api.Data.Models.Out;

namespace ubank_api.Services.Interfaces
{
    public interface ICityService
    {
        public List<CityOut>? GetCities();

        public CityOut? GetCity(Guid id);

        public CityOut? CreateCity(CityIn city);

        public CityOut? UpdateCity(CityIn city, Guid id);

        public bool DeleteCity(Guid id);
    }
}
