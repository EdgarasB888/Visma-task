using System;
using Visma_task.Data.Entities.Enums;

namespace Visma_task.Data.Validations
{
    public class EnumValidators
    {
        public static bool TryParseEnum<TEnum>(string input, out TEnum result) where TEnum : struct
        {
            result = default(TEnum);

            foreach (var value in Enum.GetValues(typeof(TEnum)))
            {
                if (value is TEnum enumValue)
                {
                    string description = EnumHelper.GetEnumDescription(enumValue);

                    if (string.Equals(description, input, StringComparison.OrdinalIgnoreCase))
                    {
                        result = enumValue;
                        return true;
                    }
                }
            }

            return false;
        }

        public static TEnum GetValidEnumInput<TEnum>(string promptMessage) where TEnum : struct
        {
            while (true)
            {
                Console.Write(promptMessage);
                string? input = Console.ReadLine()/*.Trim()*/;

                if (TryParseEnum(input, out TEnum result))
                {
                    return result;
                }

                Console.WriteLine($"Invalid input! Please enter a valid {typeof(TEnum).Name}. Available values: {string.Join(", ", Enum.GetNames(typeof(TEnum)))}");
            }
        }
    }
}

