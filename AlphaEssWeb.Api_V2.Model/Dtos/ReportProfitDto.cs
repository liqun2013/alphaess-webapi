using System;

namespace AlphaEssWeb.Api_V2.Model.Dtos
{
	public sealed class ProfitReportDataDto : IDto<Guid>
	{
		public Guid Id { get; set; }
		public decimal InputCost { get; set; }
		public decimal TotalIncome { get; set; }
		public string MoneyType { get; set; }
		/// <summary>
		/// 卖电收益
		/// </summary>
		public decimal[] SellIncome { get; set; }
		/// <summary>
		/// 买电收益
		/// </summary>
		public decimal[] BuyIncome { get; set; }
		/// <summary>
		/// 谷充峰用收益
		/// </summary>
		public decimal[] ChargeIncome { get; set; }
		/// <summary>
		/// Demand charge收益
		/// </summary>
		public decimal[] DemandCharge { get; set; }

		public string[] Timeline { get; set; }
	}
}
