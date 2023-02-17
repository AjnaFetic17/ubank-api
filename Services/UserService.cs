using Microsoft.EntityFrameworkCore;
using ubank_api.Data;
using ubank_api.Data.Helpers;
using ubank_api.Data.Helpers.AuthHelpers;
using ubank_api.Data.Models.Entities;
using ubank_api.Data.Models.In;
using ubank_api.Data.Models.Out;
using ubank_api.Services.Interfaces;

namespace ubank_api.Services
{
    public class UserService : IUserService
    {
        private readonly DatabaseContext _context;
        private readonly ICacheService _cacheService;
        private readonly IAuthService _authService;

        public UserService(DatabaseContext context, ICacheService cacheService, IAuthService authService)
        {
            _context = context;
            _cacheService = cacheService;
            _authService = authService;
        }
        public UserOut? GetUser(Guid id)
        {
            var result = _context.Users.Where(cli => cli.Id == id).SingleOrDefault();

            if (result != null)
            {
                return new UserOut(result);
            }

            return null;
        }

        public UserOut? GetUserByEmail(string email)
        {
            var result = _context.Users.Where(user => user.Email == email).SingleOrDefault();

            if (result != null)
            {
                return new UserOut(result);
            }

            return null;
        }

        public List<UserOut>? GetUsers()
        {
            var items = _context.Users.OrderBy(x => x.Id).Select(u => new UserOut(u)).ToList();

            if (items != null)
            {
                _cacheService.AddToCache(CacheKeys.User, items);
                return items;
            }

            return null;
        }

        public bool CreateUser(UserRegister userIn)
        {
            var user = _context.Users.Any(user => user.Email == userIn.Email);

            if (user)
            {
                throw new ArgumentException($"Please provide different Email adress");
            }

            _authService.CreatePasswordHash(userIn.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var added = new User(userIn, passwordHash, passwordSalt);

            _context.Users.Add(added);
            _cacheService.RemoveFromCache(CacheKeys.User);

            _context.SaveChanges();
            return true;
        }

        public UserOut? UpdateUser(UserIn userIn, Guid id)
        {
            var user = _context.Users.Where(u => u.Id == id).FirstOrDefault();
            if (user == null) { return null; }

            if (_context.Users.Any(user => (user.Email == userIn.Email) && user.Id != id))
            {   
                throw new ArgumentException("Please provide different email adress or username.");
            }

            _context.Entry(user).CurrentValues.SetValues(userIn);
            _context.SaveChanges();
            _cacheService.RemoveFromCache(CacheKeys.User);
            return new UserOut(user);
        }

        public bool DeleteUser(Guid id)
        {
            var result = _context.Users.Where(user => user.Id == id).SingleOrDefault();

            if (result != null)
            {
                var cli = _context.Clients.Where(cli=> cli.Id== id).Include(cli=>cli.Accounts).SingleOrDefault();
                if(cli != null)
                {
                    var temp = cli;
                    temp.IsDeleted = true;
                    temp.Accounts?.ForEach(acc => acc.IsDeleted = true);
                    _context.Clients.Entry(cli).CurrentValues.SetValues(temp);
                }
                _context.Users.Remove(result);
                _context.SaveChanges();
                _cacheService.RemoveFromCache(CacheKeys.User);

                return true;
            }

            return false;
        }
    }
}
