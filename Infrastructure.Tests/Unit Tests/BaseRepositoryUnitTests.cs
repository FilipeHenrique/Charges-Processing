using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Domain.Clients.Entities;

namespace Infrastructure.Tests.Unit_Tests
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options): base(options){}
        public DbSet<Client> Clients { get; set; }
    }

    public class BaseRepositoryUnitTests : IDisposable
    {
        private readonly DbContextOptions<TestDbContext> options;
        private TestDbContext dbContext;
        private readonly BaseRepository<Client> repository;

        public BaseRepositoryUnitTests()
        {
            options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            dbContext = new TestDbContext(options);
            repository = new BaseRepository<Client>(dbContext);

        }

        public void Dispose()
        {
            dbContext.Dispose();
        }


        [Fact]
        public async Task Create_Should_Add_Entity()
        {

            var expectedEntity = new Client("Lucas", "MG", "960.747.590-90");

            await repository.Create(expectedEntity);

            var actualEntity = dbContext.Clients.FirstOrDefault();

            Assert.Equal(expectedEntity.Name, actualEntity.Name);
            Assert.Equal(expectedEntity.Name, actualEntity.Name);
            Assert.Equal(expectedEntity.State, actualEntity.State);
        }


        [Fact]
        public async Task GetAll_Should_Return_All_Entities()
        {
            var expectedEntity1 = new Client("Lucas1", "MG", "960.747.590-91");
            var expectedEntity2 = new Client("Lucas2", "MG", "960.747.590-92");

            await repository.Create(expectedEntity1);
            await repository.Create(expectedEntity2);

            var entities = await repository.GetAll().ToListAsync();

            Assert.Equal(2, entities.Count);
            Assert.Contains(expectedEntity1, entities);
            Assert.Contains(expectedEntity2, entities);
        }

        [Fact]
        public async Task Get_Should_Return_Client_Entity()
        {
            var expectedEntity = new Client("Lucas", "MG", "960.747.590-90");
            dbContext.Clients.Add(expectedEntity);
            await dbContext.SaveChangesAsync();

            var retrievedEntity = await repository.Get().FirstOrDefaultAsync();

            Assert.Equal(expectedEntity.Name, retrievedEntity.Name);
            Assert.Equal(expectedEntity.State, retrievedEntity.State);
            Assert.Equal(expectedEntity.CPF, retrievedEntity.CPF);
        }
    }
}