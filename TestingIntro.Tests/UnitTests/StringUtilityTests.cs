using System.Diagnostics;
using System.Globalization;
using TestingIntro.BLL.Utils;
using Xunit;
using Xunit.Abstractions;

namespace TestingIntro.Tests.UnitTests
{
    /// <summary>
    /// Alls kyns dæmi um próf
    /// </summary>
    public class StringUtilityTests
    {
        private readonly ITestOutputHelper _testOutputHelper; // Dependecy Injection gaur

        /// <summary>
        /// Smiður með "constructor injection"
        /// </summary>
        /// <param name="testOutputHelper"></param>
        public StringUtilityTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [Trait("Category", "U")]
        [InlineData("", "")]
        [InlineData("456716110", "456716110")] // < 10 stafir
        [InlineData("4567161102", "4567161102")]
        [InlineData("45671611020", "456716*1020")]
        [InlineData("456716110200", "456716**0200")]
        [InlineData("4567161102000", "456716***2000")]
        [InlineData("45671611020002", "456716****0002")]
        [InlineData("456716110200025", "456716*****0025")]
        [InlineData("4567161102000251", "456716******0251")]
        [InlineData("45671611020002519", "456716*******2519")]
        [InlineData("456716110200025199", "456716********5199")]
        [InlineData("4567161102000251999", "456716*********1999")]
        [InlineData("45671611020002519999", "********************")] // > 19 stafir
        public void Mask_CardNumber_As_Expected(string input, string expected)
        {
            Assert.Equal(expected, StringUtility.MaskCardNumber(input));
        }

        [Theory]
        [Trait("Category", "U")]
        [InlineData("123.459", 2, true, "123,46")]
        [InlineData("7,459", 2, false, "7.46")]
        [InlineData("123.459", 5, true, "123,45900")]
        [InlineData("8,459", 5, false, "8.45900")]
        [InlineData("9,759", 0, false, "10")]
        public void Format_Amount_As_Expected(string amount, int precision, bool separatorISK, string expected)
        {
            Assert.Equal(expected, StringUtility.FormatAmount(amount, precision, separatorISK));
        }

        [Theory]
        [Trait("Category", "T")]
        [InlineData("2010-01-15T00:00:00.000000", "yyyy-MM-ddTHH:mm:ss.ffffff")]
        [InlineData("2011-02-16 01.59.59.111111", "yyyy-MM-dd HH.mm.ss.ffffff")]
        [InlineData("2012-03-17-02.59.59.222222", "yyyy-MM-dd-HH.mm.ss.ffffff")]
        [InlineData("2013-04-18 03.59.59.333", "yyyy-MM-dd HH.mm.ss.fff")]
        [InlineData("2014-05-19T04:59.59", "yyyy-MM-ddTHH:mm.ss")]
        [InlineData("2015-06-20T05:59:59", "yyyy-MM-ddTHH:mm:ss")]
        [InlineData("21/07/2016 06:59:59.444444", "dd/MM/yyyy HH:mm:ss.ffffff")]
        [InlineData("22.08.2017 07:59:59.555555", "dd.MM.yyyy HH:mm:ss.ffffff")]
        [InlineData("23.09.2018 08:59:59.666", "dd.MM.yyyy HH:mm:ss.fff")]
        [InlineData("24.10.2019-09:59:59.777", "dd.MM.yyyy-HH:mm:ss.fff")]
        [InlineData("25.11.2020-10:59:59", "dd.MM.yyyy-HH:mm:ss")]
        [InlineData("18-10-2021 13.54.58,370704", "dd-MM-yyyy HH.mm.ss,ffffff")]
        public void Parse_String_DateTime_As_Expected(string input, string expectedFormat)
        {
            // Arrange
            string dateFormat = StringUtility.GetDateFormat(input);
            string expectedOutput = input;

            // Act
            DateTime dt = DateTime.ParseExact(input, dateFormat, CultureInfo.InvariantCulture);
            string output = dt.ToString(dateFormat, CultureInfo.InvariantCulture);

            // Assert
            Assert.Equal(expectedFormat, dateFormat);
            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        [Trait("Category", "T")]
        [Conditional("DEBUG")]
        public void Parse_String_DateTime_Performance()
        {
            // Arrange
            uint counter = 0;
            Stopwatch stopwatch = new();
            String[] arrayRawDates = {  "2010-01-15T00:00:00.000000",
                                        "2011-02-16 01.59.59.111111",
                                        "2012-03-17-02.59.59.222222",
                                        "2013-04-18 03.59.59.333",
                                        "2014-05-19T04:59.59",
                                        "2015-06-20T05:59:59",
                                        "21/07/2016 06:59:59.444444",
                                        "22.08.2017 07:59:59.555555",
                                        "23.09.2018 08:59:59.666",
                                        "24.10.2019-09:59:59.777",
                                        "25.11.2020-10:59:59" };

            // Act
            stopwatch.Start();
            for (int i = 0; i < 10000; i++)
            {
                foreach (var rawDate in arrayRawDates)
                {
                    DateTime dt = StringUtility.ParseDate(rawDate);
                    counter++;
                }
            }
            stopwatch.Stop();

            decimal actual = (counter / (decimal)stopwatch.ElapsedTicks) * 1000; // Average response time in µs (10^-6 seconds).

            // Assert
            _testOutputHelper.WriteLine($"Number of samples = {counter:N0}, average response time = {actual:F3} [µs]");
            Assert.InRange<decimal>(actual, 0.01m, 20.0m);
        }

        [Fact]
        [Trait("Category", "T")]
        public void Format_Json_As_Expected()
        {
            // Arrange
            string json = "{ \"firstName\": \"John\", \"lastName\": \"Smith\", \"isAlive\": true, \"age\": 27 }";

            // Act
            string jsonFormatted = StringUtility.BeautifyJson(json);
            int lines = jsonFormatted.Split(new char[] { '\n' }).Count();

            // Assert
            Assert.Equal(6, lines);
        }

        [Theory]
        [Trait("Category", "T")]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("7865 787", false)]
        [InlineData("0123456789", true)]
        [InlineData("01234567891", false, 10)]
        public void IsNumeric_As_Expected(string input, bool expected, int len = 0)
        {
            // Arrange/Act
            bool actual = StringUtility.IsNumeric(input, len);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [Trait("Category", "T")]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("UIOY7dhg", false)]
        [InlineData("abcdefghij", true)]
        [InlineData("ABCDEFGHIJL", false, 10)]
        public void IsAlphabetic_As_Expected(string input, bool expected, int len = 0)
        {
            // Arrange/Act
            bool actual = StringUtility.IsAlphabetic(input, len);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [Trait("Category", "T")]
        [InlineData("123,45", 123.45)]
        [InlineData("456.789", 456.789)]
        public void Convert_String_Amount_To_Decimal(string input, decimal expected)
        {
            // Arrange/Act
            decimal actual = StringUtility.GetAmount(input);

            // Assert
            Assert.Equal(expected, actual);
        }

    }
}