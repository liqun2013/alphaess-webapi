namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Table("MeterRecord")]
	public partial class MeterRecord : IEntity<Guid>
	{
		[Key]
		[Column("Id")]
		public Guid Key { get; set; }

		public Guid MicrogridId { get; set; }

		[StringLength(64)]
		public string MeterUses { get; set; }

		public decimal? PMeterL1 { get; set; }

		public decimal? PMeterL2 { get; set; }

		public decimal? PMeterL3 { get; set; }

		public int? DERunningState { get; set; }

		[StringLength(128)]
		public string DEAlarm { get; set; }

		public DateTime CreateTime { get; set; }

		[StringLength(64)]
		public string SysSn { get; set; }

		public decimal? Einput { get; set; }

		public decimal? Eoutput { get; set; }
	}
}
