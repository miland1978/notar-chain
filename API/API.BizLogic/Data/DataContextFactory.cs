using Microsoft.EntityFrameworkCore;
using System;

namespace API.BizLogic.Data
{
	public class DataContextFactory : IFactory<DataContext>
	{
		private readonly bool _useManagedIdentity;
		private readonly Action<DbContextOptionsBuilder> _builder;

		public DataContextFactory(string connString, bool useManagedIdentity, int maxRetryCount = 5, TimeSpan maxRetryDelay = default(TimeSpan))
		{
			_useManagedIdentity = useManagedIdentity;

			if (maxRetryDelay == default(TimeSpan))
			{
				maxRetryDelay = TimeSpan.FromSeconds(30);
			}

			if (connString.Contains("Sqlite"))
			{
				_builder = options =>
				{
					options.UseSqlite(connString);
				};
			}
			else
			{
				_builder = options =>
				{
					options.UseSqlServer(
						connString,
						x => x.EnableRetryOnFailure(maxRetryCount, maxRetryDelay, errorNumbersToAdd: null));
				};
			}
		}

		public bool UseManagedServiceIdentity { get; set; }

		public DataContext Create()
		{
			var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
			_builder(optionsBuilder);
			var context = new DataContext(optionsBuilder.Options, new DbContextConfig<DataContext> { UseManagedIdentity = _useManagedIdentity });

			return context;
		}

		public Action<DbContextOptionsBuilder> GetOptionsBuilder()
		{
			return _builder;
		}
	}
}
