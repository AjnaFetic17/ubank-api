using System.ComponentModel.DataAnnotations;
using ubank_api.Data.Models.In;

namespace ubank_api.Data.Models.Entities
{
    public class Account : BaseEntity
    {
        [Required]
        public string AccountNumber { get; set; } = string.Empty;

        [Required]
        public string Type { get; set; } = string.Empty;

        [Required]
        public float Balance { get; set; }

        [Required]
        public Guid ClientId { get; set; }

        public Client? Client { get; set; }

        public Account() { }

        public Account(AccountIn accountIn)
        {
            AccountNumber = accountIn.AccountNumber;
            Type = accountIn.Type;
            Balance = accountIn.Balance;
            ClientId= accountIn.ClientId;
        }
    }
}
