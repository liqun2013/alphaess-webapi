namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class SYS_MENU : IEntity<Guid>
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public SYS_MENU()
		{
			SYS_MENU1 = new HashSet<SYS_MENU>();
			SYS_ROLEMENU = new HashSet<SYS_ROLEMENU>();
		}

		//[Key]
		//public Guid MENU_ID { get; set; }
		[Key]
		[Column("MENU_ID")]
		public Guid Key { get; set; }

		[StringLength(50)]
		public string MENUNAME { get; set; }

		[StringLength(50)]
		public string MENUNAME_en { get; set; }

		[StringLength(50)]
		public string MENUNAME_de { get; set; }

		[StringLength(200)]
		public string MENUURL { get; set; }

		public Guid? PARENTID { get; set; }

		public int? ORDERINDEX { get; set; }

		[StringLength(100)]
		public string CREATE_ACCOUNT { get; set; }

		public DateTime? CREATE_DATETIME { get; set; }

		[StringLength(100)]
		public string LASTUPDATE_ACCOUNT { get; set; }

		public DateTime? LASTUPDATE_DATETIME { get; set; }

		[StringLength(16)]
		public string MenuType { get; set; }

		[StringLength(64)]
		public string MENUNAME_fr { get; set; }

		[StringLength(64)]
		public string MENUNAME_jp { get; set; }

		public Guid? CompanyId { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<SYS_MENU> SYS_MENU1 { get; set; }

		public virtual SYS_MENU SYS_MENU2 { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<SYS_ROLEMENU> SYS_ROLEMENU { get; set; }
	}
}
