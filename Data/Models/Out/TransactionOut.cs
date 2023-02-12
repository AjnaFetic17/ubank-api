using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ubank_api.Data.Models.Entities;

namespace ubank_api.Data.Models.Out
{
    public class TransactionOut
    {
        public Guid Id { get; set; }
        public Guid FromAccountId { get; set; }

        public Guid? ToAccountId { get; set; }

        public float Amount { get; set; }

        public DateTime TransactionDate { get; set; }

        public TransactionOut(Transaction transaction)
        {
            FromAccountId = transaction.FromAccountId;
            ToAccountId = transaction.ToAccountId;
            Amount = transaction.Amount;
            TransactionDate = transaction.TransactionDate;
        }
    }
}
