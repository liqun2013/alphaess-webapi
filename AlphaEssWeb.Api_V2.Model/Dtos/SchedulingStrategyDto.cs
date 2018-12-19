using System;

namespace AlphaEssWeb.Api_V2.Model.Dtos
{
	public sealed class SchedulingStrategyDto : IDto<Guid>
	{
		public Guid Id { get; set; }

		public Guid MicrogridId { get; set; }

		public decimal PGridMax { get; set; }

		public decimal POutputMax { get; set; }

		public decimal DieselStartSOC { get; set; }

		public decimal DieselPOutputMax { get; set; }

		public decimal DieselStopSOC { get; set; }

		public decimal DieselStopPower { get; set; }

		public decimal SOC1 { get; set; }

		public decimal SOC2 { get; set; }

		public decimal SOC3 { get; set; }

		public decimal SOC4 { get; set; }

		public decimal SOC5 { get; set; }

		public decimal SOC6 { get; set; }

		public decimal SOC7 { get; set; }

		public decimal Power1 { get; set; }

		public decimal Power2 { get; set; }

		public decimal Power3 { get; set; }

		public decimal Power4 { get; set; }

		public decimal Power5 { get; set; }

		public int ChargingStart1 { get; set; }

		public int ChargingEnd1 { get; set; }

		public int ChargingStart2 { get; set; }

		public int ChargingEnd2 { get; set; }

		public int DischargeStart1 { get; set; }

		public int DischargeEnd1 { get; set; }

		public int DischargeStart2 { get; set; }

		public int DischargeEnd2 { get; set; }

		public decimal ChargingSOCPoint1 { get; set; }

		public decimal ChargingSOCPoint2 { get; set; }

		public decimal DischargeSOCPoint1 { get; set; }

		public decimal DischargeSOCPoint2 { get; set; }
	}
}
