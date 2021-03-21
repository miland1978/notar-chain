using System.Text.Json.Serialization;
using API.BizLogic.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace API.Host
{
	public class Startup
	{
		private readonly IWebHostEnvironment _environment;

		public Startup(IWebHostEnvironment env, IConfiguration configuration)
		{
			Configuration = configuration;
			_environment = env;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddApplicationInsightsTelemetry();
			services.AddDatabaseDeveloperPageExceptionFilter();

			bool useMSIForDatabase = Configuration.GetValue(Settings.UseMSIForDatabase, false);
			string connectionString = Configuration.GetConnectionString(Settings.DbConnectionString);
			services.AddSingleton(new DbContextConfig<DataContext> { UseManagedIdentity = useMSIForDatabase });
			var contextFactory = new DataContextFactory(connectionString, useMSIForDatabase);
			services.AddDbContext<DataContext>(contextFactory.GetOptionsBuilder());

			services.AddControllers()
				.AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "API.Host", Version = "v1" });
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseMigrationsEndPoint();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API.Host v1"));
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
