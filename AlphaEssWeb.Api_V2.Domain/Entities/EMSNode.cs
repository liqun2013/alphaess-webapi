namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Table("EMSNode")]
	public partial class EMSNode : IEntity<Guid>
	{
		[Key]
		[Column("Id")]
		public Guid Key { get; set; }

		public int? MeterBoxNO { get; set; }

		public decimal? Ppv { get; set; }

		public decimal? PrealL1 { get; set; }

		public decimal? PrealL2 { get; set; }

		public decimal? PrealL3 { get; set; }

		public decimal? Pbat { get; set; }

		public decimal? SOC { get; set; }

		public int? FlagBms { get; set; }

		public decimal? Pcharge { get; set; }

		public decimal? Pdischarge { get; set; }

		[StringLength(32)]
		public string EmsStatus { get; set; }

		public DateTime CreateTime { get; set; }

		public Guid MicrogridId { get; set; }

		[StringLength(64)]
		public string SysSn { get; set; }
	}
}
