using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using FustWebApp.Models.Domain;

namespace FustWebApp.Models
{
	public class LoadViewModel
	{

		[DisplayName("Trailer Number")]
		[DefaultValue("Hello")]
		public string LoadTrailerNumber { get; set; }
		[DisplayName("Load Date")]

		//[DataType(DataType.Date), Required]
		//[DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
		public DateTime LoadDate { get; set; }



		[DisplayName("Supplier")]
		[Required(ErrorMessage = "Please select Supplier from list")]
		public Supplier LoadSupplier { get; set; }



		[DisplayName("Load Type")]
		public string LoadType { get; set; } //inbound or outbound


		[DisplayName("Fust Items")]
		public List<Fust> LoadFustItems { get; set; }
		[DisplayName("Purchase Order")]
		public int PONumber { get; set; } = 0;

		public string InvoiceNumber { get; set; } = "null";

		[DisplayName("Load Group")]
		public string LoadGroup { get; set; }
		[DisplayName("Load Origin")]
		public string LoadOrigin { get; set; }
		[DisplayName("Load Comment")]
		public string LoadComment { get; set; }

	}
}
