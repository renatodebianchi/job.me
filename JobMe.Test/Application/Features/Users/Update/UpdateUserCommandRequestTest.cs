using Domain.Entities;
using Features.Users.Update;

namespace JobMe.Test.Application.Features.Users.Update
{
    public class UpdateUserCommandRequestTest
    {
        [Fact]
        public void IsValid_ShouldReturnFalse_WhenIdIsZero()
        {
            // Arrange
            var command = new UpdateUserCommandRequest { Id = 0 };

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
            var command = new UpdateUserCommandRequest { Id = -1 };

            // Act
            var result = command.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains(command.Notifications, n => n.Key == "Invalid Id" && n.Value == "Id is required");
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenModelIsNull()
        {
            // Arrange
            var command = new UpdateUserCommandRequest { Id = 1, DataModel = null };

            // Act
            var result = command.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains(command.Notifications, n => n.Key == "Invalid Model" && n.Value == "Model is null");
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenUsernameIsEmpty()
        {
            // Arrange
            var command = new UpdateUserCommandRequest
            {
                Id = 1,
                DataModel = new User { Username = "", Password = "password", Email = "email@example.com" }
            };

            // Act
            var result = command.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains(command.Notifications, n => n.Key == "Invalid Username" && n.Value == "Username is required");
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenPasswordIsEmpty()
        {
            // Arrange
            var command = new UpdateUserCommandRequest
            {
                Id = 1,
                DataModel = new User { Username = "username", Password = "", Email = "email@example.com" }
            };

            // Act
            var result = command.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains(command.Notifications, n => n.Key == "Invalid Password" && n.Value == "Password is required");
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenEmailIsEmpty()
        {
            // Arrange
            var command = new UpdateUserCommandRequest
            {
                Id = 1,
                DataModel = new User { Username = "username", Password = "password", Email = "" }
            };

            // Act
            var result = command.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains(command.Notifications, n => n.Key == "Invalid Email" && n.Value == "Email is required");
        }

        [Fact]
        public void IsValid_ShouldReturnTrue_WhenAllFieldsAreValid()
        {
            // Arrange
            var command = new UpdateUserCommandRequest
            {
                Id = 1,
                DataModel = new User { Username = "username", Password = "password", Email = "email@example.com" }
            };

            // Act
            var result = command.IsValid();

            // Assert
            Assert.True(result);
            Assert.Empty(command.Notifications);
        }
    }
}