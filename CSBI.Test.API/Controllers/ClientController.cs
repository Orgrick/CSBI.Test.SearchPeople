using AutoMapper;
using CSBI.Test.Abstractions.Services;
using CSBI.Test.API.Dto;
using CSBI.Test.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSBI.Test.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;

        public ClientController(
            IClientService clientService,
            IMapper mapper
            )
        {
            _clientService = clientService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClient(int id)
        {
            var client = await _clientService.GetClientAsync(id);

            var res = _mapper.Map<ClientDto>(client);

            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ChangeClient(int id)
        {
            await _clientService.DeleteClientAsync(id);

            return Ok();
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateClient(ClientDto req)
        {
            var id = await _clientService.CreateClientAsync(req.Name, req.Surname, req.Patronymic);

            return Ok(id);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetClients()
        {
            var clients = await _clientService.GetClientsAsync();

            return Ok(_mapper.Map<IEnumerable<ClientDto>>(clients));
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchClients(string name, string surname, string patronymic)
        {
            var clients = await _clientService.SearchClientsAsync(name, surname, patronymic);

            var res = _mapper.Map<IEnumerable<ClientDto>>(clients);

            return Ok(res);
        }
    }
}
