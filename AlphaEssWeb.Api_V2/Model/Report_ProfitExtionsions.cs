using AlphaEssWeb.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Model.Dtos;

namespace AlphaEssWeb.Api_V2.Model
{
	internal static class Report_ProfitExtionsions
	{
		public static ProfitReportDataDto ToProfitReportDataDto(this ProfitReportData prd)
		{
			return new ProfitReportDataDto
			{
				Timeline = prd.Timeline, BuyIncome = prd.BuyIncome, ChargeIncome = prd.ChargeIncome, DemandCharge = prd.DemandCharge, InputCost = prd.InputCost, SellIncome = prd.SellIncome,
				MoneyType = prd.MoneyType, TotalIncome = prd.TotalIncome
			};
		}
	}
}
