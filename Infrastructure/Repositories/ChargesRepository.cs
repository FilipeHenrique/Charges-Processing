using Infrastructure.DbContext;

namespace Infrastructure.Repositories
{
    public class ChargesRepository<T> : BaseRepository<T> where T : class
    {
        public ChargesRepository(IDBContext context, string collectionName) : base(context, collectionName)
        {

        }
    }
}
