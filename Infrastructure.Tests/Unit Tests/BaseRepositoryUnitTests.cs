using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Domain.Clients.Entities;


namespace Infrastructure.Tests.Unit_Tests
{
    public class BaseRepositoryUnitTests
    {
        private readonly Mock<DbContext> dbContextMock = new Mock<DbContext>();
        private readonly Mock<DbSet<Client>> dbSetMock = new Mock<DbSet<Client>>();
        private readonly BaseRepository<Client> repository;

        public BaseRepositoryUnitTests()
        {
            dbContextMock.Setup(dbset => dbset.Set<Client>()).Returns(dbSetMock.Object);
            repository = new BaseRepository<Client>(dbContextMock.Object);
        }

        [Fact]
        public async Task Create_ShouldAddEntityToDbSetAndSaveChanges()
        {
            var entity = new Client("Lucas", "MG", "960.747.590-90");
            await repository.Create(entity);

            dbSetMock.Verify(dbset => dbset.AddAsync(entity, It.IsAny<CancellationToken>()), Times.Once);
            dbContextMock.Verify(dbset => dbset.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Get_ShouldReturnDbSet()
        {
            var result = repository.Get();
            Assert.Same(dbSetMock.Object, result);
        }

        [Fact]
        public void GetAll_ShouldReturnIAsyncEnumerableFromEntity()
        {
            var result = repository.GetAll();
            Assert.IsAssignableFrom<IAsyncEnumerable<Client>>(result);
        }
    }
}