using Domain.Contracts.UseCases.Clients;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Clients_API.Controllers
{
    [Route("clients/")]
    [ApiController]
    public class ListClientsController : Controller
    {
        private readonly IListClientsUseCase _listClientsUseCase;
        public ListClientsController(IListClientsUseCase listClientsUseCase) {
            _listClientsUseCase = listClientsUseCase;
        }
        [HttpGet]
        public async Task<IActionResult> ListClients()
        {
            List<Client> clients = await _listClientsUseCase.ListClients();
            return Ok(clients);
        }
    }
}
