using Charges_API.DTO;
using Domain.Contracts.UseCases.Charges;
using Domain.Entities;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Charges_API.Controllers
{
    [Route("charges")]
    [ApiController]
    public class CreateChargeController : Controller
    {
        private readonly ICPFValidationService _cpfValidationService;
        private readonly ICreateChargeUseCase _createChargeUseCase;
        public CreateChargeController(ICPFValidationService cPFValidationService, ICreateChargeUseCase createChargeUseCase) {
            _cpfValidationService = cPFValidationService;
            _createChargeUseCase = createChargeUseCase;
        }
        [HttpPost]
        public IActionResult CreateCharge(CreateChargeDTO createChargeDTO)
        {
            if (!_cpfValidationService.IsCpf(createChargeDTO.ClientCPF))
            {
                return BadRequest("Invalid CPF.");
            }
            string formattedCPF = _cpfValidationService.CPFToNumericString(createChargeDTO.ClientCPF);
            Charge newCharge = new Charge(createChargeDTO.Value, createChargeDTO.DueDate, formattedCPF);
            _createChargeUseCase.CreateCharge(newCharge);
            return Created("", newCharge);
        }
    }
}
