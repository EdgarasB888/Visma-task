using System;
namespace Visma_task.Data.Validations
{
    public static class StringValidators
    {
        public static bool IsNullOrEmpty(string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNullOrWhiteSpace(string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static string GetValidStringInput(string promptMessage, Func<string, bool> validationFunc)
        {
            while (true)
            {
                Console.Write(promptMessage);
                string input = Console.ReadLine()?.Trim();

                if (IsNullOrEmpty(input) || validationFunc(input))
                {
                    Console.WriteLine("Invalid input! Please enter a valid value.");
                }
                else
                {
                    return input;
                }
            }
        }
    }
}

