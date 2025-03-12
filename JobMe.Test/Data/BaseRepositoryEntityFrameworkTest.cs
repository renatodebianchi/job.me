using Domain.Entities;
using Domain.Interfaces.Entities;
using Domain.Interfaces.Repositories;
using Infra.Data.Contexts;
using Infra.Data.Repositories.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace JobMe.Infra.Data.Repositories.EntityFramework
{
    public class ApplicationDbContextTest : ApplicationDbContext
    { 
        public ApplicationDbContextTest(IConfiguration configuration, DbContextOptions<ApplicationDbContext> options) : base(configuration, options) 
        {}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        { 
            optionsBuilder.UseInMemoryDatabase(databaseName: "InMemoryDb"); 
        } 
    }

    public class BaseRepositoryEntityFrameworkTest
    {
        private readonly Mock<IConfiguration> _configuration;
        private readonly ApplicationDbContext _contextMock;
        private readonly IGenericRepository<User> _repository;
        private static int entityCount = 0;
        
        private User CreateNewEntity() => new User { Id = ++entityCount, Email=$"test{entityCount}@test.com", Password="123456", Username=$"test{entityCount}" };
        

        public BaseRepositoryEntityFrameworkTest()
        {
            _configuration = new Mock<IConfiguration>();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;

            _contextMock = new ApplicationDbContextTest(_configuration.Object, options);
            _repository = new BaseRepositoryEntityFramework<User>(_contextMock);
        }

        [Fact]
        public async Task AddAsync_ShouldAddEntity()
        {
            // Arrange
            var entity = CreateNewEntity();

            // Act
            var result = await _repository.AddAsync(entity);

            // Assert
            Assert.Equal(entity.Id, result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteEntity()
        {
            // Arrange
            var entity = CreateNewEntity();

            // Act
            await _repository.AddAsync(entity);
            var deleteResult = await _repository.DeleteAsync(entity.Id);

            // Assert
            Assert.Equal(1, deleteResult);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllEntities()
        {
            // Arrange
            var entity1 = CreateNewEntity();
            var entity2 = CreateNewEntity();

            // Act
            await _repository.AddAsync(entity1);
            await _repository.AddAsync(entity2);
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.True(result.Count() > 0);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntity()
        {
            // Arrange
            var entity = CreateNewEntity();

            // Act
            await _repository.AddAsync(entity);
            var result = await _repository.GetByIdAsync(entity.Id);

            // Assert
            Assert.Equal(entity.Id, result.Id);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateEntity()
        {
            // Arrange
            var entity = CreateNewEntity();

            // Act
            await _repository.AddAsync(entity);
            var result = await _repository.UpdateAsync(entity);

            // Assert
            Assert.Equal(entity.Id, result);
        }

        private Mock<DbSet<T>> MockDbSet<T>(T[] elements) where T : class
        {
            var queryable = elements.AsQueryable();
            var dbSetMock = new Mock<DbSet<T>>();
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            return dbSetMock;
        }
    }
}