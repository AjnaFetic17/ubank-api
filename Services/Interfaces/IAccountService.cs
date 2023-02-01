using ubank_api.Data.Models.In;
using ubank_api.Data.Models.Out;

namespace ubank_api.Services.Interfaces
{
    public interface IAccountService
    {
        public List<AccountOut>? GetAccounts(Guid clientId);

        public AccountOut? GetAccount(Guid id);

        public AccountOut? CreateAccount(AccountIn accountIn);

        public AccountOut? UpdateAccount(AccountIn client, Guid id);

        public bool DeleteAccount(Guid id);

        public AccountOut? Deposit(Guid id, float amount); //{ balance += amount; }

        public AccountOut? Withdraw(Guid id, float amount); //{ balance -= amount; }
    }
}
