namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class VT_SYSFAULT : IEntity<Guid>
	{
		[Key]
		//public Guid FAULT_ID { get; set; }
		[Column("FAULT_ID")]
		public Guid Key { get; set; }

		[StringLength(50)]
		public string SYS_SN { get; set; }

		[StringLength(50)]
		public string ERROR_CODE { get; set; }

		public DateTime? HAPPEN_TIME { get; set; }

		public DateTime? END_TIME { get; set; }

		public int? ERROR_TYPE { get; set; }

		[StringLength(100)]
		public string LASTMAINTAIN_USERID { get; set; }

		public DateTime? LASTMAINTAIN_TIME { get; set; }

		public int DELETE_FLAG { get; set; }

		[StringLength(100)]
		public string CREATE_ACCOUNT { get; set; }

		public DateTime? CREATE_DATETIME { get; set; }

		[StringLength(100)]
		public string LASTUPDATE_ACCOUNT { get; set; }

		public DateTime? LASTUPDATE_DATETIME { get; set; }

		[StringLength(100)]
		public string ERR_CONTENT { get; set; }
		public Guid? CompanyId { get; set; }
	}
}
