using Features.Users.Delete;

namespace JobMe.Test.Application.Features.Users.Delete
{
    public class DeleteUserCommandRequestTest
    {
        [Fact]
        public void IsValid_ShouldReturnFalse_WhenIdIsZero()
        {
            // Arrange
            var command = new DeleteUserCommandRequest { Id = 0 };

            // Act
            var result = command.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains(command.Notifications, n => n.Key == "Invalid Id" && n.Value == "Id is required");
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenIdIsNegative()
        {
            // Arrange
            var command = new DeleteUserCommandRequest { Id = -1 };

            // Act
            var result = command.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains(command.Notifications, n => n.Key == "Invalid Id" && n.Value == "Id is required");
        }

        [Fact]
        public void IsValid_ShouldReturnTrue_WhenIdIsPositive()
        {
            // Arrange
            var command = new DeleteUserCommandRequest { Id = 1 };

            // Act
            var result = command.IsValid();

            // Assert
            Assert.True(result);
            Assert.Empty(command.Notifications);
        }
    }
}