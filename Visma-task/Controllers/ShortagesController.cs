using System;
using System.Xml.Linq;
using Visma_task.Data.Entities;
using Visma_task.Data.Entities.Enums;
using Visma_task.Repositories;

namespace Visma_task.Controllers
{
	public class ShortagesController
	{
        private IShortagesRepository _shortagesRepository;

        public ShortagesController(IShortagesRepository shortageRepository)
        {
            this._shortagesRepository = shortageRepository;
        }

        public List<Shortage> GetShortages(string name)
        {
            if (isAdmin(name))
            {
                return _shortagesRepository.GetShortages(name);
            }
            return _shortagesRepository.GetShortages(name).Where(s => s.Name == name).ToList();
        }

        public List<Shortage> GetShortagesByTitle(string name, string titleFilter)
        {
            if (!isAdmin(name))
            {
                return _shortagesRepository.GetShortagesByTitle(titleFilter).Where(s => s.Name == name).ToList();
            }    
            return _shortagesRepository.GetShortagesByTitle(titleFilter);
        }

        public List<Shortage> GetShortagesByCreatedOn(string name, DateOnly startDate, DateOnly endDate)
        {
            if (!isAdmin(name))
            {
                return _shortagesRepository.GetShortagesByCreatedOn(startDate, endDate).Where(s => s.Name == name).ToList();
            }
            return _shortagesRepository.GetShortagesByCreatedOn(startDate, endDate);
        }

        public List<Shortage> GetShortagesByCategory(string name, CategoryType categoryFilter)
        {
            if (!isAdmin(name))
            {
                return _shortagesRepository.GetShortagesByCategory(categoryFilter).Where(s => s.Name == name).ToList();
            }
            return _shortagesRepository.GetShortagesByCategory(categoryFilter);
        }

        public List<Shortage> GetShortagesByRoom(string name, RoomType roomFilter)
        {
            if (!isAdmin(name))
            {
                return _shortagesRepository.GetShortagesByRoom(roomFilter).Where(s => s.Name == name).ToList();
            }
            return _shortagesRepository.GetShortagesByRoom(roomFilter);
        }

        public void AddShortage(Shortage shortage)
        {
            var existingShortage = _shortagesRepository.GetShortageByTitleAndRoom(shortage.Title, shortage.Room);

            if (existingShortage != null)
            {
                if(shortage.Priority > existingShortage.Priority)
                {
                    _shortagesRepository.UpdateShortage(existingShortage, shortage);
                    return;
                }

                Console.WriteLine("\nShortage already exists!");
                return;
            }
            
            _shortagesRepository.AddShortage(shortage);
            Console.WriteLine("\nShortage successfully added!");
        }

        public void UpdateShortage(Shortage existingShortage, Shortage newShortage)
        {
            _shortagesRepository.UpdateShortage(existingShortage, newShortage);
            Console.WriteLine("\nShortage successfully updated!");
        }

        public void DeleteShortage(string name, string title, RoomType room)
        {
            var shortageToDelete = _shortagesRepository.GetShortageByTitleAndRoom(title, room);

            if (shortageToDelete == null)
            {
                Console.WriteLine("\nError! Could not find required shortage!"); ;
                return;
            }

            if (!isAdmin(name) && name != shortageToDelete.Name)
            {
                Console.WriteLine("\nError! You do not have the permission " +
                    "to delete this shortage!"); ;
                return;
            }

            _shortagesRepository.DeleteShortage(shortageToDelete);
            Console.WriteLine("\nShortage successfully deleted!");
        }

        public bool isAdmin(string name)
        {
            if(name != "Admin")
            {
                return false;
            }
            return true;
        }
    }
}

