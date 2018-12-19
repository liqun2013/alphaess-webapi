namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class SYS_MSGUSER : IEntity<Guid>
	{
		//[Key]
		//public Guid MSGUSER_ID { get; set; }
		[Key]
		[Column("MSGUSER_ID")]
		public Guid Key { get; set; }

		public Guid? USER_ID { get; set; }

		public int USERREAD_FLAG { get; set; }

		public int USERDEL_FLAG { get; set; }

		public int DELETE_FLAG { get; set; }

		[StringLength(100)]
		public string CREATE_ACCOUNT { get; set; }

		public DateTime? CREATE_DATETIME { get; set; }

		[StringLength(100)]
		public string LASTUPDATE_ACCOUNT { get; set; }

		public DateTime? LASTUPDATE_DATETIME { get; set; }

		public Guid? SENDUSER_ID { get; set; }

		public Guid? MSG_ID { get; set; }

		public virtual SYS_MSG SYS_MSG { get; set; }

		public virtual SYS_USER SYS_USER { get; set; }

		public virtual SYS_USER SYS_USER1 { get; set; }
	}
}
