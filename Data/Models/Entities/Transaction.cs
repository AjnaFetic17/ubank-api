using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ubank_api.Data.Models.In;

namespace ubank_api.Data.Models.Entities
{
    public class Transaction : BaseEntity
    {
        [ForeignKey("Account")]
        public Guid FromAccountId { get; set; }

        public virtual Account? FromAccount { get; set; }

        [ForeignKey("Account")]
        public Guid? ToAccountId { get; set; }

        public virtual Account? ToAccount { get; set; }

        [Required]
        public float Amount { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        public Transaction() { }

        public Transaction(TransactionIn transactionIn)
        {
            FromAccountId = transactionIn.FromAccountId;
            ToAccountId = transactionIn.ToAccountId;
            Amount = transactionIn.Amount;
            TransactionDate = transactionIn.TransactionDate;
        }
    }
}
