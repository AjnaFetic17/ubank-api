using ubank_api.Data.Models.In;
using ubank_api.Data.Models.Out;

namespace ubank_api.Services.Interfaces
{
    public interface ITransactionService
    {
        public List<TransactionOut>? GetTransactions(Guid clientId);

        public TransactionOut? GetTransaction(Guid id);

        public TransactionOut? CreateTransaction(TransactionIn transactionIn);

        public bool DeleteTransaction(Guid id);
    }
}
