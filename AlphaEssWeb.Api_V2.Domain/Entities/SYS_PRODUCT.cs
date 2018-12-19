namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class SYS_PRODUCT : IEntity<Guid>
	{
		//[Key]
		//public Guid PROD_ID { get; set; }
		[Key]
		[Column("PROD_ID")]
		public Guid Key { get; set; }

		[StringLength(100)]
		public string PRODNAME { get; set; }

		[StringLength(4000)]
		public string PRODDESC { get; set; }

		public int DELETE_FLAG { get; set; }

		[StringLength(100)]
		public string CREATE_ACCOUNT { get; set; }

		public DateTime? CREATE_DATETIME { get; set; }

		[StringLength(100)]
		public string LASTUPDATE_ACCOUNT { get; set; }

		public DateTime? LASTUPDATE_DATETIME { get; set; }

		[StringLength(200)]
		public string PRODPIC { get; set; }

		[Column(TypeName = "text")]
		public string PRODSHORTDESC { get; set; }
		[StringLength(250)]
		public string ProductCategoryId { get; set; }

		[StringLength(250)]
		public string ProductCategoryName { get; set; }
		public Guid? CompanyId { get; set; }
	}
}
