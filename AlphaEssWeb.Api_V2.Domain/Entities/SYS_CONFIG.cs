namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class SYS_CONFIG : IEntity<Guid>
	{
		//[Key]
		//public Guid CONFIG_ID { get; set; }
		[Key]
		[Column("CONFIG_ID")]
		public Guid Key { get; set; }

		[StringLength(64)]
		public string KEYNAME { get; set; }

		[StringLength(128)]
		public string KEYVAL { get; set; }

		[StringLength(64)]
		public string CREATE_ACCOUNT { get; set; }

		public DateTime? CREATE_DATETIME { get; set; }

		[StringLength(64)]
		public string LASTUPDATE_ACCOUNT { get; set; }

		public DateTime? LASTUPDATE_DATETIME { get; set; }

	}
}
