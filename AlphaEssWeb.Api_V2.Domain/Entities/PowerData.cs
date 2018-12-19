namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("PowerData")]
	public partial class PowerData : IEntity<Guid>
	{
		[Key]
		[Column("Id")]
		public Guid Key { get; set; }

		public DateTime? CreateTime { get; set; }

		public DateTime? UploadTime { get; set; }

		[StringLength(50)]
		public string Sn { get; set; }
		[Column("PPV1")]
		public decimal? Ppv1 { get; set; }
		[Column("PPV2")]
		public decimal? Ppv2 { get; set; }
		[Column("PREAL_L1")]
		public decimal? PrealL1 { get; set; }
		[Column("PREAL_L2")]
		public decimal? PrealL2 { get; set; }
		[Column("PREAL_L3")]
		public decimal? PrealL3 { get; set; }
		[Column("PMETER_L1")]
		public decimal? PmeterL1 { get; set; }
		[Column("PMETER_L2")]
		public decimal? PmeterL2 { get; set; }
		[Column("PMETER_L3")]
		public decimal? PmeterL3 { get; set; }
		[Column("PMETER_DC")]
		public decimal? PmeterDc { get; set; }
		[Column("SOC")]
		public decimal? Soc { get; set; }
		[Column("FACTORY_FLAG")]
		public int? FactoryFlag { get; set; }
		[Column("Pbat")]
		public decimal? Pbat { get; set; }
		[Column("Sva")]
		public decimal? Sva { get; set; }
		[Column("VarAC")]
		public decimal? VarAC { get; set; }
		[Column("VarDC")]
		public decimal? VarDC { get; set; }

	}
}
