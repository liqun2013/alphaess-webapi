namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class SYS_ROLE : IEntity<Guid>
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public SYS_ROLE()
		{
			SYS_MSG = new HashSet<SYS_MSG>();
			SYS_ROLEMENU = new HashSet<SYS_ROLEMENU>();
			SYS_ROLEUSER = new HashSet<SYS_ROLEUSER>();
		}

		//[Key]
		//public Guid ROLE_ID { get; set; }
		[Key]
		[Column("ROLE_ID")]
		public Guid Key { get; set; }

		[StringLength(50)]
		public string ROLENAME { get; set; }

		[StringLength(50)]
		public string ROLEDESC { get; set; }

		[StringLength(100)]
		public string CREATE_ACCOUNT { get; set; }

		public DateTime? CREATE_DATETIME { get; set; }

		[StringLength(100)]
		public string LASTUPDATE_ACCOUNT { get; set; }

		public DateTime? LASTUPDATE_DATETIME { get; set; }

		public Guid? CompanyId { get; set; }

		public int? RoleLevel { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<SYS_MSG> SYS_MSG { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<SYS_ROLEMENU> SYS_ROLEMENU { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<SYS_ROLEUSER> SYS_ROLEUSER { get; set; }
	}
}
