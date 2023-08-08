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
        public async Task<ActionResult<Client>> GetClient(string cpf)
        {
            if (!_cpfValidationService.IsCpf(cpf))
            {
                return BadRequest("Invalid CPF.");
            }
            else
            {
                var formattedCPF = _cpfValidationService.CPFToNumericString(cpf);
                var client =  await _getClientUseCase.GetClient(formattedCPF);
                return Ok(client);
            }
        }
    }
}

