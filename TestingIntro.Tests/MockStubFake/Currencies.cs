/*
 * Purpose of this class is to mimic the real world.
 * Instead of implement database interactivity we throw error.
 * In the unit test we have to use a mock to prevent db interactivity.
 * 
 * What Is A Mock?
 * 
 * A mock is a pre-programmed object that can have dynamic responses/behaviour 
 * defined as part of the test. They do not need to be concrete in implementation 
 * and (generally) don’t need behaviour to be shared amongst tests.
 */
using TestingIntro.BLL.Utils;

namespace TestingIntro.Tests.MockStubFake
{
	public class Currencies : ICurrencies
	{
		public string GetCode(string numericCode)
		{
            // Go to the database and fetch currency. 
            throw new NotImplementedException();
        }

        public List<string> GetCodes()
        {
			// Go to the database and fetch all currencies. 
			throw new NotImplementedException();
        }

        public bool Insert(string alphaCode, string numericCode)
        {
			bool success = false;
			try
			{
				/*
				 * Pseudo code (i. sauðakóði)
				 * --------------------------
				 * 1. Error check input parameters
				 *    a. Check for null and correct length.
				 *    b. Validate for alphabetic and numeric style.
				 *    c. Throw error if necessary.
				 * 2. Lookup for the value in the database.
				 * 3. If lookup fails we can insert into the database.
				 * 4. TODO: Database error handling
				 */

				if (!StringUtility.IsAlphabetic(alphaCode, 3) || !StringUtility.IsNumeric(numericCode, 3))
				{
					throw new ArgumentNullException($"Villa í inntaki: Insert({alphaCode}, string {numericCode})");
				}

				// Insert a currency into the database here.
				throw new NotImplementedException();
			}
			catch (Exception) { }

			return success;
        }
	}
}