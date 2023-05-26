using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace FustWebApp.Models.Domain
{
	public class AdjustmentCodes
	{

		public int Id { get; set; }

		[Required]
		[DisplayName("Adjustment Code")]
		[RegularExpression(@"^\w+$",ErrorMessage ="Adjusment Code must be single word")]
		public string AdjustmentCode { get; set; }

		[Required]
		[DisplayName("Code Description")]
		public string CodeDescription { get; set; }
	}
}
