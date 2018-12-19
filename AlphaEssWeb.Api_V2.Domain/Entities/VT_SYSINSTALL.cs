namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class VT_SYSINSTALL : IEntity<Guid>
	{
		[Key]
		//public Guid INSTALL_ID { get; set; }
		[Column("INSTALL_ID")]
		public Guid Key { get; set; }

		public Guid? CompanyId { get; set; }

		[StringLength(50)]
		public string SYS_SN { get; set; }

		public DateTime? INSTALL_TIME { get; set; }

		[StringLength(50)]
		public string INSTALL_USERID { get; set; }

		[StringLength(50)]
		public string CUST_NAME { get; set; }

		[StringLength(50)]
		public string CELL_PHONE { get; set; }

		[StringLength(300)]
		public string REAMRK { get; set; }

		public int DELETE_FLAG { get; set; }

		[StringLength(100)]
		public string CREATE_ACCOUNT { get; set; }

		public DateTime? CREATE_DATETIME { get; set; }

		[StringLength(100)]
		public string LASTUPDATE_ACCOUNT { get; set; }

		public DateTime? LASTUPDATE_DATETIME { get; set; }

		[StringLength(200)]
		public string ADDRESS { get; set; }

		[StringLength(50)]
		public string INSTALL_USERNAME { get; set; }
	}
}
