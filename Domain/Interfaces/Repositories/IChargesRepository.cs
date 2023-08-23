﻿using Domain.Entities;

namespace Domain.Contracts.Repositories
{
    public interface IChargesRepository
    {
        Task Create(Charge charge);
        Task<List<Charge>> ListByCPF(string cpf);
        Task<List<Charge>> ListByMonth(int month);
    }
}