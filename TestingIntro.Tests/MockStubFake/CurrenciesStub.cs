/*
 * What Is A Stub?
 * 
 * A stub is an implementation that returns hardcoded responses, 
 * but does not have any “smarts” to it. There is no tying together 
 * of calls on the object, instead each method just returns a 
 * pre-defined canned response.
 */

namespace TestingIntro.Tests.MockStubFake
{
	public class CurrenciesStub : ICurrencies
	{
		public string GetCode(string numericCode)
		{
			return "ISK";
		}

        public List<string> GetCodes()
        {
            return new List<string>() { "AED", "AFN", "ALL", "AMD", "ANG", "AOA", "ARS", "ATS", "AUD" };
        }

        public bool Insert(string alphaCode, string numericCode)
        {
            return true;
        }
    }
}
