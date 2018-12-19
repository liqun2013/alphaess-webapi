namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("DevData")]
	public partial class DevData : IEntity<Guid>
	{
		[Key]
		[Column("ID")]
		public Guid Key { get; set; }

		[Required]
		[StringLength(20)]
		public string DeviceID { get; set; }

		public DateTime Upload_DateTime { get; set; }

		public decimal CV { get; set; }

		public DateTime? CreateTime { get; set; }

		public decimal? CV1 { get; set; }

		public decimal? CV2 { get; set; }

		public decimal? CV3 { get; set; }

	}
}
