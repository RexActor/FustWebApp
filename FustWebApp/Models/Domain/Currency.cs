using System.ComponentModel;

namespace FustWebApp.Models.Domain
{
	public class Currency
	{

		[DisplayName("Id")]
		public int currencyId { get; set; }
		[DisplayName("Name")]
		public string currencyName { get; set; }
		[DisplayName("Abbrevation")]
		public string currencyAbrevation { get; set; }

		

		[DisplayName("Country Code")]
		public string currencyCountryCode { get; set; }

		[DisplayName("Target Name")]
		public string currencyTargetName { get; set; }
		[DisplayName("Target Abbrevation")]
		public string currencyTargetNameAbrevation { get; set; }
		[DisplayName("Exchange Rate")]
		public float currencyExchangeRate { get; set; }
		[DisplayName("Year")]
		public int exchangeRateYear { get; set; }

	}
}
