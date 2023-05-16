using Microsoft.Build.Framework;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace FustWebApp.Models.Domain
{
	public class Fust
	{
		public int Id { get; set; }

		[DisplayName("Name")]
		public string FustName { get; set; }

		[DisplayName("Amount Per Pallet")]
		public int FustAmountPerPallet { get; set; }

		public int ExpectedQuantity { get; set; } = 0;

		[DisplayName("Base Value")]
		public float baseValue { get; set; }

		
		[DisplayName("Type")]
		public  FustType FustType { get; set; }
		
	   
	   
	}
}