namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class SYS_PTYPEDETAIL : IEntity<Guid>
	{
		//[Key]
		//[DatabaseGenerated(DatabaseGeneratedOption.None)]
		//public long PTYPEDETAIL_ID { get; set; }
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		[Column("PTYPEDETAIL_ID")]
		public Guid Key { get; set; }

		public long? PTYPE_ID { get; set; }

		public long? PARENTID { get; set; }

		[StringLength(50)]
		public string CHNAME { get; set; }

		[StringLength(50)]
		public string ENNAME { get; set; }

		public int? SELECTVALUE { get; set; }

		public long? SORTORDER { get; set; }

		[StringLength(200)]
		public string DESCRIPTION { get; set; }

		public virtual SYS_PTYPE SYS_PTYPE { get; set; }
	}
}
