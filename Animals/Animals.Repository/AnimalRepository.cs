using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Animals.Logic;

namespace Animals.Repository
{
    public static class AnimalRepository
    {
        public static AnimalCollection ReadFromJsonFile(string path)
        {
            var animalCollection = new AnimalCollection();

            var streamReader = new StreamReader(path);
            var json = (JObject)JToken.ReadFrom(new JsonTextReader(streamReader));
            var animalTypes = json.Children();

            foreach (var type in animalTypes)
            {
                var animals = type.Children().First();
                foreach (var animal in animals)
                {
                    Animal newAnimal = new Animal(type.Path,
                                             animal.SelectToken("breed").ToString(),
                                             animal.SelectToken("name").ToString(),
                                             int.Parse(animal.SelectToken("age").ToString()),
                                             animal.SelectToken("gender").ToString());

                    animalCollection.Add(newAnimal);
                }
            }
            streamReader.Close();
            return animalCollection;
        }

        public static void WriteToJsonFile(AnimalCollection animalCollection, string path)
        {
            JContainer jsonFile = (JContainer)JObject.FromObject(new object());
            
            foreach (var animalType in animalCollection.GetTypes())
            {
                var animalArray = new JArray();
                foreach (var animal in animalCollection.List(animalType))
                {
                    var animalToken = JToken.FromObject(new
                    {
                        breed = animal.Breed,
                        age = animal.Age,
                        name = animal.Name,
                        gender = animal.Gender.ToString()
                    });
                    animalArray.Add(animalToken);
                }
                var animalArrayWithLabel = new JProperty(animalType, animalArray);
                jsonFile.Add(animalArrayWithLabel);
            }

            var streamWriter = new StreamWriter(path);
            streamWriter.WriteLine(jsonFile);
            streamWriter.Flush();
            streamWriter.Close();
        }
    }
}
