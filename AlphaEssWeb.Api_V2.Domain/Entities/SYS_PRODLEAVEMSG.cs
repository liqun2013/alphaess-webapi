namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class SYS_PRODLEAVEMSG : IEntity<Guid>
	{
		//[Key]
		//public Guid LEAVEMSG_ID { get; set; }
		[Key]
		[Column("LEAVEMSG_ID")]
		public Guid Key { get; set; }

		public Guid? PRODID { get; set; }

		public int? NUMS { get; set; }

		public decimal? PRICE { get; set; }

		public Guid? USER_ID { get; set; }

		[StringLength(4000)]
		public string CONTENT { get; set; }

		public int DELETE_FLAG { get; set; }

		[StringLength(100)]
		public string CREATE_ACCOUNT { get; set; }

		public DateTime? CREATE_DATETIME { get; set; }

		[StringLength(100)]
		public string LASTUPDATE_ACCOUNT { get; set; }

		public DateTime? LASTUPDATE_DATETIME { get; set; }

		[StringLength(100)]
		public string EMAIL { get; set; }

		[StringLength(50)]
		public string LINKMAN { get; set; }

		[StringLength(50)]
		public string CELLPHONE { get; set; }
	}
}
