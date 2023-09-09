using Charges_API.DTO;
using Charges_API.Mappers;
using Domain.Charges.Entities;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Charges_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChargesController : Controller
    {
        private readonly ICPFHandler cpfHandler;
        private readonly IRepository<Charge> repository;

        public ChargesController(ICPFHandler cpfHandler, IRepository<Charge> repository)
        {
            this.cpfHandler = cpfHandler;
            this.repository = repository;
        }

        [HttpPost]
        public IActionResult Create(ChargesDTO chargeDTO)
        {

            if (!cpfHandler.IsCpf(chargeDTO.ClientCPF))
            {
                return BadRequest("Invalid CPF.");
            }
            var formattedCPF = cpfHandler.CPFToNumericString(chargeDTO.ClientCPF);
            var newCharge = new Charge(chargeDTO.Value, chargeDTO.DueDate, formattedCPF);
            repository.Create(newCharge);
            return Created("", newCharge);
        }

        [HttpGet]
        public IActionResult GetAll(string cpf = null, int? month = null)
        {
            if (string.IsNullOrWhiteSpace(cpf) && month == null)
            {
                return BadRequest("Atleast one query param is necessary, please specify cpf or dueDate");
            }

            var charges = repository.Get();

            if (!string.IsNullOrWhiteSpace(cpf) && month != null)
            {
                if (!cpfHandler.IsCpf(cpf))
                {
                    return BadRequest("Invalid CPF.");
                }

                string formattedCPF = cpfHandler.CPFToNumericString(cpf);

                var currentYear = DateTime.UtcNow.Year;
                var startDate = new DateTime(currentYear, (int)month, 1);
                var endDate = startDate.AddMonths(1);

                charges = charges.Where(charge => charge.ClientCPF == formattedCPF)
                    .Where(charge => charge.DueDate > startDate && charge.DueDate < endDate);
            }

            if (!string.IsNullOrWhiteSpace(cpf))
            {
                if (!cpfHandler.IsCpf(cpf))
                {
                    return BadRequest("Invalid CPF.");
                }
                string formattedCPF = cpfHandler.CPFToNumericString(cpf);
                charges = charges.Where(charge => charge.ClientCPF == formattedCPF);
            }

            if (month != null)
            {
                var currentYear = DateTime.UtcNow.Year;
                var startDate = new DateTime(currentYear, (int)month, 1);
                var endDate = startDate.AddMonths(1);

                charges = charges.Where(charge => charge.DueDate > startDate && charge.DueDate < endDate);
            }

            return Ok(ChargeMapper.ListToDTO(charges));
        }
    }
}
