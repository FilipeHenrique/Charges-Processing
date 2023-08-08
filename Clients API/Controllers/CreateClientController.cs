using Domain.Contracts.UseCases;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Clients_API.Controllers
{
    [Route ("clients")]
    [ApiController]
    public class CreateClientController : Controller
    {
        private readonly ICreateClientUseCase _createClientUseCase;

        public CreateClientController(ICreateClientUseCase createClientUseCase)
        {
            _createClientUseCase = createClientUseCase;
        }

        [HttpPost]
        public IActionResult CreateClient(Client client)
        {
            var newClient = new Client(client.Name, client.CPF, client.State);
            _createClientUseCase.CreateClient(newClient);
            return Created("", newClient);
        }
    }
}
