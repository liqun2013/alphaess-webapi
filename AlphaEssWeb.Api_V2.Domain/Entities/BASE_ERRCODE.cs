namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class BASE_ERRCODE : IEntity<Guid>
	{
		//[Key]
		//public Guid ERRCODE_ID { get; set; }
		[Key]
		[Column("ERRCODE_ID")]
		public Guid Key { get; set; }

		[StringLength(64)]
		public string CODE { get; set; }

		[StringLength(256)]
		public string DESC { get; set; }

		public int DELETE_FLAG { get; set; }

		[StringLength(64)]
		public string CREATE_ACCOUNT { get; set; }

		public DateTime? CREATE_DATETIME { get; set; }

		[StringLength(64)]
		public string LASTUPDATE_ACCOUNT { get; set; }

		public DateTime? LASTUPDATE_DATETIME { get; set; }

		//[StringLength(250)]
		//public string ATTR1 { get; set; }

		//[StringLength(250)]
		//public string ATTR2 { get; set; }

		//[StringLength(250)]
		//public string ATTR3 { get; set; }

		//[StringLength(250)]
		//public string ATTR4 { get; set; }

		//[StringLength(250)]
		//public string ATTR5 { get; set; }
	}
}
