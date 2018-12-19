namespace AlphaEssWeb.Api_V2.Domain
{
	public sealed class ProfitReportData
	{
		/// <summary>
		/// 用户投资成本
		/// </summary>
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

		public void Init(int l)
		{
			InputCost = 0;
			TotalIncome = 0;
			MoneyType = string.Empty;
			SellIncome = new decimal[l];
			BuyIncome = new decimal[l];
			ChargeIncome = new decimal[l];
			DemandCharge = new decimal[l];
			Timeline = new string[l];
		}
	}
}
