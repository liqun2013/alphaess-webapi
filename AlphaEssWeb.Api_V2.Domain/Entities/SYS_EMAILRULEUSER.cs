namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class SYS_EMAILRULEUSER : IEntity<Guid>
	{
		//[Key]
		//public Guid EMAILRULEUSER_ID { get; set; }
		[Key]
		[Column("EMAILRULEUSER_ID")]
		public Guid Key { get; set; }

		public Guid? EMAILRULE_ID { get; set; }

		public Guid? USER_ID { get; set; }

		public int DELETE_FLAG { get; set; }

		[StringLength(64)]
		public string CREATE_ACCOUNT { get; set; }

		public DateTime? CREATE_DATETIME { get; set; }

		[StringLength(64)]
		public string LASTUPDATE_ACCOUNT { get; set; }

		public DateTime? LASTUPDATE_DATETIME { get; set; }

		public virtual SYS_USER SYS_USER { get; set; }
	}
}
