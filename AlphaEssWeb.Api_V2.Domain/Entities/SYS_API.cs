namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class SYS_API : IEntity<Guid>
	{
		//[Key]
		//public Guid Api_AccountId { get; set; }
		[Key]
		[Column("Api_AccountId")]
		public Guid Key { get; set; }

		[Required]
		[StringLength(64)]
		public string Api_Account { get; set; }

		[Required]
		[StringLength(128)]
		public string Api_SecretKey { get; set; }

		[StringLength(256)]
		public string Api_Description { get; set; }

		public DateTime? Api_CreateTime { get; set; }

		public DateTime? Api_UpdateTime { get; set; }

		public int? Api_Available { get; set; }

		public int? Api_Deleted { get; set; }

		[StringLength(256)]
		[Column("Api_Attr1")]
		public string CompanyId { get; set; }

		//[StringLength(256)]
		//public string Api_Attr2 { get; set; }

		//[StringLength(256)]
		//public string Api_Attr3 { get; set; }

		//[StringLength(256)]
		//public string Api_Attr4 { get; set; }
	}
}
