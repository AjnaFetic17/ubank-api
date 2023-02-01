using System.Text.Json.Serialization;
using ubank_api.Data.Models.Entities;

namespace ubank_api.Data.Models.Out
{
    public class ClientOut : Client
    {
        public string CityName { get; set; } = string.Empty;

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [JsonIgnore]
        new public Guid CityId { get; set; }
        public ClientOut(Client client)
        {
            Id = client.Id;
            FirstName = client.User!.FirstName;
            LastName = client.User!.LastName;
            Address = client.Address;
            CityName = client.City!.CityName;
            Phone = client.Phone;
            Gender = client.Gender;
            Salary = client.Salary;
            DateOfBirth = client.DateOfBirth;
            Email = client.User!.Email;

            if (client.Fax != null)
            {
                Fax = client.Fax;
            }
        }
    }
}
