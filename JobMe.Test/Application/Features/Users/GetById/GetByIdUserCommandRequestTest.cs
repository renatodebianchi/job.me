using Features.Users.GetById;

namespace JobMe.Test.Application.Features.Users.GetById
{
    public class GetByIdUserQueryRequestTest
    {
        [Fact]
        public void IsValid_ShouldReturnFalse_WhenIdIsZero()
        {
            // Arrange
            var query = new GetByIdUserQueryRequest { Id = 0 };

            // Act
            var result = query.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains(query.Notifications, n => n.Key == "Id" && n.Value == "Id must be greater than 0");
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenIdIsNegative()
        {
            // Arrange
            var query = new GetByIdUserQueryRequest { Id = -1 };

            // Act
            var result = query.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains(query.Notifications, n => n.Key == "Id" && n.Value == "Id must be greater than 0");
        }

        [Fact]
        public void IsValid_ShouldReturnTrue_WhenIdIsPositive()
        {
            // Arrange
            var query = new GetByIdUserQueryRequest { Id = 1 };

            // Act
            var result = query.IsValid();

            // Assert
            Assert.True(result);
            Assert.Empty(query.Notifications);
        }
    }
}