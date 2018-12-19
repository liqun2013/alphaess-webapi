namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class SYS_APPVERSION : IEntity<Guid>
	{
		//[Key]
		//public Guid APPVERSION_ID { get; set; }
		[Key]
		[Column("APPVERSION_ID")]
		public Guid Key { get; set; }

		[StringLength(32)]
		public string APP_TYPE { get; set; }

		[StringLength(40)]
		public string APP_VERSION { get; set; }

		public int? UPDATE_FLAG { get; set; }

		[StringLength(192)]
		public string UPDATE_URL { get; set; }

		public int DELETE_FLAG { get; set; }

		[StringLength(64)]
		public string CREATE_ACCOUNT { get; set; }

		public DateTime? CREATE_DATETIME { get; set; }

		[StringLength(64)]
		public string LASTUPDATE_ACCOUNT { get; set; }

		public DateTime? LASTUPDATE_DATETIME { get; set; }

		[StringLength(64)]
		public string DEV_VERSION { get; set; }

		[StringLength(192)]
		public string VER_REMARK { get; set; }

		public Guid? CompanyId { get; set; }

		[StringLength(128)]
		public string HashMd5 { get; set; }
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
