using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FustWebApp.Models.Domain
{
	public class Loads : LoadBaseModel
	{
		[Key]
		[DisplayName("ID")]
		public int LoadId { get; set; }


	}
}
