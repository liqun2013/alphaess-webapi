using System;

namespace AlphaEssWeb.Api_V2.Model.Dtos
{
	public sealed class MicrogridSummaryDto : IDto<Guid>
	{
		public Guid Id { get; set; }

		public Guid MicrogridId { get; set; }

		public int OnlineSystemCount { get; set; }

		public int ExceptionSystemCount { get; set; }

		public decimal? BuyingPower { get; set; }

		public decimal? SellingPower { get; set; }

		public decimal? LoadPower { get; set; }

		public decimal? GeneratedOutput { get; set; }

		public decimal? RemainingCapacity { get; set; }

		public decimal? SOC { get; set; }

		public decimal? Pdischarge { get; set; }

		public decimal? Pcharge { get; set; }

		//public DateTime? UpdateTime { get; set; }

		//public DateTime CreateTime { get; set; }

		public decimal? BuyElectricPower { get; set; }

		public decimal? SellElectricPower { get; set; }

		public decimal? PowerGeneration { get; set; }

		public decimal? ConsumptionPower { get; set; }
	}
}
