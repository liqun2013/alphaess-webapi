using System;
using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class ExternalUpdateSchedulingStrategyRequestModel : ExternalBaseRequestModel
	{
		[Required]
		public Guid MicrogridId { get; set; }
		[Required]
		public decimal PGridMax { get; set; }
		[Required]
		public decimal POutputMax { get; set; }
		[Required]
		public decimal DieselStartSOC { get; set; }
		[Required]
		public decimal DieselPOutputMax { get; set; }
		[Required]
		public decimal DieselStopSOC { get; set; }
		[Required]
		public decimal DieselStopPower { get; set; }
		[Required]
		public decimal SOC1 { get; set; }
		[Required]
		public decimal SOC2 { get; set; }
		[Required]
		public decimal SOC3 { get; set; }
		[Required]
		public decimal SOC4 { get; set; }
		[Required]
		public decimal SOC5 { get; set; }
		[Required]
		public decimal SOC6 { get; set; }
		[Required]
		public decimal SOC7 { get; set; }
		[Required]
		public decimal Power1 { get; set; }
		[Required]
		public decimal Power2 { get; set; }
		[Required]
		public decimal Power3 { get; set; }
		[Required]
		public decimal Power4 { get; set; }
		[Required]
		public decimal Power5 { get; set; }
		[Required]
		public int ChargingStart1 { get; set; }
		[Required]
		public int ChargingEnd1 { get; set; }
		[Required]
		public int ChargingStart2 { get; set; }
		[Required]
		public int ChargingEnd2 { get; set; }
		[Required]
		public int DischargeStart1 { get; set; }
		[Required]
		public int DischargeEnd1 { get; set; }
		[Required]
		public int DischargeStart2 { get; set; }
		[Required]
		public int DischargeEnd2 { get; set; }
		[Required]
		public decimal ChargingSOCPoint1 { get; set; }
		[Required]
		public decimal ChargingSOCPoint2 { get; set; }
		[Required]
		public decimal DischargeSOCPoint1 { get; set; }
		[Required]
		public decimal DischargeSOCPoint2 { get; set; }
	}
}
