﻿/*
 * What Is A Fake?
 * 
 * A fake is an object used in place of a concrete implementation 
 * that has some “smarts” to it. Usually a shortcut implementation 
 * that makes it useful across different unit tests, but stops short 
 * of being an integration test. 
 */

using TestingIntro.BLL.Utils;

namespace TestingIntro.Tests.MockStubFake
{
	public class CurrenciesFake : ICurrencies
	{
		private static readonly Dictionary<string, string> dict = new()
		{
			{ "008", "ALL" },
			{ "012", "DZD" },
			{ "032", "ARS" },
			{ "036", "AUD" },
			{ "040", "ATS" },
			{ "044", "BSD" },
			{ "048", "BHD" },
			{ "050", "BDT" },
			{ "051", "AMD" },
			{ "052", "BBD" },
			{ "056", "BEF" },
			{ "060", "BMD" },
			{ "064", "BTN" },
			{ "068", "BOB" },
			{ "072", "BWP" },
			{ "084", "BZD" },
			{ "090", "SBD" },
			{ "096", "BND" },
			{ "104", "MMK" },
			{ "108", "BIF" },
			{ "116", "KHR" },
			{ "124", "CAD" },
			{ "132", "CVE" },
			{ "136", "KYD" },
			{ "144", "LKR" },
			{ "152", "CLP" },
			{ "156", "CNY" },
			{ "170", "COP" },
			{ "174", "KMF" },
			{ "188", "CRC" },
			{ "191", "HRK" },
			{ "192", "CUP" },
			{ "203", "CZK" },
			{ "208", "DKK" },
			{ "214", "DOP" },
			{ "222", "SVC" },
			{ "230", "ETB" },
			{ "232", "ERN" },
			{ "233", "EEK" },
			{ "238", "FKP" },
			{ "242", "FJD" },
			{ "246", "FIM" },
			{ "250", "FRF" },
			{ "262", "DJF" },
			{ "270", "GMD" },
			{ "280", "DEM" },
			{ "292", "GIP" },
			{ "300", "GRD" },
			{ "320", "GTQ" },
			{ "324", "GNF" },
			{ "328", "GYD" },
			{ "332", "HTG" },
			{ "340", "HNL" },
			{ "344", "HKD" },
			{ "348", "HUF" },
			{ "352", "ISK" },
			{ "356", "INR" },
			{ "360", "IDR" },
			{ "364", "IRR" },
			{ "368", "IQD" },
			{ "372", "IEP" },
			{ "376", "ILS" },
			{ "380", "ITL" },
			{ "388", "JMD" },
			{ "392", "JPY" },
			{ "398", "KZT" },
			{ "400", "JOD" },
			{ "404", "KES" },
			{ "408", "KPW" },
			{ "410", "KRW" },
			{ "414", "KWD" },
			{ "417", "KGS" },
			{ "418", "LAK" },
			{ "422", "LBP" },
			{ "426", "LSL" },
			{ "428", "LVL" },
			{ "430", "LRD" },
			{ "434", "LYD" },
			{ "440", "LTL" },
			{ "442", "LUF" },
			{ "446", "MOP" },
			{ "454", "MWK" },
			{ "458", "MYR" },
			{ "462", "MVR" },
			{ "470", "MTL" },
			{ "478", "MRO" },
			{ "480", "MUR" },
			{ "484", "MXN" },
			{ "496", "MNT" },
			{ "498", "MDL" },
			{ "504", "MAD" },
			{ "512", "OMR" },
			{ "516", "NAD" },
			{ "524", "NPR" },
			{ "528", "NLG" },
			{ "532", "ANG" },
			{ "533", "AWG" },
			{ "548", "VUV" },
			{ "554", "NZD" },
			{ "558", "NIO" },
			{ "566", "NGN" },
			{ "578", "NOK" },
			{ "586", "PKR" },
			{ "590", "PAB" },
			{ "598", "PGK" },
			{ "600", "PYG" },
			{ "604", "PEN" },
			{ "608", "PHP" },
			{ "620", "PTE" },
			{ "634", "QAR" },
			{ "643", "RUB" },
			{ "646", "RWF" },
			{ "654", "SHP" },
			{ "678", "STD" },
			{ "682", "SAR" },
			{ "690", "SCR" },
			{ "694", "SLL" },
			{ "702", "SGD" },
			{ "703", "SKK" },
			{ "704", "VND" },
			{ "706", "SOS" },
			{ "710", "ZAR" },
			{ "724", "ESP" },
			{ "728", "SSP" },
			{ "748", "SZL" },
			{ "752", "SEK" },
			{ "756", "CHF" },
			{ "760", "SYP" },
			{ "764", "THB" },
			{ "776", "TOP" },
			{ "780", "TTD" },
			{ "784", "AED" },
			{ "788", "TND" },
			{ "800", "UGX" },
			{ "807", "MKD" },
			{ "818", "EGP" },
			{ "826", "GBP" },
			{ "834", "TZS" },
			{ "840", "USD" },
			{ "858", "UYU" },
			{ "860", "UZS" },
			{ "882", "WST" },
			{ "886", "YER" },
			{ "901", "TWD" },
			{ "928", "VES" },
			{ "929", "MRU" },
			{ "932", "ZWL" },
			{ "933", "BYN" },
			{ "934", "TMT" },
			{ "936", "GHS" },
			{ "938", "SDG" },
			{ "941", "RSD" },
			{ "943", "MZN" },
			{ "944", "AZN" },
			{ "946", "RON" },
			{ "947", "CHE" },
			{ "948", "CHW" },
			{ "949", "TRY" },
			{ "950", "XAF" },
			{ "951", "XCD" },
			{ "952", "XOF" },
			{ "953", "XPF" },
			{ "955", "XBA" },
			{ "956", "XBB" },
			{ "957", "XBC" },
			{ "958", "XBD" },
			{ "959", "XAU" },
			{ "960", "XDR" },
			{ "961", "XAG" },
			{ "962", "XPT" },
			{ "964", "XPD" },
			{ "967", "ZMW" },
			{ "968", "SRD" },
			{ "969", "MGA" },
			{ "970", "COU" },
			{ "971", "AFN" },
			{ "972", "TJS" },
			{ "973", "AOA" },
			{ "975", "BGN" },
			{ "976", "CDF" },
			{ "977", "BAM" },
			{ "978", "EUR" },
			{ "979", "MXV" },
			{ "980", "UAH" },
			{ "981", "GEL" },
			{ "984", "BOV" },
			{ "985", "PLN" },
			{ "986", "BRL" },
			{ "990", "CLF" },
			{ "997", "USN" },
			{ "999", "XXX" },
		};

		public string GetCode(string numericCode)
		{
			numericCode ??= "";
			if (dict.ContainsKey(numericCode))
			{
				return dict[numericCode];
			}
			return numericCode;
		}

        public List<string> GetCodes()
        {
			return dict.OrderBy(x => x.Value).Select(kvp => kvp.Value).ToList();
        }

        public bool Insert(string alphaCode, string numericCode)
        {
			bool success = false;
			try
			{
				if (!StringUtility.IsNumeric(alphaCode, 3) || !StringUtility.IsNumeric(numericCode, 3))
                {
					throw new ArgumentNullException($"Villa í inntaki: Insert({alphaCode}, string {numericCode})");
				}

				if (dict.ContainsKey(numericCode) == false)
				{
					dict.Add(numericCode, numericCode);
				}
				
				success = true;
			}
			catch (Exception) { }

			return success;
		}
	}
}
