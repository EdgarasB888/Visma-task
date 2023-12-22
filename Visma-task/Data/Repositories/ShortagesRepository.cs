using System;
using Newtonsoft.Json;
using Visma_task.Data.Entities;
using Visma_task.Data.Entities.Enums;

namespace Visma_task.Repositories
{
    public interface IShortagesRepository
    {
        List<Shortage> GetShortages(string name);
        List<Shortage> GetShortagesByTitle(string title);
        List<Shortage> GetShortagesByCreatedOn(DateOnly startDate, DateOnly endDate);
        List<Shortage> GetShortagesByCategory(CategoryType category);
        List<Shortage> GetShortagesByRoom(RoomType room);
        List<Shortage> GetInternalShortagesListForTesting();
        Shortage GetShortageByTitleAndRoom(string title, RoomType room);
        void AddShortage(Shortage shortage);
        void UpdateShortage(Shortage existingShortage, Shortage newShortage);
        void DeleteShortage(Shortage shortage);
    }

	public class ShortagesRepository : IShortagesRepository
	{
        private List<Shortage> _shortages = new List<Shortage>();
        private string _filePath;

        public ShortagesRepository(string filePath)
        {
            _filePath = filePath;
            LoadShortages();
        }

        public void LoadShortages()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _shortages = JsonConvert.DeserializeObject<List<Shortage>>(json) ?? new List<Shortage>();
            }
            
        }

        public void SaveShortages(List<Shortage> shortages)
        {
            var json = JsonConvert.SerializeObject(shortages, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        public List<Shortage> GetShortages(string name)
        {
            return _shortages.OrderByDescending(s => s.Priority).ToList();
        }

        public List<Shortage> GetShortagesByTitle(string title)
        {
            return _shortages
                .Where(s => s.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(s => s.Priority)
                .ToList();
        }

        public List<Shortage> GetShortagesByCreatedOn(DateOnly startDate, DateOnly endDate)
        {
            return _shortages
                .Where(s => s.CreatedOn >= startDate && s.CreatedOn <= endDate)
                .OrderByDescending(s => s.Priority)
                .ToList();
        }

        public List<Shortage> GetShortagesByCategory(CategoryType category)
        {
            return _shortages
                .Where(s => s.Category == category)
                .OrderByDescending(s => s.Priority)
                .ToList();
        }

        public List<Shortage> GetShortagesByRoom(RoomType room)
        {
            return _shortages
                .Where(s => s.Room == room)
                .OrderByDescending(s => s.Priority)
                .ToList();
        }

        public List<Shortage> GetInternalShortagesListForTesting()
        {
            return _shortages;
        }

        public Shortage GetShortageByTitleAndRoom(string title, RoomType room)
        {
            var result = _shortages.FirstOrDefault(s =>
            string.Equals(s.Title, title, StringComparison.OrdinalIgnoreCase)
            && s.Room == room);

            if (result == null)
            {
                throw new InvalidOperationException("No matching shortage found.");
            }

            return result;
        }

        public void AddShortage(Shortage shortage)
        {
            _shortages.Add(shortage);
            SaveShortages(_shortages);
        }

        public void UpdateShortage(Shortage existingShortage, Shortage newShortage)
        {
            var index = _shortages.IndexOf(existingShortage);

            if (index != -1)
            {
                _shortages[index] = newShortage;
                SaveShortages(_shortages);
            }
        }

        public void DeleteShortage(Shortage shortage)
        {
            _shortages.Remove(shortage);
            SaveShortages(_shortages);
        }
    }
}

