using System.Globalization;
using System.Text;
using TestingIntro.BLL.Entities;

namespace TestingIntro.BLL.Utils
{
    public static class RandomUtility
    {
        private static readonly Random s_random = new(Environment.TickCount);

        /// <summary>
        /// Býr til handahófskendar dagsetningar
        /// </summary>
        /// <param name="start">Byrjunar dagsetning</param>
        /// <param name="end">Loka dagsetning, default now</param>
        /// <returns>Random dags</returns>
        public static DateTime RandomDateTime(DateTime start, DateTime? end = null)
        {
            end ??= DateTime.Now;

            long range = end.Value.Ticks - start.Ticks;
            long randomTicks = s_random.NextInt64(range) + start.Ticks;

            return new DateTime(randomTicks);
        }

        /// <summary>
        /// Skilar handahófskenndu enum gildi.
        /// Notkun: var option = StringUtility.GetRandomEnumValue<SomeEnum>();
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static T GetRandomEnumValue<T>()
        {
            if (!typeof(T).IsEnum)
            {
                throw new InvalidOperationException("Must use Enum type");
            }

            Array enumValues = Enum.GetValues(typeof(T));
            return (T)enumValues.GetValue(s_random.Next(enumValues.Length));
        }

        /// <summary>
        /// Býr til random streng.
        /// Færum þetta til í UnitTests BLLinn reiðir sig á alvöru gögn.
        /// </summary>
        /// <param name="len"></param>
        /// <param name="bNumeric"></param>
        /// <returns></returns>
        public static string RandomString(int len, RandomStringType type)
        {
            const string space = "   ";
            string AB = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ" + space;
            StringBuilder sb = new(len);
            for (int i = 0; i < len; i++)
            {
                if (type == RandomStringType.Numeric)
                {
                    // x er í 0-9
                    sb.Append(s_random.Next(10));
                }
                else if (type == RandomStringType.Alphabetic)
                {
                    // x er í A-Z
                    sb.Append(AB[10 + s_random.Next(AB.Length - 10 - space.Length)]);
                }
                else if (type == RandomStringType.SpacedAlphabetic)
                {
                    // x er stafur til að byrja með, annars A-Z eða bil
                    if (i > 0)
                    {
                        sb.Append(AB[10 + s_random.Next(AB.Length - 10)]);
                    }
                    else
                    {
                        sb.Append(AB[10 + s_random.Next(AB.Length - 10 - space.Length)]);
                    }
                }
                else if (type == RandomStringType.AlphaNum)
                {
                    // x er í 0-9 eða A-Z
                    sb.Append(AB[s_random.Next(AB.Length - space.Length)]);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Swaps values using a tuple.
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        public static void Swap<T>(ref T x1, ref T x2)
        {
            (x1, x2) = (x2, x1);
        }

        /// <summary>
        /// Sama fall og að neðan nema án tilvísunar.
        /// </summary>
        /// <param name="x1">Lægra mark tölu sem á að búa til.</param>
        /// <param name="x2">Efra mark tölu sem á að búa til.</param>
        /// <param name="precision">Fjöldi aukastafa.</param>
        /// <returns></returns>
        public static decimal RandomAmount(int x1, int x2, int precision = 0)
        {
            return RandomAmount(ref x1, ref x2, precision);
        }


        /// <summary>
        /// Skilar handahófskenndri upphæð á decimal formi. Svissar tölum ef með þarf.
        /// Til að snara yfir á streng má nota temp.ToString(new CultureInfo("en-US")
        /// </summary>
        /// <param name="x1">Lægra mark tölu sem á að búa til.</param>
        /// <param name="x2">Efra mark tölu sem á að búa til.</param>
        /// <param name="precision">Fjöldi aukastafa.</param>
        /// <returns></returns>
        public static decimal RandomAmount(ref int x1, ref int x2, int precision = 0)
        {
            if (x2 < x1)
            {
                Swap(ref x1, ref x2);
            }
            
            int randomValue = s_random.Next(x1, x2);
            
            if (precision > 0 && 
                int.TryParse("".PadRight(precision, '9'), out int maxvalue) &&
                decimal.TryParse("1".PadRight(precision+1, '0'), out decimal divider))
            {
                decimal result = randomValue + (s_random.Next(1, maxvalue) / divider);
                return result;
                // return decimal.Round(result, precision, MidpointRounding.AwayFromZero);
            }
            else
            {
                return randomValue;
            }
        }
    }
}