using System;
using System.Collections.Generic;

namespace EventVenueBookingManager
{
    // Small collection of console input helpers, kept separate from Program.cs
    // so the menu flow there is cleaner and isn't buried under parsing loops
    public static class Helper
    {
        public static string ReadNonEmptyString(string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt}: ");
                var input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                    return input.Trim();

                Console.WriteLine("This field cannot be empty. Please try again.");
            }
        }

        public static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt}: ");
                if (int.TryParse(Console.ReadLine(), out var value))
                    return value;

                Console.WriteLine("Please enter a whole number.");
            }
        }

        public static double ReadDouble(string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt}: ");
                if (double.TryParse(Console.ReadLine(), out var value))
                    return value;

                Console.WriteLine("Please enter a number.");
            }
        }

        public static DateTime ReadDateTime(string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt} (yyyy-mm-dd hh:mm): ");
                if (DateTime.TryParse(Console.ReadLine(), out var value))
                    return value;

                Console.WriteLine("Please use the format yyyy-mm-dd hh:mm.");
            }
        }

        // Lists every value of an enum and lets the user pick one
        public static T ChooseEnum<T>(string prompt) where T : struct, Enum
        {
            var values = Enum.GetValues<T>();

            Console.WriteLine(prompt);
            for (int i = 0; i < values.Length; i++)
                Console.WriteLine($"  {i + 1}. {values[i]}");

            var index = ReadInt("Choose an option") - 1;
            while (index < 0 || index >= values.Length)
            {
                Console.WriteLine("Invalid choice, try again.");
                index = ReadInt("Choose an option") - 1;
            }

            return values[index];
        }

        // Lists items and lets the user pick one
        // Returns null if there was nothing to choose from
        public static T? ChooseFromList<T>(string prompt, IReadOnlyList<T> items) where T : class
        {
            if (items.Count == 0)
            {
                Console.WriteLine("Currently no available option, going back.");
                return null;
            }

            Console.WriteLine(prompt);
            for (int i = 0; i < items.Count; i++)
                Console.WriteLine($"  {i + 1}. {items[i]}");

            var index = ReadInt("Choose an option") - 1;
            while (index < 0 || index >= items.Count)
            {
                Console.WriteLine("Invalid choice, try again.");
                index = ReadInt("Choose an option") - 1;
            }

            return items[index];
        }
    }
}
