using Features.Users.GetAll;

namespace JobMe.Test.Application.Features.Users.GetAll
{
    public class GetAllUserQueryRequestTest
    {
        [Fact]
        public void IsValid_ShouldReturnTrue()
        {
            // Arrange
            var query = new GetAllUserQueryRequest();

            // Act
            var result = query.IsValid();

            // Assert
            Assert.True(result);
        }
    }
}