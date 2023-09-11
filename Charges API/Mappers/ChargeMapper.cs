using Charges_API.DTO;
using Domain.Charges.Entities;

namespace Charges_API.Mappers
{
    public class ChargeMapper
    {
        public static ChargeDTO ToChargeDTO(Charge charge)
        {
            return new ChargeDTO(charge.Value, charge.DueDate, charge.ClientCPF);
        }

        public static Charge ToCharge(ChargeDTO createChargeDTO)
        {
            return new Charge(createChargeDTO.Value, createChargeDTO.DueDate, createChargeDTO.ClientCPF);
        }

        public static IQueryable<ChargeDTO> ChargesToDTO(IQueryable<Charge> charges)
        {
            return charges.Select(charge => new ChargeDTO(charge.Value, charge.DueDate, charge.ClientCPF));
        }
    }
}
