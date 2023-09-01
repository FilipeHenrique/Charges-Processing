using Infrastructure.DbContext;

namespace Infrastructure.Repositories
{
    public class ClientsRepository<T> : BaseRepository<T> where T : class
    {
        public ClientsRepository(IDBContext context, string collectionName) : base(context, collectionName)
        {

        }
    }
}
