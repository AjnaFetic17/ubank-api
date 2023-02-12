using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ubank_api.Data.Models.In
{
    public class TransactionIn
    {
        public Guid? Id { get; set; }
        [Required]
        public Guid FromAccountId { get; set; }

        [Required]
        public string ToAccountNumber { get; set; }

        [Required]
        public float Amount { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

    }
}
