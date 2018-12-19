namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class Sys_ResellerLicense : IEntity<long>
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		[Column("Id")]
		public long Key { get; set; }

		public Guid UserId { get; set; }

		[Required]
		[StringLength(64)]
		public string LicenseNo { get; set; }

		public DateTime? CreateTime { get; set; }

	}
}
