using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.BizLogic.Data
{
	public class DataContext : DbContext
	{
		private readonly bool _validateValuesOnSave;

		public DataContext(DbContextOptions options, DbContextConfig<DataContext> config = null)
			: this(options, config, false)
		{
		}

		public DataContext(DbContextOptions options, DbContextConfig<DataContext> config = null, bool validateValuesOnSave = false)
			: base(options)
		{
			_validateValuesOnSave = validateValuesOnSave;

			if (config?.UseManagedIdentity == true)
			{
				var connection = (Microsoft.Data.SqlClient.SqlConnection)Database.GetDbConnection();
				connection.AccessToken = new Microsoft.Azure.Services.AppAuthentication.AzureServiceTokenProvider().GetAccessTokenAsync("https://database.windows.net/").Result;
			}
		}

		public DbSet<UserDeviceInfo> UserDevices { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<UserDeviceInfo>()
				.ToTable("UserDevices", schema: "kyc")
				.Property(x => x.DeviceType)
				.HasConversion(new EnumToStringConverter<UserDeviceType>())
				.HasMaxLength(Lengths.DeviceType);

			if (Database.ProviderName.Equals("Microsoft.EntityFrameworkCore.Sqlite"))
			{
				ConfigureSqlite(builder);
			}

			SeedData(builder);
		}

		protected virtual void SeedData(ModelBuilder builder)
		{
		}

		private static void ConfigureSqlite(ModelBuilder builder)
		{
			foreach (var entityType in builder.Model.GetEntityTypes())
			{
				var properties = entityType.ClrType.GetProperties()
					.Where(p => p.PropertyType == typeof(DateTimeOffset) || p.PropertyType == typeof(DateTimeOffset?));

				foreach (var property in properties)
				{
					builder
						.Entity(entityType.Name)
						.Property(property.Name)
						.HasConversion(new DateTimeOffsetToBinaryConverter());
				}
			}
		}

		public override int SaveChanges(bool acceptAllChangesOnSuccess)
		{
			UpdateValues();
			ValidateValuesOnSave();

			return base.SaveChanges(acceptAllChangesOnSuccess);
		}

		public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
		{
			UpdateValues();
			ValidateValuesOnSave();

			return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}

		private void UpdateValues()
		{
			foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
			{
				if (entry.Entity is IRootEntity root)
				{
					root.ModifiedAt = DateTime.UtcNow;

					if (entry.State == EntityState.Added && root.CreatedAt == default)
					{
						root.CreatedAt = root.ModifiedAt;
					}
				}
			}
		}

		private void ValidateValuesOnSave()
		{
			if (_validateValuesOnSave)
			{
				foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
				{
					Validator.ValidateObject(entry.Entity, new ValidationContext(entry.Entity), true);
				}
			}
		}
	}
}
