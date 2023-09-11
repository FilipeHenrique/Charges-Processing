using Infrastructure.Database;

namespace Infrastructure.Repositories
{
    public class ClientsRepository<T> : BaseRepository<T> where T : class
    {
        public ClientsRepository(DBContext<T> context) : base(context)
        {

        }
    }
}
