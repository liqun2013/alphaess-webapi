using AlphaEssWeb.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Model.Dtos;

namespace AlphaEssWeb.Api_V2.Model
{
	public static class SchedulingStrategyExtension
	{
		public static SchedulingStrategyDto ToSchedulingStrategyDto(this SchedulingStrategy ss)
		{
			return new SchedulingStrategyDto
			{
				ChargingEnd1 = ss.ChargingEnd1,
				ChargingEnd2 = ss.ChargingEnd2,
				ChargingSOCPoint1 = ss.ChargingSOCPoint1,
				ChargingSOCPoint2 = ss.ChargingSOCPoint2,
				ChargingStart1 = ss.ChargingStart1,
				ChargingStart2 = ss.ChargingStart2,
				DieselPOutputMax = ss.DieselPOutputMax,
				DieselStartSOC = ss.DieselStartSOC,
				DieselStopPower = ss.DieselStopPower,
				DieselStopSOC = ss.DieselStopSOC,
				DischargeEnd1 = ss.DischargeEnd1,
				DischargeEnd2 = ss.DischargeEnd2,
				DischargeSOCPoint1 = ss.DischargeSOCPoint1,
				DischargeSOCPoint2 = ss.DischargeSOCPoint2,
				DischargeStart1 = ss.DischargeStart1,
				DischargeStart2 = ss.DischargeStart2,
				Id = ss.Key,
				MicrogridId = ss.MicrogridId,
				PGridMax = ss.PGridMax,
				POutputMax = ss.POutputMax,
				Power1 = ss.Power1,
				Power2 = ss.Power2,
				Power3 = ss.Power3,
				Power4 = ss.Power4,
				Power5 = ss.Power5,
				SOC1 = ss.SOC1,
				SOC2 = ss.SOC2,
				SOC3 = ss.SOC3,
				SOC4 = ss.SOC4,
				SOC5 = ss.SOC5,
				SOC6 = ss.SOC6,
				SOC7 = ss.SOC7
			};
		}
	}
}
