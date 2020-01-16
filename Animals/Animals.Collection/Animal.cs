using System;
using static Animals.Logic.Gender;

namespace Animals.Logic
{
    public class Animal
    {
        public string Type { get; private set; }
        public string Breed { get; private set; } = "unkown breed";
        public string Name { get; private set; }
        public int Age { get; private set; }
        public GenderEnum Gender { get; private set; }

        public Animal(string type, string name, int age, string gender) :
            this(type, "unkown breed", name, age, gender)
        { }

        public Animal(string type, string breed, string name, int age, string gender)
        {
            if (string.IsNullOrWhiteSpace(type) ||
                string.IsNullOrWhiteSpace(breed) ||
                string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Parameter cannot be null or empty");
            }

            if (age < 0)
            {
                throw new ArgumentException("Age cannot be less than zero");
            }

            Gender = Parse(gender) ?? throw new ArgumentException("Invalid gender");

            Type = type.ToLower();
            Breed = breed.ToLower();
            Name = name.ToLower();
            Age = age;
        }

        public override string ToString()
        {
            return $"{Name,-20} {Age,3} year old {Gender,+10} of type {Type}.";
        }
    }
}
