using MessagePack;

using Microsoft.AspNetCore.Mvc;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FustWebApp.Models.Domain
{
	public class Supplier
	{
	   

		public Guid Id { get; set; }

		[Required]
		[DisplayName("Name")]
		public string SupplierName { get; set; }
		[Required]
		[DisplayName("Address")]
		public string SupplierAddress { get; set; }
		[Required]
		[DisplayName("Origin")]
		public string SupplierOrigin { get; set; }
		[Required]
		[DisplayName("Group")]
		public string SupplierGroup { get; set; }

		[Required]
		[DisplayName("Fust Type")]
		public  string FustTypeList { get; set; }

	


	}
}
