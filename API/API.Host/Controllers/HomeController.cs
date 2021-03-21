using API.BizLogic.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Host.Controllers
{
	[ApiController]
	[Route("/")]
	[ApiExplorerSettings(IgnoreApi = true)]
	public class HomeController : Controller
	{
		private readonly DataContext _context;
		private readonly ILogger<HomeController> _logger;

		public HomeController(DataContext context, ILoggerFactory loggerFactory)
		{
			_context = context;
			_logger = loggerFactory.CreateLogger<HomeController>();
		}

		[HttpGet]
		public IActionResult Index()
		{
			if (_context.Database.CanConnect())
			{
				_logger.LogInformation("Healthy");
				return Ok("OK");
			}

			return StatusCode(StatusCodes.Status500InternalServerError);
		}
	}
}
