namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Table("SchedulingStrategy")]
	public partial class SchedulingStrategy : IEntity<Guid>
	{
		[Key]
		[Column("Id")]
		public Guid Key { get; set; }

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

		public DateTime CreateTime { get; set; }

		public DateTime UpdateTime { get; set; }

		public int IsDelete { get; set; }
	}
}
