namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class SYS_LICENSE : IEntity<Guid>
	{
		//[Key]
		//public Guid LICENSE_ID { get; set; }
		[Key]
		[Column("LICENSE_ID")]
		public Guid Key { get; set; }

		public int? TYPE_ID { get; set; }

		[StringLength(64)]
		public string LICENSE_NO { get; set; }

		[StringLength(200)]
		public string DESCRIPTION { get; set; }

		public int DELETE_FLAG { get; set; }

		[StringLength(100)]
		public string CREATE_ACCOUNT { get; set; }

		public DateTime? CREATE_DATETIME { get; set; }

		[StringLength(100)]
		public string LASTUPDATE_ACCOUNT { get; set; }

		public DateTime? LASTUPDATE_DATETIME { get; set; }

		public Guid? CompanyId { get; set; }
	}
}
