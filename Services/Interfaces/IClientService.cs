using ubank_api.Data.Models.In;
using ubank_api.Data.Models.Out;

namespace ubank_api.Services.Interfaces
{
    public interface IClientService
    {
        public List<ClientOut>? GetClients();

        public ClientOut? GetClient(Guid id);

        public ClientOut? CreateClient(ClientIn client);

        public ClientOut? UpdateClient(ClientIn client, Guid id);

        public bool DeleteClient(Guid id);
    }
}
