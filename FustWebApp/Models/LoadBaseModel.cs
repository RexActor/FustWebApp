using FustWebApp.Models.Domain;


using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace FustWebApp.Models
{
	public class LoadBaseModel
	{

		
		[DisplayName("Trailer Number")]		
		public string LoadTrailerNumber { get; set; }

		[DisplayName("Load Date")]	
		public DateTime LoadDate { get; set; }

		[DisplayName("Supplier")]
		[Required(ErrorMessage = "Please select Supplier from list")]
		public string LoadSupplier { get;set; }

		[DisplayName("Load Type")]
		public string LoadType { get; set; } //inbound or outbound		
		
		[DisplayName("Fust Items")]
		public List<LoadFusts> LoadFustItems { get; set; }

		[DisplayName("Load Group")]
		public string LoadGroup { get; set; }

		[DisplayName("Load Origin")]
		public string LoadOrigin { get; set; }

		[DisplayName("Load Comment")]
		public string LoadComment { get; set; }		
		public string CreatedBy { get; set; }
		public string UpdatedBy { get; set; }
		public string ReceivedBy { get; set; }

		[DisplayName("Purhcase Order")]
		public int PONumber { get; set; } = 0;

		public DateTime LoadUpdatedDate { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime ReceivedDate { get; set;}
	}
}
