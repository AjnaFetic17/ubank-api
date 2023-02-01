using System.Linq;
using ubank_api.Data;
using ubank_api.Data.Helpers;
using ubank_api.Data.Models.Entities;
using ubank_api.Data.Models.In;
using ubank_api.Data.Models.Out;
using ubank_api.Services.Interfaces;

namespace ubank_api.Services
{
    public class AccountService : IAccountService
    {
        private readonly DatabaseContext _context;

        public AccountService(DatabaseContext context)
        {
            _context = context;
        }

        public List<AccountOut>? GetAccounts(Guid clientId)
        {
            var result = _context.Accounts.Where(acc => acc.ClientId == clientId && acc.IsDeleted == false && acc.Client!.IsDeleted == false).ToList();

            if (result != null)
            {
                return result.Select(r => new AccountOut(r)).ToList();
            }

            return null;
        }

        public AccountOut? GetAccount(Guid id)
        {
            var result = _context.Accounts.Where(acc => acc.Id == id && acc.IsDeleted == false).SingleOrDefault();

            if (result != null)
            {
                return new AccountOut(result);
            }

            return null;
        }

        public AccountOut? CreateAccount(AccountIn accountIn)
        {
            if (_context.Accounts.Any(a => a.AccountNumber == accountIn.AccountNumber))
            {
                throw new ArgumentException("Provided account number is not valid.");
            }

            var acc = new Account(accountIn);
            _context.Add(acc);
            _context.SaveChanges();
            return new AccountOut(acc);
        }

        public AccountOut? UpdateAccount(AccountIn accountIn, Guid id)
        {
            if (_context.Accounts.Any(a => a.AccountNumber == accountIn.AccountNumber && accountIn.Id != a.Id))
            {
                throw new ArgumentException("Provided account number is not valid.");
            }

            var acc = _context.Accounts.Where(acc => acc.Id == id && acc.IsDeleted == false).FirstOrDefault();
            if (acc == null) { return null; }

            _context.Entry(acc).CurrentValues.SetValues(accountIn);
            _context.SaveChanges();
            return new AccountOut(acc);
        }

        public bool DeleteAccount(Guid id)
        {
            var result = _context.Accounts.Where(acc => acc.Id == id && !acc.IsDeleted).SingleOrDefault();

            if (result != null)
            {
                var temp = result;
                temp.IsDeleted = true;
                _context.Entry(result).CurrentValues.SetValues(temp);
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        public AccountOut? Deposit(Guid id, float amount)
        {
            var result = _context.Accounts.Where(acc => acc.Id == id && !acc.IsDeleted).SingleOrDefault();

            if (result != null)
            {
                var temp = result;
                temp.Balance += amount;
                _context.Entry(result).CurrentValues.SetValues(temp);
                _context.SaveChanges();

                return new AccountOut(result);
            }

            return null;
        }

        public AccountOut? Withdraw(Guid id, float amount)
        {
            var result = _context.Accounts.Where(acc => acc.Id == id && !acc.IsDeleted).SingleOrDefault();

            if (result != null)
            {
                var temp = result;
                temp.Balance -= amount;
                _context.Entry(result).CurrentValues.SetValues(temp);
                _context.SaveChanges();

                return new AccountOut(result);
            }

            return null;
        }
    }
}
