using Domain.Entities;
using Features.Users.Add;

namespace JobMe.Test.Application.Features.Users.Add
{
    public class AddUserCommandRequestTest
    {
        [Fact]
        public void IsValid_ShouldReturnFalse_WhenDataModelIsNull()
        {
            // Arrange
            var command = new AddUserCommandRequest();

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
            var command = new AddUserCommandRequest
            {
                DataModel = new User { Password = "password", Email = "test@test.com" }
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
            var command = new AddUserCommandRequest
            {
                DataModel = new User { Username = "username", Email = "test@test.com" }
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
            var command = new AddUserCommandRequest
            {
                DataModel = new User { Username = "username", Password = "password" }
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
            var command = new AddUserCommandRequest
            {
                DataModel = new User { Username = "username", Password = "password", Email = "test@test.com" }
            };

            // Act
            var result = command.IsValid();

            // Assert
            Assert.True(result);
            Assert.Empty(command.Notifications);
        }
    }
}