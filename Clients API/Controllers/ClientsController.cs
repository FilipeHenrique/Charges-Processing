﻿using Clients_API.DTO;
using Clients_API.Mappers;
using Domain.Clients.Entities;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Clients_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClientsController : Controller
    {
        private readonly ICPFHandler cpfHandler;
        private readonly IRepository<Client> repository;

        public ClientsController(ICPFHandler cpfHandler, IRepository<Client> repository)
        {
            this.cpfHandler = cpfHandler;
            this.repository = repository;
        }

        [HttpPost]
        public ActionResult CreateClient(ClientDTO clientDTO)
        {
            if (!cpfHandler.IsCpf(clientDTO.CPF))
            {
                return BadRequest("Invalid CPF.");
            }

            var formattedCPF = cpfHandler.CPFToNumericString(clientDTO.CPF);

            var client = repository.Get().Where(client => client.CPF == formattedCPF);

            if (client.Any())
            {
                return BadRequest("CPF already exists.");
            }

            clientDTO.CPF = formattedCPF;
            var newClient = ClientMapper.ToClient(clientDTO);
            repository.Create(newClient);
            return Created("", newClient);
        }

        [HttpGet("{cpf}")]
        public ActionResult<Client> GetClient(string cpf)
        {
            if (!cpfHandler.IsCpf(cpf))
            {
                return BadRequest("Invalid CPF.");
            }

            var formattedCPF = cpfHandler.CPFToNumericString(cpf);
            var client = repository.Get().FirstOrDefault(client => client.CPF == formattedCPF);

            if (client == null)
            {
                return NotFound("Client not found.");
            }

            var newClient = ClientMapper.ToClientDTO(client);

            return Ok(newClient);

        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var clientsAsyncEnumerable = repository.GetAll();
            return (Ok(ClientMapper.ClientsToDTO(clientsAsyncEnumerable)));
        }
    }
}
