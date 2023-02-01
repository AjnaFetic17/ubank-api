using ubank_api.Data;
using ubank_api.Data.Helpers;
using ubank_api.Data.Models.Entities;
using ubank_api.Data.Models.In;
using ubank_api.Data.Models.Out;
using ubank_api.Services.Interfaces;
namespace ubank_api.Services
{
    public class ClientService : IClientService
    {
        private readonly DatabaseContext _context;

        private readonly ICacheService _cacheService;

        public ClientService(DatabaseContext context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }

        public ClientOut? GetClient(Guid id)
        {
            var result = _context.Clients.Where(cli => cli.Id == id && cli.IsDeleted==false).SingleOrDefault();

            if (result != null)
            {
                return new ClientOut(result);
            }

            return null;
        }

        public List<ClientOut>? GetClients()
        {
            var items = new List<Data.Models.Entities.Client>();

            items = _context.Clients.OrderBy(x => x.Id).Where(cli=> cli.IsDeleted == false).ToList();

            if (items != null)
            {
                var listOfClients = items.Select(i=>new ClientOut(i)).ToList();

                _cacheService.AddToCache(CacheKeys.Client, listOfClients);

                return listOfClients;
            }

            return null;
        }

        public ClientOut? CreateClient(ClientIn client)
        {
            if (!_context.Users.Any(u => u.Id == client.Id))
            {
                throw new ArgumentException("User with provided ID is not in database.");
            }

            var cli = new Client(client);
            _context.Add(cli);
            _context.SaveChanges();
            _cacheService.RemoveFromCache(CacheKeys.Client);
            return new ClientOut(cli);
        }

        public ClientOut? UpdateClient(ClientIn client, Guid id)
        {
            var cli = _context.Clients.Where(cli => cli.Id == id && cli.IsDeleted == false).FirstOrDefault();
            if (cli == null) { return null; }

            _context.Entry(cli).CurrentValues.SetValues(client);
            _context.SaveChanges();
            _cacheService.RemoveFromCache(CacheKeys.Client);
            return new ClientOut(cli);
        }

        public bool DeleteClient(Guid id)
        {
            var result = _context.Clients.Where(cli=>cli.Id==id && !cli.IsDeleted).SingleOrDefault();

            if (result != null)
            {
                var temp = result;
                temp.IsDeleted = true;
                _context.Entry(result).CurrentValues.SetValues(temp);
                _context.SaveChanges();
                _cacheService.RemoveFromCache(CacheKeys.Client);

                return true;
            }

            return false;
        }
    }
}
