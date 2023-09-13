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
        public ActionResult Create(ChargeDTO chargeDTO)
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
        public ActionResult GetAll(string cpf, int? month)
        {
            if (string.IsNullOrWhiteSpace(cpf) && month == null)
            {
                return BadRequest("Atleast one parameter is necessary. Please specify cpf or month.");
            }

            if (!string.IsNullOrWhiteSpace(cpf) && !cpfHandler.IsCpf(cpf))
            {
                return BadRequest("Invalid CPF.");
            }

            var filteredCharges = repository.Get();

            if (!string.IsNullOrWhiteSpace(cpf))
            {
                string formattedCPF = cpfHandler.CPFToNumericString(cpf);
                filteredCharges = filteredCharges.Where(charge => charge.ClientCPF == formattedCPF);
            }

            if (month != null)
            {
                var currentYear = DateTime.UtcNow.Year;
                var startDate = new DateTime(currentYear, month.Value, 1);
                var endDate = startDate.AddMonths(1);

                filteredCharges = filteredCharges.Where(charge => charge.DueDate > startDate && charge.DueDate < endDate);
            }

            return Ok(ChargeMapper.ChargesToDTO(filteredCharges));
        }
    }
}
