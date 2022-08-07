using CSBI.Test.Abstractions.Services;
using CSBI.Test.Application.Exceptions;
using CSBI.Test.DataAccess.EF;
using CSBI.Test.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSBI.Test.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly ClientsContext _context;

        public ClientService(ClientsContext context)
            => _context = context;

        public async Task ChangeClientAsync(int id, string name, string surname, string patronymic)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(x => x.Id == id);

            if (client is null)
            {
                throw new DomainException("По указанному идентификатору клиент не найден");
            }

            client.Name = name;
            client.Surname = surname;
            client.Patronymic = patronymic;

            await _context.SaveChangesAsync();
        }

        public async Task<int> CreateClientAsync(string name, string surname, string patronymic)
        {
            var client = new Client()
            {
                Name = name.Trim(),
                Surname = surname.Trim(),
                Patronymic = patronymic.Trim()
            };

            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();

            return client.Id;
        }

        public async Task DeleteClientAsync(int id)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(x => x.Id == id);

            if (client is null)
            {
                throw new DomainException("По указанному идентификатору клиент не найден");
            }

            _context.Clients.Remove(client);

            await _context.SaveChangesAsync();
        }

        public async Task<Client> GetClientAsync(int id)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(x => x.Id == id) ;

            if (client is null)
            {
                throw new DomainException("По указанному идентификатору клиент не найден");
            }

            return client;
        }

        public async Task<IEnumerable<Client>> GetClientsAsync() => await _context.Clients.ToListAsync();

        public async Task<IEnumerable<Client>> SearchClientsAsync(string name, string surname, string patronymic)
        {
            var clients = _context.Clients.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                // сделано так потому что по умолчанию StartWith учитывает регистр, а через ToLower и StartWith без учета регистра не транслируется в SQL
                clients = clients.Where(x => EF.Functions.Like(x.Name, $"{name}%"));
            }

            if (!string.IsNullOrWhiteSpace(surname))
            {
                clients = clients.Where(x => EF.Functions.Like(x.Surname, $"{surname}%"));
            }

            if (!string.IsNullOrWhiteSpace(patronymic))
            {
                clients = clients.Where(x => EF.Functions.Like(x.Patronymic, $"{patronymic}%"));
            }

            var foundСlients = await clients.ToListAsync();

            return foundСlients;
        }
    }
}
