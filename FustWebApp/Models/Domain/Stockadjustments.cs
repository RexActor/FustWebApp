namespace FustWebApp.Models.Domain
{
	public class Stockadjustments
	{

		public int Id { get; set; }
		public string Reason { get; set; }
		public int FromQuantity { get; set; }
		public int ToQuantity {  get; set; }
		public DateTime adjustmentDate { get;set; }
		public AdjustmentCodes AdjustmentCode { get;set; }

		public StockHolding stockHolding { get; set; }



	}
}
