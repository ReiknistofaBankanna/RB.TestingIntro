using TestingIntro.BLL.Entities;
using TestingIntro.BLL.Utils;

namespace TestingIntro
{
    class Program
    {
        public static void Main(string[] args)
        {
            DisplayTokenLoop();
        }

        public static void DisplayTokenLoop()
        {
            ConsoleKeyInfo input;
            string banki = "Landsbankinn";
            do
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Press any key to regenerate, ESC to quit.");
                Console.WriteLine("Press number for mainbank: 1,3,5.\n");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Random ExternalId = {RandomUtility.RandomString(15, RandomStringType.Numeric)}");
                Console.WriteLine($"Random Amount     = {RandomUtility.RandomAmount(100, 1000)}");
                Console.WriteLine($"Random Date       = {RandomUtility.RandomDateTime(new DateTime(1970, 12, 3))}");
                Console.WriteLine($"Random Type       = {RandomUtility.GetRandomEnumValue<RandomStringType>()}\n");

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"Banki = \"{banki}\"");

                input = Console.ReadKey();
                switch (input.KeyChar)
                {
                    case '1': banki = "Landsbankinn"; break;
                    case '3': banki = "Arion banki"; break;
                    case '5': banki = "Íslandsbanki"; break;
                }

                Console.Clear();
            } while (input.Key != ConsoleKey.Escape);
        }
    }
}