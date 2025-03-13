using System.Threading.Tasks;
using Application.Responses;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Features.Users.Add;
using Moq;
using Xunit;

namespace Features.Tests.Users
{
    public class AddUserCommandHandlerTest
    {
        private readonly Mock<IGenericRepository<User>> _userRepositoryMock;
        private readonly AddUserCommandHandler _handler;

        public AddUserCommandHandlerTest()
        {
            _userRepositoryMock = new Mock<IGenericRepository<User>>();
            _handler = new AddUserCommandHandler(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnInvalidResponse_WhenRequestIsInvalid()
        {
            // Arrange
            var request = new AddUserCommandRequest { DataModel = null };
            
            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal("Invalid Request", result.Message);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task Handle_ShouldAddUser_WhenRequestIsValid()
        {
            // Arrange
            var request = new AddUserCommandRequest
            {
                DataModel = new User
                {
                    Username = "testuser",
                    Password = "password",
                    Email = "testuser@test.com"
                }
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Once);
            Assert.True(result.IsSuccessful);
            Assert.Equal("User added successfully", result.Message);
            Assert.NotNull(result.Data);
        }
    }
}