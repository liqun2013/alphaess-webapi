using AlphaEssWeb.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Model.Dtos;

namespace AlphaEssWeb.Api_V2.Model
{
	internal static class MicrogridSummaryExtension
	{
		public static MicrogridSummaryDto ToMicrogridSummaryDto(this MicrogridSummary ms)
		{
			return new MicrogridSummaryDto
			{
				BuyElectricPower = ms.BuyElectricPower,
				BuyingPower = ms.BuyingPower,
				ConsumptionPower = ms.ConsumptionPower,
				ExceptionSystemCount = ms.ExceptionSystemCount,
				GeneratedOutput = ms.GeneratedOutput,
				Id = ms.Key,
				LoadPower = ms.LoadPower,
				MicrogridId = ms.MicrogridId,
				OnlineSystemCount = ms.OnlineSystemCount,
				Pcharge = ms.Pcharge,
				Pdischarge = ms.Pdischarge,
				PowerGeneration = ms.PowerGeneration,
				RemainingCapacity = ms.RemainingCapacity,
				SellElectricPower = ms.SellElectricPower,
				SellingPower = ms.SellingPower,
				SOC = ms.SOC
			};
		}
	}
}
