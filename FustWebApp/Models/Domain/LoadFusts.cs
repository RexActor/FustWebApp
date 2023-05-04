using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FustWebApp.Models.Domain
{
	public class LoadFusts
	{
		[Key]
		[DisplayName("ID")]
		public int LoadFustId { get; set; }

		[DisplayName("Name")]
		public string FustName { get; set; }

		public int ExpectedQuantity { get; set; }
		public int ReceivedQty { get; set; }

		public Loads Loads { get; set; }

		[DisplayName("Type")]
		public FustType FustType { get; set; }


	}
}
