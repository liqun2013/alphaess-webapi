namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class SYS_USER : IEntity<Guid>
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public SYS_USER()
		{
			SYS_EMAILRULEUSER = new HashSet<SYS_EMAILRULEUSER>();
			SYS_MSG = new HashSet<SYS_MSG>();
			SYS_MSGUSER = new HashSet<SYS_MSGUSER>();
			SYS_MSGUSER1 = new HashSet<SYS_MSGUSER>();
			SYS_ROLEUSER = new HashSet<SYS_ROLEUSER>();
			VT_SYSTEM = new HashSet<VT_SYSTEM>();
		}

		[Key]
		//public Guid USER_ID { get; set; }
		[Column("USER_ID")]
		public Guid Key { get; set; }

		[StringLength(50)]
		public string USERTYPE { get; set; }

		[StringLength(50)]
		public string LICNO { get; set; }

		[StringLength(50)]
		public string USERNAME { get; set; }

		[StringLength(50)]
		public string REALNAME { get; set; }

		[StringLength(50)]
		public string USERPWD { get; set; }

		[StringLength(100)]
		public string EMAIL { get; set; }

		[StringLength(50)]
		public string COUNTRYCODE { get; set; }

		[StringLength(50)]
		public string STATECODE { get; set; }

		[StringLength(50)]
		public string CITYCODE { get; set; }

		[StringLength(50)]
		public string ADDRESS { get; set; }

		[StringLength(50)]
		public string POSTCODE { get; set; }

		[StringLength(50)]
		public string TIMEZONECODE { get; set; }

		[StringLength(50)]
		public string LINKMAN { get; set; }

		[StringLength(50)]
		public string CELLPHONE { get; set; }

		[StringLength(50)]
		public string FAX { get; set; }

		public int DELETE_FLAG { get; set; }

		[StringLength(100)]
		public string CREATE_ACCOUNT { get; set; }

		public DateTime? CREATE_DATETIME { get; set; }

		[StringLength(100)]
		public string LASTUPDATE_ACCOUNT { get; set; }

		public DateTime? LASTUPDATE_DATETIME { get; set; }

		[StringLength(64)]
		public string Inviter { get; set; }

		public int? Validity { get; set; }

		public DateTime? ShareTime { get; set; }

		[StringLength(256)]
		public string HeaderPath { get; set; }
		public int SERVICEAREA_FLAG { get; set; }

		[StringLength(50)]
		public string LANGUAGECODE { get; set; }

		public int LICINVALID_FLAG { get; set; }

		public Guid? CompanyId { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<SYS_EMAILRULEUSER> SYS_EMAILRULEUSER { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<SYS_MSG> SYS_MSG { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<SYS_MSGUSER> SYS_MSGUSER { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<SYS_MSGUSER> SYS_MSGUSER1 { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<SYS_ROLEUSER> SYS_ROLEUSER { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<VT_SYSTEM> VT_SYSTEM { get; set; }
	}
}
