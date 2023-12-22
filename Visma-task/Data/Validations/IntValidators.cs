using System;
namespace Visma_task.Data.Validations
{
    public static class IntValidators
    {
        public static bool IsValidPriority(string input, out int priority)
        {
            return int.TryParse(input, out priority) && priority >= 1 && priority <= 10;
        }

        public static int GetValidPriorityInput(string promptMessage)
        {
            while (true)
            {
                Console.Write(promptMessage);
                string? input = Console.ReadLine();

                if (IsValidPriority(input, out int priority))
                {
                    return priority;
                }

                Console.WriteLine("Invalid priority. Priority must be a number between 1 and 10.");
            }
        }
    }
}

