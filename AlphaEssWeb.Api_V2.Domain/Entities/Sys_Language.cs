namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class Sys_Language : IEntity<Guid>
	{
		[Key]
		[Column("LanguageId")]
		public Guid Key { get; set; }

		[Required]
		[StringLength(64)]
		public string LanguageCategory { get; set; }

		[Required]
		[StringLength(64)]
		public string LanguageKey { get; set; }

		[Required]
		[StringLength(1024)]
		public string LanguageValue { get; set; }

		public DateTime? CreateTime { get; set; }

		public Guid? CompanyId { get; set; }
	}
}
