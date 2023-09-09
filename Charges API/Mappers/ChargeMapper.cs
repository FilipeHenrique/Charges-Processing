using Charges_API.DTO;
using Domain.Charges.Entities;

namespace Charges_API.Mappers
{
    public class ChargeMapper
    {
        public static ChargesDTO ToChargeDTO(Charge charge)
        {
            return new ChargesDTO(charge.Value, charge.DueDate, charge.ClientCPF);
        }

        public static Charge ToCharge(ChargesDTO createChargeDTO)
        {
            return new Charge(createChargeDTO.Value, createChargeDTO.DueDate, createChargeDTO.ClientCPF);
        }

        public static IEnumerable<ChargesDTO> ListToDTO(IEnumerable<Charge> charges)
        {
            return charges.Select(charge => new ChargesDTO(charge.Value, charge.DueDate, charge.ClientCPF));
        }
    }
}
