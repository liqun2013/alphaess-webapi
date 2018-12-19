namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class Sys_ServicePartnerSn : IEntity<long>
	{
		[Key]
		[Column("Id")]
		public long Key { get; set; }

		public Guid UserId { get; set; }

		[Required]
		[StringLength(64)]
		public string SysSn { get; set; }

		public DateTime? CreateTime { get; set; }
	}
}
