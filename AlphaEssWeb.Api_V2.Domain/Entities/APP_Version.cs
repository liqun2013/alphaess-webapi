using AlphaEss.Api_V2.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlphaEssWeb.Api_V2.Domain
{
	public partial class APP_Version : IEntity<Guid>
	{
		[Key]
		[Column("AppVersionId")]
		public Guid Key { get; set; }
		[Required]
		[StringLength(16)]
		public string AppType { get; set; }
		[Required]
		public int AppVersionCode { get; set; }
		[Required]
		[StringLength(32)]
		public string AppVersion { get; set; }
		[Required]
		[StringLength(640)]
		public string AppDownloadUrl { get; set; }
		[Required]
		public int AppForcedUpdates { get; set; }
		public DateTime? AppCreateTime { get; set; }
  }
}
