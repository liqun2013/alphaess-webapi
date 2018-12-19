namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("DevInfo")]
	public partial class DevInfo : IEntity<Guid>
	{
		[Key]
		[Column("ID")]
		public Guid Key { get; set; }

		[Required]
		[StringLength(20)]
		public string DeviceID { get; set; }

		[StringLength(20)]
		public string UserName { get; set; }

		[StringLength(20)]
		public string TimeZone { get; set; }

		public decimal Voltage { get; set; }

		[StringLength(50)]
		public string Remark { get; set; }

		[StringLength(20)]
		public string IpAddress { get; set; }

		public DateTime? CreateTime { get; set; }

		public DateTime? UpdateTime { get; set; }

		public int? Delete_Flag { get; set; }

	}
}
