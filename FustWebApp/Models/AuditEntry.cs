using Microsoft.EntityFrameworkCore.ChangeTracking;
using FustWebApp.Models.Domain;
using Newtonsoft.Json;

namespace FustWebApp.Models
{
	public class AuditEntry
	{

		public AuditEntry(EntityEntry entry)
		{
			Entry = entry;
		}


		public EntityEntry Entry { get; }
		public string UserId { get; set; }
		public string TableName { get; set; }
		public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
		public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
		public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
		public AuditType AuditType { get; set; }
		public List<string> ChangedColumns { get; } = new List<string>();



		public FustWebApp.Models.Domain.Audit ToAudit()
		{
			var audit = new FustWebApp.Models.Domain.Audit();
			audit.UserId = UserId;
			audit.Type = AuditType.ToString();
			audit.TableName = TableName;
			audit.DateTime = DateTime.Now;
			audit.PrimaryKey = JsonConvert.SerializeObject(KeyValues);
			audit.OldValues = OldValues.Count == 0 ? "N/A" : JsonConvert.SerializeObject(OldValues);
			audit.NewValues = NewValues.Count == 0 ? "N/A" : JsonConvert.SerializeObject(NewValues);
			audit.AffectedColumns = ChangedColumns.Count == 0 ? "N/A" : JsonConvert.SerializeObject(ChangedColumns);
			return audit;

		}

	}
}
