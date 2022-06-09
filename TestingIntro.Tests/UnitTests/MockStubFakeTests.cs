using Moq;
using TestingIntro.Tests.MockStubFake;
using Xunit;
using Xunit.Abstractions;

namespace TestingIntro.Tests.UnitTests
{
    public class MockStubFakeTests
    {
        private readonly ITestOutputHelper _testOutputHelper; // Dependecy Injection gaur

        public MockStubFakeTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        [Trait("Category", "T")]
        public void Should_Throw_NotImplementedException()
        {
            // Arrange
            ICurrencies cur = new Currencies();

            // Act/Assert
            var ex = Assert.Throws<NotImplementedException>(() => cur.GetCode("352"));
            Assert.Equal("The method or operation is not implemented.", ex.Message);
        }

        [Fact]
        [Trait("Category", "T")]
        public void Get_Mock_CurrencyCode_Returns_ISK()
        {
            // Arrange
            var curMock = new Mock<ICurrencies>();
            curMock.Setup(x => x.GetCode("352")).Returns("ISK");

            // Act
            var actual = curMock.Object.GetCode("352");

            // Assert
            Assert.Equal(expected: "ISK", actual);
        }


        [Fact]
        [Trait("Category", "T")]
        public void Get_Stub_CurrencyCode_Returns_ISK()
        {
            // Arrange
            ICurrencies cur = new CurrenciesStub();

            // Act
            var actual = cur.GetCode("352");

            // Assert
            Assert.Equal(expected: "ISK", actual);
        }

        [Fact]
        [Trait("Category", "T")]
        public void Get_Fake_CurrencyCode_Returns_USD()
        {
            // Assert
            ICurrencies cur = new CurrenciesFake();

            // Act
            var actual = cur.GetCode("840");

            // Assert
            Assert.Equal(expected: "USD", actual);
        }

        [Fact]
        [Trait("Category", "T")]
        public void Get_Fake_CurrencyCode_Returns_AED_ZWL()
        {
            // Arrange
            ICurrencies cur = new CurrenciesFake();

            // Act
            var actual = cur.GetCodes();
            _testOutputHelper.WriteLine($"Fjöldi mynta = {actual.Count}");

            // Assert
            Assert.True(actual.Count > 0);
            Assert.Equal(expected: "AED", actual.First());
            Assert.Equal(expected: "ZWL", actual.Last());
        }
    }
}
