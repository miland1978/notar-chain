using Microsoft.EntityFrameworkCore;

namespace API.BizLogic.Data
{
	public class DbContextConfig<T>
		where T : DbContext
	{
		public bool UseManagedIdentity { get; set; }
	}
}
