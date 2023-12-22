using Visma_task.Controllers;
using Visma_task.Repositories;
using Visma_task.UI;

namespace Visma_task;

class Program
{
    static void Main(string[] args)
    {
        var shortagesRepository = new ShortagesRepository("Shortages.json");
        var shortagesController = new ShortagesController(shortagesRepository);

        var consoleUIManager = new ConsoleUIManager(shortagesController);
        consoleUIManager.HandleUI();
    }
}