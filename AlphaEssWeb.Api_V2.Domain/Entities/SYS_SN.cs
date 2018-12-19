namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class SYS_SN : IEntity<Guid>
	{
		[Key]
		//public Guid SN_ID { get; set; }
		[Column("SN_ID")]
		public Guid Key { get; set; }

		public int? TYPE_ID { get; set; }

		[StringLength(50)]
		public string SN_NO { get; set; }

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

		[StringLength(64)]
		public string BindPwd { get; set; }
		public int? IsUpgrade { get; set; }

		[StringLength(255)]
		public string NoUpgrade_Reason { get; set; }
	}
}
