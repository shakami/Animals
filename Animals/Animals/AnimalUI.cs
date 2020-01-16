using Animals.Logic;
using Animals.Repository;
using System;
using System.Collections.Generic;
using System.IO;

namespace Animals
{
    public class AnimalUI
    {
        private AnimalCollection animalCollection;
        private const string dataPath = "DataFiles/animals.json";
        public void Run()
        {
            InitializeUI();
            while (true)
            {
                var input = AskForInput();
                var output = ProcessUserInput(input);
                DisplayOutput(output);
            }
        }

        private string AskForInput()
        {
            Console.Write(">");
            return Console.ReadLine().ToLower();
        }

        private void DisplayOutput(IEnumerable<string> output)
        {
            foreach (var line in output)
            {
                Console.WriteLine(line);
            }
        }

        private IEnumerable<string> ProcessUserInput(string userCommand)
        {
            var userCommandArgs = userCommand.Split(' ');
            int argsCount = userCommandArgs.Length - 1;

            switch (userCommandArgs[0])
            {
                case "a":
                case "add":
                    CallToAdd(userCommandArgs, argsCount);
                    break;

                case "r":
                case "remove":
                    CallToRemove(userCommandArgs, argsCount);
                    break;

                case "l":
                case "list":
                    return CallToList(userCommandArgs, argsCount);

                case "o":
                case "oldest":
                    return CallToOldest(userCommandArgs, argsCount);

                case "y":
                case "youngest":
                    return CallToYoungest(userCommandArgs, argsCount);

                case "c":
                case "count":
                    return CallToCount(userCommandArgs, argsCount);

                case "d":
                case "duplicate":
                    return CallToDuplicates();

                case "s":
                case "save":
                    SaveData();
                    break;

                case "h":
                case "help":
                    return DisplayHelp();

                case "clear":
                    Console.Clear();
                    break;

                case "q":
                case "quit":
                    CloseProgram();
                    break;
            }
            return new List<string>();
        }

        private IEnumerable<String> CallToCount(string[] userCommandArgs, int argsCount)
        {
            if (argsCount > 0)
            {
                var gender = Gender.Parse(userCommandArgs[1]);
                if (gender.HasValue)
                {
                    if (argsCount == 2)
                    {
                        var type = userCommandArgs[2];
                        Console.WriteLine(animalCollection.Count(type, gender.ToString()));
                    }
                    else
                    {
                        Console.WriteLine(animalCollection.Count(gender.Value.ToString()));
                    }
                }
                else
                {
                    Console.WriteLine(animalCollection.CountByType(userCommandArgs[1]));
                }
            }
            else
            {
                Console.WriteLine(animalCollection.Count());
            }
            return new List<string>();
        }

        private IEnumerable<string> CallToDuplicates()
        {
            return animalCollection.RetrieveDuplicateNames();
        }

        private IEnumerable<string> CallToYoungest(string[] userCommandArgs, int argsCount)
        {
            if (argsCount == 1)
            {
                var type = userCommandArgs[1];
                if (!string.IsNullOrWhiteSpace(type))
                {
                    yield return animalCollection.RetrieveYoungest(type)?.ToString() ??
                       $"no animal of type {type}";
                }
            }
            else if (argsCount == 0)
            {
                yield return animalCollection.RetrieveYoungest().ToString();
            }
        }

        private IEnumerable<string> CallToOldest(string[] userCommandArgs, int argsCount)
        {
            if (argsCount == 1)
            {
                var type = userCommandArgs[1];
                if (!string.IsNullOrWhiteSpace(type))
                {
                    yield return animalCollection.RetrieveOldest(type)?.ToString() ??
                       $"no animal of type {type}";
                }
            }
            else if (argsCount == 0)
            {
                yield return animalCollection.RetrieveOldest().ToString();
            }
        }

        private void CallToAdd(string[] userCommandArgs, int argsCount)
        {
            if (argsCount == 4)
            {
                var gender = Gender.Parse(userCommandArgs[4]);
                if (int.TryParse(userCommandArgs[3], out int age) &&
                    gender.HasValue)
                {
                    animalCollection.Add(userCommandArgs[1],
                                        userCommandArgs[2],
                                        age,
                                        gender.ToString());
                }
            }
        }

        private void CallToRemove(string[] userCommandArgs, int argsCount)
        {
            if (argsCount == 1)
            {
                animalCollection.Remove(userCommandArgs[1]);
            }
        }

        private IEnumerable<string> CallToList(string[] userCommandArgs, int argsCount)
        {
            IEnumerable<Animal> result = new List<Animal>();

            if (argsCount == 1)
            {
                if (!string.IsNullOrWhiteSpace(userCommandArgs[1]))
                {
                    result = animalCollection.List(userCommandArgs[1]);
                }
            }
            else if (argsCount == 0)
            {
                result = animalCollection.List();
            }

            foreach (var animal in result)
            {
                yield return animal.ToString();
            }
        }

        private IEnumerable<string> DisplayHelp()
        {
            Console.WriteLine("Here is a list of possible commands:\n");

            var helpData = File.ReadAllLines("DataFiles/helpData.txt");
            const int helpDataFormatting = -45;

            foreach (var line in helpData)
            {
                var lineInfo = line.Split(',');
                yield return ($"{lineInfo[0],helpDataFormatting}|{lineInfo[1]}");
            }
        }

        private void InitializeUI()
        {
            this.animalCollection = AnimalRepository.ReadFromJsonFile(dataPath);
            Console.WriteLine("Animal Query Program");
            Console.WriteLine("Enter a command. Type \"help\" for help.");
        }

        private void CloseProgram()
        {
            Console.WriteLine("Save Changes? Type Y or N");

            var needSaving = Console.Read();
            if (needSaving == 'Y' || needSaving == 'y')
            {
                SaveData();
            }
            else
            {
                Console.WriteLine("Exiting without save");
            }

            Environment.Exit(0);
        }

        private void SaveData()
        {
            AnimalRepository.WriteToJsonFile(animalCollection, dataPath);
            Console.WriteLine("Save successful");
        }
    }
}
