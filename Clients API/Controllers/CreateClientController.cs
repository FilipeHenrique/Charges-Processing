using Clients_API.DTO;
using Clients_API.Mappers;
using Domain.Contracts.UseCases.Clients;
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

        private readonly IGetClientsUseCase _getClientUseCase;

        private readonly ICPFValidationService _cpfValidationService;

        public CreateClientController(ICreateClientUseCase createClientUseCase, ICPFValidationService cPFValidationService, IGetClientsUseCase getClientUseCase )
        {
            _createClientUseCase = createClientUseCase;
            _cpfValidationService = cPFValidationService;
            _getClientUseCase = getClientUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient(CreateClientDTO createClientDTO)
        {
            if (!_cpfValidationService.IsCpf(createClientDTO.CPF))
            {
                return BadRequest("Invalid CPF.");
            }

            string formattedCPF = _cpfValidationService.CPFToNumericString(createClientDTO.CPF);
            createClientDTO.CPF = formattedCPF;

            Client client = await _getClientUseCase.GetClient(createClientDTO.CPF);

            if (client != null)
            {
                return BadRequest("CPF already exists.");
            }

            Client newClient = ClientMapper.ToClient(createClientDTO);
            _createClientUseCase.CreateClient(newClient);
            return Created("", newClient);
        }
    }
}
