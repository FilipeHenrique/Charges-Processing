using Charges_API.DTO;
using Domain.Charges.Entities;

namespace Charges_API.Mappers
{
    public class ChargeMapper
    {
        public static CreateChargeDTO ToCreateChargeDTO(Charge charge)
        {
            return new CreateChargeDTO(charge.Value, charge.DueDate, charge.ClientCPF);
        }

        public static Charge ToCharge(CreateChargeDTO createChargeDTO)
        {
            return new Charge(createChargeDTO.Value, createChargeDTO.DueDate, createChargeDTO.ClientCPF);
        }

        public static IEnumerable<CreateChargeDTO> ListToDTO(IEnumerable<Charge> charges)
        {
            return charges.Select(charge => new CreateChargeDTO(charge.Value, charge.DueDate, charge.ClientCPF));
        }
    }
}
