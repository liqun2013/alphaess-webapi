namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Table("MicrogridInfo")]
	public partial class MicrogridInfo : IEntity<Guid>
	{
		[Key]
		[Column("Id")]
		public Guid Key { get; set; }

		public Guid UserId { get; set; }

		[StringLength(64)]
		public string MicrogridName { get; set; }

		public decimal? MaxGridPower { get; set; }

		public decimal? MaxPEPower { get; set; }

		public decimal? PVPower { get; set; }

		public decimal? WPInstalledPower { get; set; }

		public decimal? DEInstalledPower { get; set; }

		public decimal? OPGInstalledPower { get; set; }

		public int? SystemInstalledCount { get; set; }

		[StringLength(32)]
		public string SystemModel { get; set; }

		public decimal? SystemTotalPower { get; set; }

		public int? NumberOfCells { get; set; }

		[StringLength(32)]
		public string BatteryModel { get; set; }

		public decimal? TotalInstalledCapacity { get; set; }

		public int? RunningState { get; set; }

		[StringLength(64)]
		public string CustomerName { get; set; }

		[StringLength(40)]
		public string CustomerContact { get; set; }

		[StringLength(16)]
		public string CustomerPhone { get; set; }

		[StringLength(40)]
		public string CustomerEmail { get; set; }

		[StringLength(32)]
		public string CustomerCountry { get; set; }

		[StringLength(32)]
		public string CustomerState { get; set; }

		[StringLength(32)]
		public string CustomerCity { get; set; }

		[StringLength(80)]
		public string CustomerAddress { get; set; }

		public DateTime? SetUpTime { get; set; }

		public decimal? SystemTimeSpan { get; set; }

		public DateTime CreateTime { get; set; }

		public DateTime? UpdateTime { get; set; }

		public int? IsDelete { get; set; }
	}
}
