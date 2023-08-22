using Clients_API.DTO;
using Clients_API.Mappers;
using Domain.Contracts.UseCases.Clients;
using Domain.Entities;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Clients_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClientsController : Controller
    {
        private readonly ICreateClientUseCase createClientUseCase;

        private readonly IGetClientUseCase getClientUseCase;

        private readonly IListClientsUseCase listClientsUseCase;

        private readonly ICPFValidationService cpfValidationService;

        public ClientsController(ICreateClientUseCase createClientUseCase, ICPFValidationService cPFValidationService, IGetClientUseCase getClientUseCase, IListClientsUseCase listClientsUseCase)
        {
            this.createClientUseCase = createClientUseCase;
            this.getClientUseCase = getClientUseCase;
            this.listClientsUseCase = listClientsUseCase;
            this.cpfValidationService = cPFValidationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient(CreateClientDTO createClientDTO)
        {
            if (!cpfValidationService.IsCpf(createClientDTO.CPF))
            {
                return BadRequest("Invalid CPF.");
            }

            string formattedCPF = cpfValidationService.CPFToNumericString(createClientDTO.CPF);
            createClientDTO.CPF = formattedCPF;

            Client client = await getClientUseCase.GetClient(createClientDTO.CPF);

            if (client != null)
            {
                return BadRequest("CPF already exists.");
            }

            Client newClient = ClientMapper.ToClient(createClientDTO);
            createClientUseCase.CreateClient(newClient);
            return Created("", newClient);
        }

        [HttpGet("{cpf}")]
        public async Task<IActionResult> GetClient(string cpf)
        {
            if (!cpfValidationService.IsCpf(cpf))
            {
                return BadRequest("Invalid CPF.");
            }

            string formattedCPF = cpfValidationService.CPFToNumericString(cpf);
            Client client = await getClientUseCase.GetClient(formattedCPF);

            if (client == null)
            {
                return NotFound("Client not found.");
            }

            return Ok(client);

        }
        [HttpGet]
        public async Task<IActionResult> ListClients()
        {
            List<Client> clients = await listClientsUseCase.ListClients();
            return Ok(clients);
        }
    }
}
