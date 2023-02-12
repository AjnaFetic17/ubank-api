using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Diagnostics;
using ubank_api.Data;
using ubank_api.Data.Helpers;
using ubank_api.Data.Models.Entities;
using ubank_api.Data.Models.In;
using ubank_api.Data.Models.Out;
using ubank_api.Services.Interfaces;

namespace ubank_api.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly DatabaseContext _context;
        private readonly IAccountService _accountService;

        public TransactionService(DatabaseContext context, IAccountService accountService)
        {
            _context = context;
            _accountService = accountService;
        }

        public List<TransactionOut>? GetTransactions(Guid clientId)
        {
            var result = _context.Transactions.Include(trans => trans.FromAccount).ThenInclude(acc => acc!.Client).
                Include(trans => trans.FromAccount).ThenInclude(acc => acc!.Client)
                .Where(trans => (trans.FromAccount!.Client!.Id == clientId && trans.FromAccount!.IsDeleted == false && trans.FromAccount.Client!.IsDeleted == false)
                || (trans.ToAccount!.Client!.Id == clientId && trans.ToAccount!.IsDeleted == false && trans.ToAccount!.Client!.IsDeleted == false)
                && trans.IsDeleted == false).ToList();

            if (result != null)
            {
                return result.Select(r => new TransactionOut(r)).ToList();
            }

            return null;
        }

        public TransactionOut? GetTransaction(Guid id)
        {
            var result = _context.Transactions.Where(trans => trans.Id == id && trans.IsDeleted == false).SingleOrDefault();

            if (result != null)
            {
                return new TransactionOut(result);
            }

            return null;
        }

        public TransactionOut? CreateTransaction(TransactionIn transactionIn)
        {
            var toAccountId = _context.Accounts.Where(acc => acc.AccountNumber == transactionIn.ToAccountNumber).SingleOrDefault()?.Id;

            if (!_context.Accounts.Any(acc => acc.Id == transactionIn.FromAccountId) || toAccountId == null || (transactionIn.FromAccountId == toAccountId))
            {
                throw new ArgumentException("Accounts with provided ID are not in database.");
            }

            var transaction = new Transaction(transactionIn, (Guid)toAccountId);

            _accountService.Withdraw(transactionIn.FromAccountId, transactionIn.Amount);
            _accountService.Deposit((Guid)toAccountId, transactionIn.Amount);

            _context.Add(transaction);
            _context.SaveChanges();

            return new TransactionOut(transaction);
        }

        public bool DeleteTransaction(Guid id)
        {
            var result = _context.Transactions.Where(trans => trans.Id == id && !trans.IsDeleted).SingleOrDefault();

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
    }
}
