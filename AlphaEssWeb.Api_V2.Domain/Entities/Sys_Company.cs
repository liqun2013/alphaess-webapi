namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class Sys_Company : IEntity<Guid>
	{
		[Key]
		[Column("CompanyId")]
		public Guid Key { get; set; }

		[Required]
		[StringLength(128)]
		public string CompanyName { get; set; }

		[StringLength(400)]
		public string CompanyDescription { get; set; }

		public Guid? Manager { get; set; }

		[StringLength(128)]
		public string CompanyUrl { get; set; }

		[StringLength(64)]
		public string Country { get; set; }

		[StringLength(64)]
		public string State { get; set; }

		[StringLength(64)]
		public string City { get; set; }

		public DateTime? CreateTime { get; set; }

		public DateTime? UpdateTime { get; set; }
	}
}
