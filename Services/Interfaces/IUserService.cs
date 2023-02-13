using ubank_api.Data.Helpers.AuthHelpers;
using ubank_api.Data.Models.Entities;
using ubank_api.Data.Models.In;
using ubank_api.Data.Models.Out;

namespace ubank_api.Services.Interfaces
{
    public interface IUserService
    {
        public List<UserOut>? GetUsers();

        public UserOut? GetUser(Guid id);

        public UserOut? GetUserByEmail(string email);

        bool CreateUser(UserRegister userIn);

        public UserOut? UpdateUser(UserIn user, Guid id);

        public bool DeleteUser(Guid id);
    }
}
