using System;
using System.ComponentModel.DataAnnotations;

namespace API.BizLogic.Data
{
	public class UserDeviceInfo : IRootEntity
	{
		[Required]
		[StringLength(Lengths.DeviceId)]
		public string Id { get; set; }

		[Required]
		[StringLength(Lengths.DeviceType)]
		public UserDeviceType DeviceType { get; set; }

		[StringLength(Lengths.DeviceName)]
		public string DeviceName { get; set; }

		[StringLength(Lengths.PushId)]
		public string PushId { get; set; }

		[StringLength(Lengths.AppVersion)]
		public string AppVersion { get; set; }

		public DateTimeOffset CreatedAt { get; set; }

		public DateTimeOffset ModifiedAt { get; set; }
	}
}
