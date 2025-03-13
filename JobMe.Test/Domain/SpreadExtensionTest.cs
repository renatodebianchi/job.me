using System.Reflection;
using Domain.Extensions;
using Xunit;

namespace JobMe.Test.Domain.Extensions
{
    public class SpreadExtensionTest
    {
        private class Source
        {
            public string? Name { get; set; }
            public int Age { get; set; }
            public DateTime? BirthDate { get; set; }
        }

        private class Target
        {
            public string? Name { get; set; }
            public int Age { get; set; }
            public DateTime? BirthDate { get; set; }
        }

        [Fact]
        public void Spread_ShouldCopyProperties()
        {
            // Arrange
            var source = new Source { Name = "John", Age = 30, BirthDate = new DateTime(1990, 1, 1) };
            var target = new Target();

            // Act
            target.Spread(source);

            // Assert
            Assert.Equal(source.Name, target.Name);
            Assert.Equal(source.Age, target.Age);
            Assert.Equal(source.BirthDate, target.BirthDate);
        }

        [Fact]
        public void SpreadNotNull_ShouldCopyNonNullProperties()
        {
            // Arrange
            var source = new Source { Name = "John", Age = 30, BirthDate = null };
            var target = new Target { Name = "Doe", Age = 25, BirthDate = new DateTime(1995, 1, 1) };

            // Act
            target.SpreadNotNull(source);

            // Assert
            Assert.Equal(source.Name, target.Name);
            Assert.Equal(source.Age, target.Age);
            Assert.NotNull(target.BirthDate);
            Assert.Equal(new DateTime(1995, 1, 1), target.BirthDate);
        }

        [Fact]
        public void SpreadNotNull_ShouldSkipDefaultDateTime()
        {
            // Arrange
            var source = new Source { Name = "John", Age = 30, BirthDate = new DateTime() };
            var target = new Target { Name = "Doe", Age = 25, BirthDate = new DateTime(1995, 1, 1) };

            // Act
            target.SpreadNotNull(source);

            // Assert
            Assert.Equal(source.Name, target.Name);
            Assert.Equal(source.Age, target.Age);
            Assert.NotNull(target.BirthDate);
            Assert.Equal(new DateTime(1995, 1, 1), target.BirthDate);
        }
    }
}