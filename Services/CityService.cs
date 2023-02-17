using ubank_api.Data;
using ubank_api.Data.Helpers;
using ubank_api.Data.Models.Entities;
using ubank_api.Data.Models.In;
using ubank_api.Data.Models.Out;
using ubank_api.Services.Interfaces;
namespace ubank_api.Services
{
    public class CityService : ICityService
    {
        private readonly DatabaseContext _context;

        private readonly ICacheService _cacheService;

        public CityService(DatabaseContext context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }

        public CityOut? GetCity(Guid id)
        {
            var result = _context.Cities.Where(c => c.Id == id && c.IsDeleted == false).SingleOrDefault();

            if (result != null)
            {
                return new CityOut(result);
            }

            return null;
        }

        public List<CityOut>? GetCities()
        {
            var items = new List<City>();

            items = _context.Cities.OrderBy(x => x.Id).Where(cli => cli.IsDeleted == false).ToList();

            if (items != null)
            {
                var listOfClients = items.Select(i => new CityOut(i)).ToList();

                _cacheService.AddToCache(CacheKeys.City, listOfClients);

                return listOfClients;
            }

            return null;
        }

        public CityOut? CreateCity(CityIn city)
        {
            var cli = new City(city);
            _context.Add(cli);
            _context.SaveChanges();
            _cacheService.RemoveFromCache(CacheKeys.City);
            return new CityOut(cli);
        }

        public CityOut? UpdateCity(CityIn city, Guid id)
        {
            var cli = _context.Cities.Where(cli => cli.Id == id && cli.IsDeleted == false).FirstOrDefault();
            if (cli == null) { return null; }

            _context.Entry(cli).CurrentValues.SetValues(city);
            _context.SaveChanges();
            _cacheService.RemoveFromCache(CacheKeys.City);
            return new CityOut(cli);
        }

        public bool DeleteCity(Guid id)
        {
            var result = _context.Cities.Where(cli => cli.Id == id && !cli.IsDeleted).SingleOrDefault();

            if (result != null)
            {
                var temp = result;
                temp.IsDeleted = true;
                _context.Entry(result).CurrentValues.SetValues(temp);
                _context.SaveChanges();
                _cacheService.RemoveFromCache(CacheKeys.City);

                return true;
            }

            return false;
        }
    }
}
