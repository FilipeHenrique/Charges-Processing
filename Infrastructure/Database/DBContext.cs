using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
{
    public class DBContext<T> : DbContext where T : class
    {
        public DbSet<T> Entity { get; set; }

        public DBContext(DbContextOptions<DBContext<T>> options) : base(options)
        {
        }
    }
}