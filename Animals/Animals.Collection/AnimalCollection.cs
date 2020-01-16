using System;
using System.Collections.Generic;
using System.Linq;

namespace Animals.Logic
{
    public class AnimalCollection
    {
        private readonly List<Animal> animals;

        public AnimalCollection()
        {
            animals = new List<Animal>();
        }

        public void Add(Animal animal)
        {
            animals.Add(animal);
        }

        public void Add(string type, string name, int age, string gender)
        {
            try
            {
                var animal = new Animal(type, name, age, gender);
                animals.Add(animal);
            }
            catch (System.ArgumentException)
            {

            }
        }

        public void Remove(string name)
        {
            animals.RemoveAll(a => a.Name.ToLower() == name.ToLower());
        }
        public IEnumerable<Animal> List()
        {
            foreach (var animal in animals)
            {
                yield return animal;
            }
        }

        public IEnumerable<Animal> List(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException("type parameter cannot be null or empty");
            }
            return animals.Where(a => a.Type.ToLower() == type.ToLower());
        }

        public Animal RetrieveOldest()
        {
            return animals.OrderByDescending(a => a.Age).First();
        }

        public Animal RetrieveOldest(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException("type parameter cannot be null or empty");
            }

            if (GetTypes().Contains(type))
            {
                return animals.Where(a => a.Type.ToLower() == type.ToLower())
                               .OrderByDescending(a => a.Age).First();
            }
            return null;
        }

        public Animal RetrieveYoungest()
        {
            return animals.OrderBy(a => a.Age).First();
        }

        public Animal RetrieveYoungest(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException("type parameter cannot be null or empty");
            }

            if (GetTypes().Contains(type))
            {
                return animals.Where(a => a.Type.ToLower() == type.ToLower())
                               .OrderBy(a => a.Age).First();
            }
            return null;
        }

        public IEnumerable<string> RetrieveDuplicateNames()
        {
            return animals.GroupBy(a => a.Name)
                          .Where(g => g.Count() > 1)
                          .Select(g => g.Key);
        }

        public int Count()
        {
            return animals.Count;
        }

        public int CountByType(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException("type parameter cannot be null or empty");
            }

            if (GetTypes().Contains(type))
            {
                return animals.Where(a => a.Type.ToLower() == type.ToLower()).Count();
            }
            return 0;
        }

        public int Count(string gender)
        {
            return animals.Where(a => a.Gender.ToString() == gender).Count();
        }

        public int Count(string type, string gender)
        {
            return animals.Where(a => a.Type == type && a.Gender.ToString() == gender).Count();
        }

        public IEnumerable<string> GetTypes()
        {
            return animals.GroupBy(a => a.Type)
                            .Select(g => g.Key);
        }
    }
}
