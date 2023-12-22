using System;
using System.ComponentModel;

namespace Visma_task.Data.Entities.Enums
{
	public class EnumHelper
	{
        public static string GetEnumDescription<TEnum>(TEnum value) where TEnum : struct
        {
            var field = value.GetType().GetField(value.ToString());

            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                return attribute.Description ?? value.ToString() ?? "Unknown";
            }

            return value.ToString() ?? "Unknown";
        }

    }
}

