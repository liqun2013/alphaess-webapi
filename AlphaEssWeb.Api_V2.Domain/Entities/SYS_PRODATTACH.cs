namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class SYS_PRODATTACH : IEntity<Guid>
	{
		//[Key]
		//public Guid PRODATTACH_ID { get; set; }
		[Key]
		[Column("PRODATTACH_ID")]
		public Guid Key { get; set; }

		[StringLength(100)]
		public string ATTACHNAME { get; set; }

		[StringLength(200)]
		public string ATTACHURL { get; set; }

		public int DELETE_FLAG { get; set; }

		[StringLength(100)]
		public string CREATE_ACCOUNT { get; set; }

		public DateTime? CREATE_DATETIME { get; set; }

		[StringLength(100)]
		public string LASTUPDATE_ACCOUNT { get; set; }

		public DateTime? LASTUPDATE_DATETIME { get; set; }

		[Required]
		[StringLength(50)]
		public string PROD_ID { get; set; }

		[StringLength(50)]
		public string ATTACH_SUFFIX { get; set; }

		[StringLength(50)]
		public string ATTACH_MD5 { get; set; }
	}
}
