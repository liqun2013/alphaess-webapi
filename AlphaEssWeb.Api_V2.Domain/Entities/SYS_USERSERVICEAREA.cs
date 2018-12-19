namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class SYS_USERSERVICEAREA : IEntity<Guid>
	{
		[Key]
		//public Guid SERVICEAREA_ID { get; set; }
		[Column("SERVICEAREA_ID")]
		public Guid Key { get; set; }

		[Required]
		[StringLength(50)]
		public string USER_ID { get; set; }

		[StringLength(50)]
		public string COUNTRY_NAME { get; set; }

		[StringLength(50)]
		public string PROVINCE_NAME { get; set; }

		[StringLength(50)]
		public string CITY_NAME { get; set; }

		public int DELETE_FLAG { get; set; }

		[StringLength(100)]
		public string CREATE_ACCOUNT { get; set; }

		public DateTime? CREATE_DATETIME { get; set; }

		[StringLength(100)]
		public string LASTUPDATE_ACCOUNT { get; set; }

		public DateTime? LASTUPDATE_DATETIME { get; set; }

		[StringLength(8)]
		public string AllArea { get; set; }
	}
}
