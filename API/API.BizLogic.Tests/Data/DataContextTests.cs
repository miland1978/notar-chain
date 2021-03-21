using System;
using API.BizLogic.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace API.BizLogic.Tests.Data
{
	public class DataContextTests
	{
		[Fact]
		public void CanCreateSqliteSchema()
		{
			var connection = new SqliteConnection("DataSource=:memory:");
			connection.Open();
			connection.EnableExtensions();

			var builder = new DbContextOptionsBuilder<DataContext>();
			var options = builder.UseSqlite(connection).Options;

			using (var context = (DataContext)Activator.CreateInstance(typeof(DataContext), options, new DbContextConfig<DataContext>()))
			{
				context.Database.EnsureCreated();
			}
		}
	}
}
