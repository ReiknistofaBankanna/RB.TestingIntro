namespace TestingIntro.Tests.MockStubFake
{
	/// <summary>
	/// https://www.iso.org/iso-4217-currency-codes.html
	/// </summary>
	public interface ICurrencies
	{
		/// <summary>
		/// Lookup for numeric currency code.
		/// </summary>
		/// <param name="numericCode"></param>
		/// <returns>Three digit alpha code</returns>
		string GetCode(string numericCode);

		/// <summary>
		/// Return all currency codes.
		/// </summary>
		/// <returns>Three digit alpha code</returns>
		List<string> GetCodes();

		/// <summary>
		/// Insert new currency code into table.
		/// </summary>
		/// <param name="alphaCode"></param>
		/// <param name="numericCode"></param>
		/// <returns>true if success</returns>
		bool Insert(string alphaCode, string numericCode);
	}
}
