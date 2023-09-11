﻿using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> dbSet;
        private readonly DbContext dbContext;

        public BaseRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = dbContext.Set<T>();
        }

        public async Task Create(T entity)
        {
            await dbSet.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public IQueryable<T> Get()
        {
            return dbSet;
        }

        public async IAsyncEnumerable<T> GetAll()
        {
            var entities = await dbSet.ToListAsync();
            foreach (var entity in entities)
            {
                yield return entity;
            }
        }

    }
}
