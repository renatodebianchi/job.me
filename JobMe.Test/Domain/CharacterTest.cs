using Domain.Enums;
using Domain.Entities;

namespace JobMe.Test.Domain.Entities
{
    public class CharacterTest
    {
        [Fact]
        public void Character_ShouldInitializeCorrectly()
        {
            // Arrange & Act
            var character = new Character("Hero", 10, 100, 80, 15, 10, 5, CharacterStatus.Active);

            // Assert
            Assert.Equal("Hero", character.Name);
            Assert.Equal(10, character.Level);
            Assert.Equal(100, character.MaxHealth);
            Assert.Equal(80, character.Health);
            Assert.Equal(15, character.PhysicalAtack);
            Assert.Equal(10, character.PhysicalDefense);
            Assert.Equal(5, character.Speed);
            Assert.Equal(CharacterStatus.Active, character.Status);
        }

        [Fact]
        public void LevelUp_ShouldIncreaseLevel()
        {
            // Arrange
            var character = new Character("Hero", 10);

            // Act
            character.LevelUp();

            // Assert
            Assert.Equal(11, character.Level);
        }

        [Fact]
        public void TakeDamage_ShouldReduceHealth()
        {
            // Arrange
            var character = new Character("Hero", 10, 100, 80);

            // Act
            character.TakeDamage(30);

            // Assert
            Assert.Equal(50, character.Health);
        }

        [Fact]
        public void TakeDamage_ShouldSetStatusToDead_WhenHealthIsZero()
        {
            // Arrange
            var character = new Character("Hero", 10, 100, 30);

            // Act
            character.TakeDamage(30);

            // Assert
            Assert.Equal(0, character.Health);
            Assert.Equal(CharacterStatus.Dead, character.Status);
        }

        [Fact]
        public void Heal_ShouldIncreaseHealth()
        {
            // Arrange
            var character = new Character("Hero", 10, 100, 50);

            // Act
            character.Heal(30);

            // Assert
            Assert.Equal(80, character.Health);
        }

        [Fact]
        public void Heal_ShouldNotExceedMaxHealth()
        {
            // Arrange
            var character = new Character("Hero", 10, 100, 90);

            // Act
            character.Heal(20);

            // Assert
            Assert.Equal(100, character.Health);
        }

        [Fact]
        public void ToString_ShouldReturnCorrectFormat()
        {
            // Arrange
            var character = new Character("Hero", 10);

            // Act
            var result = character.ToString();

            // Assert
            Assert.Equal("Hero (Level 10)", result);
        }

        [Fact]
        public void CalculatePhysicalAtackDamage_ShouldReturnCorrectDamage()
        {
            // Arrange
            var attacker = new Character("Attacker", 10, 100, 80, 15, 5, 5);
            var target = new Character("Target", 10, 100, 80, 10, 10, 5);

            // Act
            var damage = attacker.CalculatePhysicalAtackDamage(target);

            // Assert
            Assert.Equal(5, damage);
        }

        [Fact]
        public void CalculatePhysicalAtackDamageTaken_ShouldReturnCorrectDamage()
        {
            // Arrange
            var attacker = new Character("Attacker", 10, 100, 80, 15, 5, 5);
            var target = new Character("Target", 10, 100, 80, 10, 10, 5);

            // Act
            var damage = target.CalculatePhysicalAtackDamageTaken(attacker);

            // Assert
            Assert.Equal(5, damage);
        }

        [Fact]
        public void WithCriticalChance_ShouldReturnIncreasedDamage_WhenCriticalHitOccurs()
        {
            // Arrange
            double damage = 10;
            double percentage = 100; // Ensuring critical hit
            double multiplier = 2;

            // Act
            var result = damage.WithCriticalChance(percentage, multiplier);

            // Assert
            Assert.Equal(20, result);
        }

        [Fact]
        public void WithCriticalChance_ShouldReturnSameDamage_WhenNoCriticalHitOccurs()
        {
            // Arrange
            double damage = 10;
            double percentage = 0; // Ensuring no critical hit
            double multiplier = 2;

            // Act
            var result = damage.WithCriticalChance(percentage, multiplier);

            // Assert
            Assert.Equal(10, result);
        }
    }
}