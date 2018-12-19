namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class VT_CMD : IEntity<Guid>
	{
		[Key]
		//public Guid CMD_ID { get; set; }
		[Column("CMD_ID")]
		public Guid Key { get; set; }

		[StringLength(50)]
		public string SYS_SN { get; set; }

		[StringLength(50)]
		public string LIC_NO { get; set; }

		[StringLength(300)]
		public string CMD_CONT { get; set; }

		[StringLength(300)]
		public string REMARK { get; set; }

		public int? DISABLE_FLAG { get; set; }

		public int DELETE_FLAG { get; set; }

		[StringLength(100)]
		public string CREATE_ACCOUNT { get; set; }

		public DateTime? CREATE_DATETIME { get; set; }

		[StringLength(100)]
		public string LASTUPDATE_ACCOUNT { get; set; }

		public DateTime? LASTUPDATE_DATETIME { get; set; }

		[StringLength(50)]
		public string APP_TYPE { get; set; }

		[StringLength(50)]
		public string CMD_CODE { get; set; }

		[StringLength(20)]
		public string START_TIME { get; set; }
	}
}
