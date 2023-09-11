using Infrastructure.Database;

namespace Infrastructure.Repositories
{
    public class ChargesRepository<T> : BaseRepository<T> where T : class
    {
        public ChargesRepository(DBContext<T> context) : base(context)
        {

        }
    }
}
