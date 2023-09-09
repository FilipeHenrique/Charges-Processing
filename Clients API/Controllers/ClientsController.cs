using Clients_API.DTO;
using Clients_API.Mappers;
using Domain.Clients.Entities;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Clients_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClientsController : Controller
    {
        private readonly ICPFHandler cpfHandler;
        private readonly IRepository<Client> repository;

        public ClientsController(ICPFHandler cpfHandler, IRepository<Client> repository)
        {
            this.cpfHandler = cpfHandler;
            this.repository = repository;
        }

        [HttpPost]
        public IActionResult CreateClient(ClientDTO clientDTO)
        {
            if (!cpfHandler.IsCpf(clientDTO.CPF))
            {
                return BadRequest("Invalid CPF.");
            }

            var formattedCPF = cpfHandler.CPFToNumericString(clientDTO.CPF);
            clientDTO.CPF = formattedCPF;

            var client = repository.Get().Where(client => client.CPF == clientDTO.CPF);

            if (client != null)
            {
                return BadRequest("CPF already exists.");
            }

            var newClient = ClientMapper.ToClient(clientDTO);
            repository.Create(newClient);
            return Created("", newClient);
        }

        [HttpGet("{cpf}")]
        public ActionResult<Client> GetClient(string cpf)
        {
            if (!cpfHandler.IsCpf(cpf))
            {
                return BadRequest("Invalid CPF.");
            }

            var formattedCPF = cpfHandler.CPFToNumericString(cpf);
            var client = repository.Get().FirstOrDefault(client => client.CPF == formattedCPF);

            if (client == null)
            {
                return NotFound("Client not found.");
            }

            var newClient = ClientMapper.ToClientDTO(client);

            return Ok(newClient);

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clientsAsyncEnumerable = repository.GetAll();

            var clientList = new List<Client>();

            await foreach (var client in clientsAsyncEnumerable)
            {
                clientList.Add(client);
            }

            return(Ok(ClientMapper.ListToDTO(clientList)));
        }
    }
}
