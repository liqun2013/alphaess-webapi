namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class BASE_COUNTRY : IEntity<int>
	{
		//[Key]
		//public int AREA_ID { get; set; }
		[Key]
		[Column("AREA_ID")]
		public int Key { get; set; }

		[StringLength(64)]
		public string AREA_CODE { get; set; }

		[StringLength(64)]
		public string AREA_NAME { get; set; }

		[StringLength(32)]
		public string TIMEZONE_CODE { get; set; }

		[StringLength(32)]
		public string LANG { get; set; }

		[StringLength(32)]
		public string MONEY { get; set; }

		[StringLength(64)]
		public string BEGIP { get; set; }

		[StringLength(64)]
		public string ENDIP { get; set; }

		public int? PARENT_ID { get; set; }

		public int DELETE_FLAG { get; set; }

		[StringLength(40)]
		public string CREATE_ACCOUNT { get; set; }

		public DateTime? CREATE_DATETIME { get; set; }

		[StringLength(40)]
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
