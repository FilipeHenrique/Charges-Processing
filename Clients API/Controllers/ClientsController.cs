using Clients_API.DTO;
using Clients_API.Mappers;
using Domain.Clients.Entities;
using Domain.Clients.Interfaces.UseCases;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Clients_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClientsController : Controller
    {
        private readonly ICreateClientUseCase createClientUseCase;
        private readonly IGetClientUseCase getClientUseCase;
        private readonly IGetAllClientsUseCase getClientsUseCase;
        private readonly ICPFValidationService cpfValidationService;

        public ClientsController(ICreateClientUseCase createClientUseCase, ICPFValidationService cPFValidationService, IGetClientUseCase getClientUseCase, IGetAllClientsUseCase listClientsUseCase)
        {
            this.createClientUseCase = createClientUseCase;
            this.getClientUseCase = getClientUseCase;
            this.getClientsUseCase = listClientsUseCase;
            this.cpfValidationService = cPFValidationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient(CreateClientDTO createClientDTO)
        {
            if (!cpfValidationService.IsCpf(createClientDTO.CPF))
            {
                return BadRequest("Invalid CPF.");
            }

            var formattedCPF = cpfValidationService.CPFToNumericString(createClientDTO.CPF);
            createClientDTO.CPF = formattedCPF;

            var client = await getClientUseCase.GetByCPF(createClientDTO.CPF);

            if (client != null)
            {
                return BadRequest("CPF already exists.");
            }

            var newClient = ClientMapper.ToClient(createClientDTO);
            createClientUseCase.Create(newClient);
            return Created("", newClient);
        }

        [HttpGet("{cpf}")]
        public async Task<IActionResult> GetClient(string cpf)
        {
            if (!cpfValidationService.IsCpf(cpf))
            {
                return BadRequest("Invalid CPF.");
            }

            var formattedCPF = cpfValidationService.CPFToNumericString(cpf);
            var client = await getClientUseCase.GetByCPF(formattedCPF);

            if (client == null)
            {
                return NotFound("Client not found.");
            }

            var newClient = ClientMapper.ToCreateClientDTO(client);
            return Ok(newClient);

        }

        [HttpGet]
        public async Task<IActionResult> ListClients()
        {
            var clientsAsyncEnumerable = getClientsUseCase.GetAll();

            var clientList = new List<Client>();

            await foreach (var client in clientsAsyncEnumerable)
            {
                clientList.Add(client);
            }

            return(Ok(ClientMapper.ListToDTO(clientList)));
        }
    }
}
