namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class SYS_PTYPE : IEntity<Guid>
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public SYS_PTYPE()
		{
			SYS_PTYPEDETAIL = new HashSet<SYS_PTYPEDETAIL>();
		}

		//[Key]
		//[DatabaseGenerated(DatabaseGeneratedOption.None)]
		//public long PTYPE_ID { get; set; }
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		[Column("PTYPE_ID")]
		public Guid Key { get; set; }

		[StringLength(50)]
		public string CODE { get; set; }

		[StringLength(50)]
		public string CODENAME { get; set; }

		public bool? ISSIMPLE { get; set; }

		public bool? ISREAD { get; set; }

		[StringLength(200)]
		public string DESCRIPTION { get; set; }

		public bool? ISDELETED { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<SYS_PTYPEDETAIL> SYS_PTYPEDETAIL { get; set; }
	}
}
