using System.Diagnostics;
using System.Text;
using TestingIntro.BLL.Entities;
using TestingIntro.BLL.Utils;
using Xunit;
using Xunit.Abstractions;

namespace TestingIntro.Tests.UnitTests
{
    /// <summary>
    /// Alls kyns dæmi um próf
    /// </summary>
    public class RandomUtilityTests
    {
        private readonly ITestOutputHelper _testOutputHelper; // Dependecy Injection gaur

        /// <summary>
        /// Smiður með "constructor injection"
        /// </summary>
        /// <param name="testOutputHelper"></param>
        public RandomUtilityTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [Trait("Category", "U")]
        [InlineData("03.12.1970 08:00:00", "03.12.1970 10:00:00")]
        [InlineData("03.12.1970 08:00:00", null)]
        [InlineData("07.06.2022 00:00:00", "07.06.2022 23:59:59")]
        public void Get_Random_DateTime_Interval(string start, string end)
        {
            // Arrange
            DateTime dtStart = StringUtility.ParseDate(start);
            DateTime dtEnd = (end == null ? DateTime.Now : StringUtility.ParseDate(end));

            // Act
            DateTime dtRandom = RandomUtility.RandomDateTime(dtStart, dtEnd);
            _testOutputHelper.WriteLine($"{dtStart:dd.MM.yyyy HH:mm:ss} < {dtRandom:dd.MM.yyyy HH:mm:ss} < {dtEnd:dd.MM.yyyy HH:mm:ss}");

            // Assert
            Assert.True(dtStart < dtRandom);
            Assert.True(dtRandom < dtEnd);
        }

        [Fact]
        [Trait("Category", "U")]
        [Conditional("DEBUG")]
        public void Get_Random_Enum_Teljari_Should_Hit_All()
        {
            // Arrange
            var fjoldi = Enum.GetValues(typeof(TeljariType)).Length;
            var pottur = new HashSet<TeljariType>();
            int round = 0;

            // Act
            for (; round < 100; round++)
            {
                var teljari = RandomUtility.GetRandomEnumValue<TeljariType>();
                pottur.Add(teljari);
                if ((pottur.Count * 100 / fjoldi) == 100)
                {
                    var sb = new StringBuilder();
                    foreach (TeljariType p in pottur)
                    {
                        sb.Append(p.ToString() +  ", ");
                    }
                    _testOutputHelper.WriteLine(sb.ToString());
                    break;
                }
            }

            _testOutputHelper.WriteLine($"100% coverage in {round} rounds");

            // Assert
            Assert.Equal(pottur.Count, fjoldi);
        }

        [Theory]
        [InlineData(20, RandomStringType.Alphabetic)]
        [InlineData(30, RandomStringType.AlphaNum)]
        [InlineData(40, RandomStringType.SpacedAlphabetic)]
        [InlineData(50, RandomStringType.Numeric)]
        [Trait("Category", "U")]
        public void Get_Random_String_Should_Span(int len, RandomStringType type)
        {
            // Act
            var genString = RandomUtility.RandomString(len, type);
            _testOutputHelper.WriteLine($"{type.ToString()} -> {genString}");

            // Assert
            Assert.True(genString.Length == len);
        }

        [Theory]
        [InlineData(-10000, -1000)]
        [InlineData(250000, 300000)]
        [InlineData(1000, 5000, 2)]
        [InlineData(3, 4, 6, 10)]
        [InlineData(1, -1, 3, 10)]
        [Trait("Category", "U")]
        public void Get_Random_Amount_Interval(int x1, int x2, int precision = 0, int rounds = 1)
        {
            for (int i=0; i<rounds; i++)
            {
                // Act
                decimal amount = RandomUtility.RandomAmount(ref x1, ref x2, precision);
                _testOutputHelper.WriteLine($"{x1} < {amount} < {x2}");

                // Assert
                Assert.True(x1 < amount);
                Assert.True(amount < x2);
            }
        }

        [Theory]
        [InlineData(10, 100, 100)]
        [Trait("Category", "U")]
        public void Swap_Any_Type_As_Expected(int x1, int x2, int expected)
        {
            // short
            {
                short t1 = (short) x1;
                short t2 = (short) x2;
                RandomUtility.Swap(ref t1, ref t2);
                Assert.Equal(t1, expected);
            }
            // int
            {
                int t1 = x1;
                int t2 = x2;
                RandomUtility.Swap(ref t1, ref t2);
                Assert.Equal(t1, expected);
            }
            // long
            {
                long t1 = x1;
                long t2 = x2;
                RandomUtility.Swap(ref t1, ref t2);
                Assert.Equal(t1, expected);
            }
            // float
            {
                float t1 = x1;
                float t2 = x2;
                RandomUtility.Swap(ref t1, ref t2);
                Assert.Equal(t1, expected);
            }
            // double
            {
                double t1 = x1;
                double t2 = x2;
                RandomUtility.Swap(ref t1, ref t2);
                Assert.Equal(t1, expected);
            }
            // decimal
            {
                decimal t1 = x1;
                decimal t2 = x2;
                RandomUtility.Swap(ref t1, ref t2);
                Assert.Equal(t1, expected);
            }
        }
    }
}