using System;
using System.Collections.Generic;

namespace DesignPatterns.Module06.Prototype.Practical
{
    public interface IDeepCloneable<T>
    {
        T DeepClone();
    }

    public class Weapon : IDeepCloneable<Weapon>
    {
        public string Name { get; set; }
        public int Damage { get; set; }

        public Weapon(string name, int damage)
        {
            Name = name;
            Damage = damage;
        }

        public Weapon DeepClone()
        {
            return new Weapon(this.Name, this.Damage);
        }

        public override string ToString()
        {
            return $"Оружие: {Name}, Урон: {Damage}";
        }
    }

    public class Armor : IDeepCloneable<Armor>
    {
        public string Name { get; set; }
        public int Defense { get; set; }

        public Armor(string name, int defense)
        {
            Name = name;
            Defense = defense;
        }

        public Armor DeepClone()
        {
            return new Armor(this.Name, this.Defense);
        }

        public override string ToString()
        {
            return $"Броня: {Name}, Защита: {Defense}";
        }
    }

    public class Skill : IDeepCloneable<Skill>
    {
        public string Name { get; set; }
        public string Type { get; set; }

        public Skill(string name, string type)
        {
            Name = name;
            Type = type;
        }

        public Skill DeepClone()
        {
            return new Skill(this.Name, this.Type);
        }

        public override string ToString()
        {
            return $"Умение: {Name}, Тип: {Type}";
        }
    }

    public class Character : IDeepCloneable<Character>
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int Strength { get; set; }
        public int Agility { get; set; }

        public Weapon Weapon { get; set; }
        public Armor Armor { get; set; }
        public List<Skill> Skills { get; set; } = new List<Skill>();

        public Character(string name, int health, int strength, int agility)
        {
            Name = name;
            Health = health;
            Strength = strength;
            Agility = agility;
        }

        public Character DeepClone()
        {
            Character clone = new Character(this.Name, this.Health, this.Strength, this.Agility);
            clone.Weapon = this.Weapon.DeepClone();
            clone.Armor = this.Armor.DeepClone();

            foreach (var skill in this.Skills)
            {
                clone.Skills.Add(skill.DeepClone());
            }

            return clone;
        }

        public override string ToString()
        {
            string skillsDescription = string.Join(", ", Skills);
            return $"Персонаж: {Name}, Здоровье: {Health}, Сила: {Strength}, Ловкость: {Agility}\n" +
                   $"{Weapon}\n{Armor}\nНавыки: {skillsDescription}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
           
            Character warrior = new Character("Воин", 100, 80, 60)
            {
                Weapon = new Weapon("Меч", 50),
                Armor = new Armor("Кольчуга", 30)
            };
            warrior.Skills.Add(new Skill("Мощный удар", "Физическая"));
            warrior.Skills.Add(new Skill("Защитная стойка", "Физическая"));

            Console.WriteLine("Оригинальный персонаж:");
            Console.WriteLine(warrior);

            Character clonedWarrior = warrior.DeepClone();
            clonedWarrior.Name = "Клон Воина";
            clonedWarrior.Weapon.Name = "Магический меч"; 
            clonedWarrior.Weapon.Damage = 70;
            clonedWarrior.Skills.Add(new Skill("Огненный шар", "Магическая"));

            Console.WriteLine("\nКлонированный персонаж:");
            Console.WriteLine(clonedWarrior);

            Console.WriteLine("\nОригинальный персонаж после изменений:");
            Console.WriteLine(warrior);
        }
    }
}
