namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class SYS_LOG : IEntity<Guid>
	{
		//[Key]
		//public Guid LOG_ID { get; set; }
		[Key]
		[Column("LOG_ID")]
		public Guid Key { get; set; }

		public DateTime? CREATE_DATETIME { get; set; }

		[StringLength(32)]
		public string THREAD { get; set; }

		[StringLength(32)]
		public string LEVEL { get; set; }

		[StringLength(128)]
		public string LOGGER { get; set; }

		[StringLength(4000)]
		public string MESSAGE { get; set; }

		[StringLength(4000)]
		public string EXCEPTION { get; set; }

		[StringLength(64)]
		public string CREATE_ACCOUNT { get; set; }

		public int? LOG_TYPE { get; set; }

		[StringLength(64)]
		public string LOG_CONTENT { get; set; }

	}
}
