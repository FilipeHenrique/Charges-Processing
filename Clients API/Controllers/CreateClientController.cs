using Domain.Contracts.UseCases;
using Domain.Entities;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Clients_API.Controllers
{
    [Route ("clients")]
    [ApiController]
    public class CreateClientController : Controller
    {
        private readonly ICreateClientUseCase _createClientUseCase;

        private readonly ICPFValidationService _cpfValidationService;

        public CreateClientController(ICreateClientUseCase createClientUseCase, ICPFValidationService cPFValidationService)
        {
            _createClientUseCase = createClientUseCase;
            _cpfValidationService = cPFValidationService;
        }

        [HttpPost]
        public IActionResult CreateClient(Client client)
        {
            if (!_cpfValidationService.IsCpf(client.CPF))
            {
                return BadRequest("Invalid CPF.");
            }
            else
            {
                var formattedCPF = _cpfValidationService.CPFToNumericString(client.CPF);
                var newClient = new Client(client.Name, formattedCPF, client.State);
                _createClientUseCase.CreateClient(newClient);
                return Created("", newClient);
            }
        }
    }
}
