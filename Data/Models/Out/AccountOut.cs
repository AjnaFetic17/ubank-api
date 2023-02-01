using ubank_api.Data.Models.Entities;

namespace ubank_api.Data.Models.Out
{
    public class AccountOut : Account
    {
        public AccountOut(Account account)
        {
            Id= account.Id;
            AccountNumber= account.AccountNumber;
            Balance= account.Balance;
            ClientId= account.ClientId;
            Type= account.Type;
        }
    }
}
