using System;
using API.BizLogic.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace API.BizLogic.Tests.Data
{
	public class TestContextFactory<T> : IFactory<T>, IDisposable
		where T : DbContext
	{
		private readonly DbContextOptions<T> _options;
		private readonly SqliteConnection _connection;

		public TestContextFactory()
		{
			_connection = new SqliteConnection("DataSource=:memory:");
			_connection.Open();
			_connection.EnableExtensions();

			var builder = new DbContextOptionsBuilder<T>();
			_options = builder.UseSqlite(_connection).Options;
			bool validateValuesOnSave = true;

			using (var context = (T)Activator.CreateInstance(typeof(T), _options, new DbContextConfig<T>(), validateValuesOnSave))
			{
				context.Database.EnsureCreated();
			}
		}

		public T Create()
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
