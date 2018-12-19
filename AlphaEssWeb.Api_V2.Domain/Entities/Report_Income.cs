namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class Report_Income : IEntity<long>
	{
		[Key]
		[Column("IncomeId")]
		public long Key { get; set; }

		[Required]
		[StringLength(64)]
		public string SysSn { get; set; }

		//[Column(TypeName = "date")]
		public DateTime TheDate { get; set; }

		public decimal? SellIncome { get; set; }

		public decimal? BuyIncome { get; set; }

		public decimal? ChargeIncome { get; set; }

		public decimal? DemandCharge { get; set; }

		public decimal? ToalIncome { get; set; }

		public short? WorkMode { get; set; }
	}
}
