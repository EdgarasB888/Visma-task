using System;
using System.ComponentModel;
using System.Xml.Linq;
using Visma_task.Controllers;
using Visma_task.Data.Entities;
using Visma_task.Data.Entities.Enums;
using Visma_task.Data.Validations;

namespace Visma_task.UI
{
	public class ConsoleUIManager
	{
        private ShortagesController _shortagesController;

        public ConsoleUIManager(ShortagesController shortagesController)
        {
            _shortagesController = shortagesController;
        }

        public void HandleUI()
        {
            var name = StringValidators.GetValidStringInput("Please enter your name: ", StringValidators.IsNullOrWhiteSpace);

            Console.WriteLine("\nWelcome to the Shortage Management System, {0}!", name);

            while (true)
            {
                Console.WriteLine("\nAvailable commands:");
                Console.WriteLine("1. Add Shortage");
                Console.WriteLine("2. Delete Shortage");
                Console.WriteLine("3. List Shortages");
                Console.WriteLine("4. Filter Shortages");
                Console.WriteLine("0. Exit");

                Console.Write("Enter your choice: ");
                string? choice = Console.ReadLine();



                switch (choice)
                {
                    case "1":
                        HandleAddDataEntryUI(name);
                        break;
                    case "2":
                        HandleDeleteDataEntryUI(name);        
                        break;
                    case "3":
                        var shortages = _shortagesController.GetShortages(name);
                        PrintShortagesTableToConsole(shortages);
                        break;
                    case "4":
                        HandleFilterUI(name);
                        break;
                    case "0":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("\nInvalid choice! Please try again.");
                        break;
                }
            }
        }

        public void HandleFilterUI(string name)
        {
            Console.WriteLine("\nFilter options:");
            Console.WriteLine("1. Filter by Title");
            Console.WriteLine("2. Filter by CreatedOn date");
            Console.WriteLine("3. Filter by Category");
            Console.WriteLine("4. Filter by Room");
            Console.WriteLine("0. Return to main menu");

            Console.Write("Enter your choice: ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    HandleFilterByTitle(name);
                    break;
                case "2":
                    HandleFilterByCreatedOn(name);
                    break;
                case "3":
                    HandleFilterByCategory(name);
                    break;
                case "4":
                    HandleFilterByRoom(name);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("\nInvalid choice! Please try again.");
                    break;
            }
        }

        public void HandleAddDataEntryUI(string name)
        {
            var title = StringValidators.GetValidStringInput("\nEnter shortage title: ", StringValidators.IsNullOrWhiteSpace);
            var room = EnumValidators.GetValidEnumInput<RoomType>("Enter room (Meeting room / Kitchen / Bathroom): ");
            var category = EnumValidators.GetValidEnumInput<CategoryType>("Enter category (Electronics / Food / Other): ");
            var priority = IntValidators.GetValidPriorityInput("Enter priority (1-10): ");

            _shortagesController.AddShortage(new Shortage
            {
                Title = title,
                Name = name,
                Room = room,
                Category = category,
                Priority = priority,
                CreatedOn = DateOnly.FromDateTime(DateTime.Now)
            });
        }

        public void HandleDeleteDataEntryUI(string name)
        {
            var shortages = _shortagesController.GetShortages(name);
            PrintShortagesTableToConsole(shortages);

            var title = StringValidators.GetValidStringInput("\nEnter shortage title: ", StringValidators.IsNullOrWhiteSpace);
            var room = EnumValidators.GetValidEnumInput<RoomType>("Enter room (Meeting room / Kitchen / Bathroom): ");

            _shortagesController.DeleteShortage(name, title, room);
        }

        public void HandleFilterByTitle(string name)
        {
            var title = StringValidators.GetValidStringInput("\nEnter title to filter by: ", StringValidators.IsNullOrWhiteSpace);
            var filteredShortages = _shortagesController.GetShortagesByTitle(name, title);
            PrintShortagesTableToConsole(filteredShortages);
        }

        public void HandleFilterByCreatedOn(string name)
        {
            var startDate = DateOnlyValidators.GetValidDateOnlyInput("\nEnter start date (yyyy-MM-dd): ");
            var endDate = DateOnlyValidators.GetValidDateOnlyInput("Enter end date (yyyy-MM-dd): ");
            var filteredShortages = _shortagesController.GetShortagesByCreatedOn(name, startDate, endDate);
            PrintShortagesTableToConsole(filteredShortages);
        }

        public void HandleFilterByCategory(string name)
        {
            var category = EnumValidators.GetValidEnumInput<CategoryType>("\nEnter category to filter by (Electronics / Food / Other): ");
            var filteredShortages = _shortagesController.GetShortagesByCategory(name, category);
            PrintShortagesTableToConsole(filteredShortages);
        }

        public void HandleFilterByRoom(string name)
        {
            var room = EnumValidators.GetValidEnumInput<RoomType>("\nEnter room to filter by (Meeting room / Kitchen / Bathroom): ");
            var filteredShortages = _shortagesController.GetShortagesByRoom(name, room);
            PrintShortagesTableToConsole(filteredShortages);
        }

        public void PrintShortagesTableToConsole(List<Shortage> shortages)
        {
            if (shortages == null || shortages.Count == 0)
            {
                Console.WriteLine("\nNo shortages to display!");
                return;
            }

            Console.WriteLine("");
            Console.WriteLine(new string('-', 105));
            Console.WriteLine($"| {"Title", -30} | {"Created By", -15} | {"Room", -12} | {"Category", -11} | {"Priority", -8} | {"Created on", -9} |");

            Console.WriteLine(new string('-', 105));

            for (int i = 0; i < shortages.Count; i++)
            {
                Console.WriteLine("| {0, -30} | {1, -15} | {2, -12} | {3, -11} | {4, -8} | {5, -9:yyyy-MM-dd} |",
                shortages[i].Title, shortages[i].Name, EnumHelper.GetEnumDescription(shortages[i].Room), EnumHelper.GetEnumDescription(shortages[i].Category), shortages[i].Priority, shortages[i].CreatedOn);
            }

            Console.WriteLine(new string('-', 105));
        }
    }
}

