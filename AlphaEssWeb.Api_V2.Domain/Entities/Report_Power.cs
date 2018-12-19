namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("Report_Power")]
	public partial class Report_Power : IEntity<long>
	{
		[Key]
		[Column("PowerId")]
		public long Key { get; set; }

		[StringLength(64)]
		public string SysSn { get; set; }

		public DateTime TheDate { get; set; }

		public int TimelineIndex { get; set; }

		public decimal? Ppv { get; set; }

		public decimal? UsePower { get; set; }

		public decimal? Cbat { get; set; }

		public decimal? FeedIn { get; set; }

		public decimal? GridCharge { get; set; }

		public decimal? DieselPv { get; set; }

		public short? WorkMode { get; set; }

	}
}
