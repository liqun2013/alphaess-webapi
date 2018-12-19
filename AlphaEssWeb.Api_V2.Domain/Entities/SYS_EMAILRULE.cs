namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class SYS_EMAILRULE : IEntity<Guid>
	{
		//[Key]
		//public Guid EMAILRULE_ID { get; set; }
		[Key]
		[Column("EMAILRULE_ID")]
		public Guid Key { get; set; }

		[StringLength(64)]
		public string TYPE_CODE { get; set; }

		[StringLength(256)]
		public string REMARK { get; set; }

		public int DELETE_FLAG { get; set; }

		[StringLength(64)]
		public string CREATE_ACCOUNT { get; set; }

		public DateTime? CREATE_DATETIME { get; set; }

		[StringLength(64)]
		public string LASTUPDATE_ACCOUNT { get; set; }

		public DateTime? LASTUPDATE_DATETIME { get; set; }

		public Guid? CompanyId { get; set; }
		public int? Disabled { get; set; }
	}
}
