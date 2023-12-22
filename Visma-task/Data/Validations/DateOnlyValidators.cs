using System;
namespace Visma_task.Data.Validations
{
    public static class DateOnlyValidators
    {
        public static bool IsValidDateOnly(string input, out DateOnly date)
        {
            return DateOnly.TryParse(input, out date);
        }

        public static DateOnly GetValidDateOnlyInput(string promptMessage)
        {
            while (true)
            {
                Console.Write(promptMessage);
                string? input = Console.ReadLine();

                if (IsValidDateOnly(input, out DateOnly date))
                {
                    return date;
                }
                
                Console.WriteLine("Invalid date format. Please enter a valid date in the format yyyy-MM-dd.");
            }
        }
    }
}

