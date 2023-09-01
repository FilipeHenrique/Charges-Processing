namespace Domain.Clients.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public Task Create(T entity);
        public IQueryable<T> Get();
        public IAsyncEnumerable<T> GetAll();
    }
}
