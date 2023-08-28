using Charges_API.DTO;
using Charges_API.Mappers;
using Domain.Charges.Entities;
using Domain.Charges.Interfaces.UseCases;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
            var formattedCPF = cpfValidationService.CPFToNumericString(createChargeDTO.ClientCPF);
            var newCharge = new Charge(createChargeDTO.Value, createChargeDTO.DueDate, formattedCPF);
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

            IAsyncEnumerable<Charge>? chargesAsyncEnumerable = null;

            if (!string.IsNullOrWhiteSpace(cpf))
            {
                if (!cpfValidationService.IsCpf(cpf))
                {
                    return BadRequest("Invalid CPF.");
                }
                string formattedCPF = cpfValidationService.CPFToNumericString(cpf);
                chargesAsyncEnumerable = getChargesUseCase.GetChargesByCPF(formattedCPF);
            }

            if (month != null)
            {
                chargesAsyncEnumerable = getChargesUseCase.GetChargesByMonth((int)month);
            }

            List<Charge> chargesList = new List<Charge>();

            await foreach(Charge charge in chargesAsyncEnumerable)
            {
                chargesList.Add(charge);
            }

            return Ok(ChargeMapper.ListToDTO(chargesList));
        }
    }
}
