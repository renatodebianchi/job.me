using Domain.Entities;
using Domain.Enums;

namespace Domain.Entities
{
    public class Character : GenericEntity
    {
        
        public string Name { get; set; } = string.Empty;
        public int Level { get; set; } = 1;
        public double MaxHealth { get; set; } = 1;
        public double Health { get; set; } = 1;
        public double PhysicalAtack { get; set; } = 1;
        public double PhysicalDefense { get; set; } = 1;
        public double Speed { get; set; } = 1;
        public CharacterStatus Status { get; set; } = CharacterStatus.Active;
        
        public Character(string name, int level, double maxHealth = 1, double health = 1, double physicalAtack = 1, double physicalDefense = 1, double speed = 1, CharacterStatus status = CharacterStatus.Active) : base()
        {
            Name = name;
            Level = level;
            MaxHealth = maxHealth;
            Health = health;
            PhysicalAtack = physicalAtack;
            PhysicalDefense = physicalDefense;
            Speed = speed;
            Status = status;
        }

        public void LevelUp()
        {
            Level++;
            MaxHealth *= 1.15;
            Health = MaxHealth;
            PhysicalAtack *= 1.1;
            PhysicalDefense *= 1.08;
            Speed *= 1.05;
        }

        public void TakeDamage(double damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                Health = 0;
                Status = CharacterStatus.Dead;
            }
        }

        public void Heal(double heal)
        {
            Health += heal;
            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }
        }

        public override string ToString()
        {
            return $"{Name} (Level {Level})";
        }
    }

    public static class DamageExtensions
    {
        public static double CalculatePhysicalAtackDamage(this Character character, Character target)
        {
            if (target.Status.Equals(CharacterStatus.Dead)) return 0;

            double damage = character.PhysicalAtack - target.PhysicalDefense;
            if (damage <= 0)
            {
                damage = 1;
            }
            
            return damage;
        }

        public static double CalculatePhysicalAtackDamageTaken(this Character character, Character target)
        {
            if (character.Status.Equals(CharacterStatus.Dead)) return 0;

            double damage = target.PhysicalAtack - character.PhysicalDefense;
            if (damage <= 0)
            {
                damage = 1;
            }
            return damage;
        }

        public static double WithCriticalChance(this double currentDamage, double percentage = 5, double multiplier = 2)
        {
            double damage = currentDamage;
            double randonNumber = new Random().Next(0, 10000);
            if (randonNumber <= percentage*100)
            {
                damage *= multiplier;
            }
            return damage;
        }
    }

}