using Domain.Contracts.UseCases;
using Domain.Entities;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Clients_API.Controllers
{
    [Route("clients/{cpf}")]
    [ApiController]
    public class GetClientController : Controller
    {
        private readonly IGetClientUseCase _getClientUseCase;

        private readonly ICPFValidationService _cpfValidationService;

        public GetClientController(IGetClientUseCase getClientUseCase, ICPFValidationService cPFValidationService)
        {
            _getClientUseCase = getClientUseCase;
            _cpfValidationService = cPFValidationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetClient(string cpf)
        {
            if (!_cpfValidationService.IsCpf(cpf))
            {
                return BadRequest("Invalid CPF.");
            }

            string formattedCPF = _cpfValidationService.CPFToNumericString(cpf);
            Client client = await _getClientUseCase.GetClient(formattedCPF);

            if (client == null)
            {
                return NotFound("Client not found.");
            }

            return Ok(client);

        }
    }
}

