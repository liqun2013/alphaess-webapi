namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class SYS_ROLEUSER : IEntity<Guid>
	{
		[Key]
		//public Guid ROLEMENU_ID { get; set; }
		[Column("ROLEUSER_ID")]
		public Guid Key { get; set; }

		public Guid? ROLEID { get; set; }

		public Guid? USERID { get; set; }

		[StringLength(100)]
		public string CREATE_ACCOUNT { get; set; }

		public DateTime? CREATE_DATETIME { get; set; }

		public virtual SYS_ROLE SYS_ROLE { get; set; }

		public virtual SYS_USER SYS_USER { get; set; }
	}
}
