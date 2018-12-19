namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class Report_Energy : IEntity<long>
	{
		[Key]
		[Column("EnergyId")]
		public long Key { get; set; }

		[Required]
		[StringLength(64)]
		public string SysSn { get; set; }

		public DateTime TheDate { get; set; }

		public decimal? Epvtotal { get; set; }

		public decimal? Eoutput { get; set; }

		public decimal? Echarge { get; set; }

		public decimal? Epv2Load { get; set; }

		public decimal? Eeff { get; set; }

		public decimal? Eload { get; set; }

		public decimal? EGridCharge { get; set; }

		public decimal? EGrid2Load { get; set; }

		public decimal? EselfConsumption { get; set; }

		public decimal? EselfSufficiency { get; set; }

		public decimal? Einput { get; set; }

		public decimal? Ebat { get; set; }

		public decimal? Soc { get; set; }

		public decimal? Ediesel { get; set; }

		public short? WorkMode { get; set; }
		[NotMapped]
		public decimal? TotalIncome { get; set; }
	}
}
