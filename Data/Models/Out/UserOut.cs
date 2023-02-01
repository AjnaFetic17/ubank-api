using ubank_api.Data.Models.Entities;

namespace ubank_api.Data.Models.Out
{
    public class UserOut 
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public UserOut(User user) { 
            Id= user.Id;
            Email= user.Email;
        }
    }
}
