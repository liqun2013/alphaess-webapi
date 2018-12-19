namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class SYS_APPVERSIONREC : IEntity<Guid>
	{
		//[Key]
		//public Guid APPVERSIONREC_ID { get; set; }
		[Key]
		[Column("APPVERSIONREC_ID")]
		public Guid Key { get; set; }
		public Guid? APPVERSION_ID { get; set; }

		[StringLength(256)]
		public string SNS { get; set; }

		public int? EXECUTE_FLAG { get; set; }

		[StringLength(64)]
		public string LICENSE { get; set; }

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

		public Guid? InvVersionId { get; set; }
		[StringLength(64)]
		public string COUNTRY { get; set; }

		[StringLength(64)]
		public string VER_SET { get; set; }
		public Guid? CompanyId { get; set; }
	}
}
