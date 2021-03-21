using System.Linq;
using System.Net;
using System.Threading.Tasks;
using API.BizLogic.Data;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace API.Host.Tests
{
	public class StartupTests
	{
		[Fact]
		public async Task StartupTest()
		{
			using (var waf = new WebApplicationFactory<Startup>()
				.WithWebHostBuilder(builder => builder.ConfigureTestServices(ConfigureTestServices)))
			{
				var httpClient = waf.Server.CreateClient();
				var response = await httpClient.GetAsync("/");
				response.StatusCode.Should().Be(HttpStatusCode.OK);

				response = await httpClient.GetAsync("/swagger/index.html");
				response.StatusCode.Should().Be(HttpStatusCode.OK);

				response = await httpClient.GetAsync("/swagger/v1/swagger.json");
				response.StatusCode.Should().Be(HttpStatusCode.OK);
			}
		}

		private void ConfigureTestServices(IServiceCollection services)
		{
			var descriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<DataContext>));
			services.Remove(descriptor);
			services.AddDbContext<DataContext>(options => { options.UseSqlite("DataSource=:memory:"); });
		}
	}
}
