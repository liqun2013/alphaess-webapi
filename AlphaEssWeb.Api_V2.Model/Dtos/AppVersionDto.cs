using System;

namespace AlphaEssWeb.Api_V2.Model.Dtos
{
	public sealed class AppVersionDto:IDto<Guid>
	{
		public Guid Id { get; set; }
		public string AppType { get; set; }
		public int AppVersionCode { get; set; }
		public string AppVersion { get; set; }
		public string AppDownloadUrl { get; set; }
		public int AppForcedUpdates { get; set; }
		public DateTime? AppCreateTime { get; set; }
	}
}
