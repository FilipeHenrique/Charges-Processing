using Domain.Contracts.UseCases.Charges;
using Domain.Entities;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace Charges_API.Controllers
{
    [ApiController]
    [Route("/charges")]
    public class ListChargesController : Controller
    {
        private readonly IListChargesUseCase _getChargesUseCase;
        private readonly ICPFValidationService _cpfValidationService;
        public ListChargesController(IListChargesUseCase getChargesUseCase, ICPFValidationService cpfValidationService) {
            _getChargesUseCase = getChargesUseCase;
            _cpfValidationService = cpfValidationService;
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
                if (!_cpfValidationService.IsCpf(cpf))
                {
                    return BadRequest("Invalid CPF.");
                }
                string formattedCPF = _cpfValidationService.CPFToNumericString(cpf);
                charges = await _getChargesUseCase.GetChargesByCPF(formattedCPF);
            }

            if (month != null)
            {
               charges = await _getChargesUseCase.GetChargesByMonth((int)month);
            }

            return Ok(charges);
        }
    }
}
