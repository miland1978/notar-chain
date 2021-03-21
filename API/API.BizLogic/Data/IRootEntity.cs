using System;

namespace API.BizLogic.Data
{
	public interface IRootEntity
	{
		public DateTimeOffset CreatedAt { get; set; }

		public DateTimeOffset ModifiedAt { get; set; }
	}
}
