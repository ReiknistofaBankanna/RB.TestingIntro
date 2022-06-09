using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace TestingIntro.BLL.Utils
{
    public class StringUtility{
        /// <summary>
        /// Parses datetime input string by regular expression and builds
        /// proper format string for DateTime.ParseExact function.
        /// </summary>
        /// <param name="rawDate"></param>
        /// <returns></returns>
        public static string GetDateFormat(String rawDate)
        {
            Regex r = new(@"^([0-9]{2,4})([-/\.])([0-9]{2})([-/\.])([0-9]{2,4})([T\- ])([0-9]{2})([:\.])([0-9]{2})([:\.])([0-9]{2})([\.,][0-9]{3,6})?$");
            string dateFormat = "";

            if (r.IsMatch(rawDate))
            {
                string[] groups = r.Split(rawDate);
                if (groups.Length >= 11)
                {
                    StringBuilder sb = new();

                    sb.Append(groups[1].Length == 2 ? "dd" : "yyyy");
                    sb.Append(groups[2]);
                    sb.Append("MM");
                    sb.Append(groups[4]);
                    sb.Append(groups[5].Length == 2 ? "dd" : "yyyy");
                    sb.Append(groups[6]);
                    sb.Append("HH");
                    sb.Append(groups[8]);
                    sb.Append("mm");
                    sb.Append(groups[10]);
                    sb.Append("ss");
                    if (String.IsNullOrEmpty(groups[12]) == false)
                    {
                        sb.Append(groups[12][0]);
                        sb.Append("".PadRight(groups[12].Length - 1, 'f'));
                    }

                    dateFormat = sb.ToString();
                }
            }

            return dateFormat;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string BeautifyJson(string json)
        {
            string result = json;
            try
            {
                using JsonDocument document = JsonDocument.Parse(json);
                using var stream = new MemoryStream();
                using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions() { Indented = true });
                document.WriteTo(writer);
                writer.Flush();
                result = Encoding.UTF8.GetString(stream.ToArray());
            }
            catch (Exception) { }

            return result;
        }

        /// <summary>
        /// Checks whether text is numeric or not
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumeric(string input, int len = 0)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            else
            {
                if (len > 0 && input.Length != len)
                {
                    return false;
                }

                foreach (char c in input)
                {
                    if ((c >= '0' && c <= '9') == false)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Checks whether text is numeric or not
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsAlphabetic(string input, int len = 0)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            else
            {
                if (len > 0 && input.Length != len)
                {
                    return false;
                }

                input = input.ToUpper();
                foreach (char c in input)
                {
                    if ((c >= 'A' && c <= 'Z') == false)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        /// Hide cardnumber with Mask
        /// </summary>
        /// <param name="CardNumber">The original card number</param>
        /// <returns>string</returns>
        public static string MaskCardNumber(String cardNumber)
        {
            cardNumber = (cardNumber != null) ? cardNumber.Trim() : "";

            string sOut;
            if (cardNumber.Length == 16)
            {
                sOut = cardNumber.Substring(0, 6) + "******" + cardNumber.Substring(12, 4);
            }
            else
            {
                if (cardNumber.Length >= 10 && cardNumber.Length <= 19)
                {
                    sOut = cardNumber.Substring(0, 6) + "**********".Substring(0, cardNumber.Length - 10) + cardNumber.Substring(cardNumber.Length - 4, 4);
                }
                else if (cardNumber.Length > 19)
                {
                    // Þetta kemur aldrei upp, allt kortanúmerið ruglað.
                    sOut = new String('*', cardNumber.Length);
                }
                else
                {
                    // Ómarktækt númer sem er búið að stytta
                    sOut = cardNumber;
                }
            }

            return sOut;
        }

        /// <summary>
        /// Convert amount to proper format using appropriate decimal point and number of places.
        /// </summary>
        /// <param name="amount">Any number with or without decimal point</param>
        /// <param name="precision">Defines the precision of fraction</param>
        /// <param name="separatorISK">If true then comma else dot</param>
        /// <returns></returns>
        public static string FormatAmount(string amount, int precision = 3, bool separatorISK = true, bool absoluteValue = false)
        {
            if (String.IsNullOrEmpty(amount) == false)
            {
                string currentSeparator = amount.Contains(',') ? "," : ".";
                if (Decimal.TryParse(amount, NumberStyles.Any, new NumberFormatInfo() { NumberDecimalSeparator = currentSeparator }, out decimal temp))
                {
                    if (absoluteValue)
                    {
                        temp = Math.Abs(temp);
                    }
                    return temp.ToString($"F{precision}", new CultureInfo(separatorISK ? "is-IS" : "en-US"));
                }
            }
            return amount;
        }

        public static decimal GetAmount(string amount)
        {
            if (String.IsNullOrEmpty(amount) == false)
            {
                string currentSeparator = amount.Contains(',') ? "," : ".";
                if (Decimal.TryParse(amount, NumberStyles.Any, new NumberFormatInfo() { NumberDecimalSeparator = currentSeparator }, out decimal temp))
                {
                    return temp;
                }
            }
            return 0m;
        }

        /// <summary>
        /// Parses datetime string into DateTime object.
        /// </summary>
        /// <param name="rawDate"></param>
        /// <returns></returns>
        public static DateTime ParseDate(String rawDate)
        {
            string dateFormat = StringUtility.GetDateFormat(rawDate);
            return DateTime.ParseExact(rawDate, dateFormat, CultureInfo.InvariantCulture);
        }
    }
}