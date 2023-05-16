using FustWebApp.Models.Domain;

using System.ComponentModel;

namespace FustWebApp.Models
{
	public class LoadFustItemModel
	{


		public int LoadFustItemId { get; set; }

		[DisplayName("Name")]
		public string FustName { get; set; }

		[DisplayName("Amount Per Pallet")]
		public int Qty { get; set; }



		public float unitValue { get; set; }


		[DisplayName("Currency")]
		public Currency Currency { get; set; }

		[DisplayName("Type")]
		public FustType FustType { get; set; }
	}
}
