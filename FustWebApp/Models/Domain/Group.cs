using System.ComponentModel;

namespace FustWebApp.Models.Domain
{
	public class Group
	{
		public Guid Id { get; set; }

		[DisplayName("Name")]
		public string GroupName { get; set; }
		[DisplayName("Comment")]
		public string? GroupComment { get; set; } = "N/A";

	}
}
