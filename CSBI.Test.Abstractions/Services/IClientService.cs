using CSBI.Test.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSBI.Test.Abstractions.Services
{
    public interface IClientService
    {
        Task<IEnumerable<Client>> SearchClientsAsync(string name, string surname, string patronymic);

        Task<IEnumerable<Client>> GetClientsAsync();

        Task<Client> GetClientAsync(int id);

        Task<int> CreateClientAsync(string name, string surname, string patronymic);

        Task ChangeClientAsync(int id, string name, string surname, string patronymic);

        Task DeleteClientAsync(int id);
    }
}
