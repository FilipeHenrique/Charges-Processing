using Charges_API.DTO;
using Domain.Contracts.UseCases.Charges;
using Domain.Entities;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Charges_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChargesController : Controller
    {
        private readonly IListChargesUseCase getChargesUseCase;
        private readonly ICPFValidationService cpfValidationService;
        private readonly ICreateChargeUseCase createChargeUseCase;
        public ChargesController(IListChargesUseCase getChargesUseCase, ICPFValidationService cpfValidationService, ICreateChargeUseCase createChargeUseCase)
        {
            this.getChargesUseCase = getChargesUseCase;
            this.cpfValidationService = cpfValidationService;
            this.createChargeUseCase = createChargeUseCase;
        }

        [HttpPost]
        public IActionResult CreateCharge(CreateChargeDTO createChargeDTO)
        {
            if (!cpfValidationService.IsCpf(createChargeDTO.ClientCPF))
            {
                return BadRequest("Invalid CPF.");
            }
            string formattedCPF = cpfValidationService.CPFToNumericString(createChargeDTO.ClientCPF);
            Charge newCharge = new Charge(createChargeDTO.Value, createChargeDTO.DueDate, formattedCPF);
            createChargeUseCase.CreateCharge(newCharge);
            return Created("", newCharge);
        }

        [HttpGet]
        public async Task<IActionResult> GetCharges(string? cpf = null, int? month = null)
        {
            if (string.IsNullOrWhiteSpace(cpf) && month == null)
            {
                return BadRequest("Atleast one query param is necessary, please specify cpf or dueDate");
            }

            if (!string.IsNullOrWhiteSpace(cpf) && month != null)
            {
                return BadRequest("Only one filter is permitted, choose between cpf or dueDate");
            }

            List<Charge>? charges = null;

            if (!string.IsNullOrWhiteSpace(cpf))
            {
                if (!cpfValidationService.IsCpf(cpf))
                {
                    return BadRequest("Invalid CPF.");
                }
                string formattedCPF = cpfValidationService.CPFToNumericString(cpf);
                charges = await getChargesUseCase.GetChargesByCPF(formattedCPF);
            }

            if (month != null)
            {
                charges = await getChargesUseCase.GetChargesByMonth((int)month);
            }

            return Ok(charges);
        }
    }
}
