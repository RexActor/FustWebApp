using System.ComponentModel;

namespace FustWebApp.Models.Domain
{
	public class StockHolding
	{
		[DisplayName("Id")]
		public int StockHoldingId { get; set; }

		[DisplayName("Stockholding Date")]
		public DateTime StockholdingDate { get; set; }

		[DisplayName("Fust Item")]
		public Fust StockHoldingFustItems { get; set; }

		[DisplayName("Supplier")]
		public Supplier StockHoldingSupplier { get; set; }

		[DisplayName("Holding Quantity")]
		public int StockHoldingQty { get;set; }

		[DisplayName("Base Value")]
		public float baseValue { get; set; }





	}
}
