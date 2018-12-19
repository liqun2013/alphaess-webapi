namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class VT_SYSMAINTAIN : IEntity<Guid>
	{
		[Key]
		//public Guid MAINTAIN_ID { get; set; }
		[Column("MAINTAIN_ID")]
		public Guid Key { get; set; }

		public Guid? CompanyId { get; set; }

		[StringLength(50)]
		public string SYS_SN { get; set; }

		[StringLength(200)]
		public string FAULT_DESC { get; set; }

		[StringLength(50)]
		public string OLDDEV_SN { get; set; }

		[StringLength(50)]
		public string NEWDEV_SN { get; set; }

		[StringLength(50)]
		public string RESULT { get; set; }

		[Required]
		[StringLength(300)]
		public string REMARK { get; set; }

		[StringLength(50)]
		public string MAINTAIN_USERID { get; set; }

		public DateTime? MAINTAIN_TIME { get; set; }

		public int DELETE_FLAG { get; set; }

		[StringLength(100)]
		public string CREATE_ACCOUNT { get; set; }

		public DateTime? CREATE_DATETIME { get; set; }

		[StringLength(100)]
		public string LASTUPDATE_ACCOUNT { get; set; }

		public DateTime? LASTUPDATE_DATETIME { get; set; }

		[StringLength(200)]
		public string REASON_ANALY { get; set; }

		[StringLength(50)]
		public string DEAL_METHOD { get; set; }

		[StringLength(50)]
		public string DEV_TYPE { get; set; }
	}
}
