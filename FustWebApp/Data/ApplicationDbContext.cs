using Audit.EntityFramework;

using FustWebApp.Models;
using FustWebApp.Models.Domain;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;



using System.Reflection.Metadata;
using System.Security.Claims;

namespace FustWebApp.Data
{


	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{

		}






		public DbSet<Supplier> Suppliers { get; set; }
		public DbSet<Fust> Fusts { get; set; }
		public DbSet<FustType> FustTypes { get; set; }
		public DbSet<Origin> Origins { get; set; }
		public DbSet<Group> Groups { get; set; }

		public DbSet<Loads> Loads { get; set; }

		public DbSet<LoadFusts> LoadFusts { get; set; }
		public DbSet<StockHolding> StockHolding { get; set; }




		public DbSet<FustWebApp.Models.Domain.Audit> AuditLogs { get; set; }

		public virtual async Task<int> SaveChangesAsync(string userId = null)
		{
			OnBeforeSaveChanges(userId);
			var result = await base.SaveChangesAsync();
			return result;
		}

		private void OnBeforeSaveChanges(string userId)
		{
			ChangeTracker.DetectChanges();
			var auditEntries = new List<AuditEntry>();
			foreach (var entry in ChangeTracker.Entries())
			{

				if (entry.Entity is FustWebApp.Models.Domain.Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
				{
					continue;
				}

				var auditEntry = new AuditEntry(entry);
				auditEntry.TableName = entry.Entity.GetType().Name;
				auditEntry.UserId = userId;
				auditEntries.Add(auditEntry);
				var databaseValues = entry.GetDatabaseValues();

				foreach (var property in entry.Properties)
				{
					string propertyName = property.Metadata.Name;
					if (property.Metadata.IsPrimaryKey())
					{
						auditEntry.KeyValues[propertyName] = property.CurrentValue;
						continue;
					}
					switch (entry.State)
					{
						case EntityState.Added:
							auditEntry.AuditType = AuditType.Create;
							auditEntry.NewValues[propertyName] = property.CurrentValue;
							break;
						case EntityState.Deleted:
							auditEntry.AuditType = AuditType.Delete;
							auditEntry.OldValues[propertyName] = property.OriginalValue;
							break;
						case EntityState.Modified:
							if (property.IsModified)
							{
								auditEntry.ChangedColumns.Add(propertyName);
								auditEntry.AuditType = AuditType.Update;
								
								auditEntry.OldValues[propertyName] = databaseValues[propertyName];
								auditEntry.NewValues[propertyName] = property.CurrentValue;
							}
							break;
					}
				}


			}

			foreach (var auditEntry in auditEntries)
			{
				AuditLogs.Add(auditEntry.ToAudit());
			}

		}







	}




}