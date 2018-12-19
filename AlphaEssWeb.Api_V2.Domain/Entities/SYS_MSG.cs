namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class SYS_MSG : IEntity<Guid>
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public SYS_MSG()
		{
			SYS_MSGUSER = new HashSet<SYS_MSGUSER>();
		}

		//[Key]
		//public Guid MSG_ID { get; set; }
		[Key]
		[Column("MSG_ID")]
		public Guid Key { get; set; }

		[StringLength(100)]
		public string MSGTITLE { get; set; }

		[Column(TypeName = "text")]
		public string MSGCONT { get; set; }

		public Guid? ROLE_ID { get; set; }

		public int DELETE_FLAG { get; set; }

		public DateTime? CREATE_DATETIME { get; set; }

		[StringLength(100)]
		public string LASTUPDATE_ACCOUNT { get; set; }

		public DateTime? LASTUPDATE_DATETIME { get; set; }

		public int? MsgType { get; set; }

		public int? MsgLevel { get; set; }

		public Guid? CREATE_ACCOUNT { get; set; }

		public Guid? CompanyId { get; set; }
		public virtual SYS_ROLE SYS_ROLE { get; set; }

		public virtual SYS_USER SYS_USER { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<SYS_MSGUSER> SYS_MSGUSER { get; set; }
	}
}
