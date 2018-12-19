using System;

namespace AlphaEssWeb.Api_V2.Model.Dtos
{
	public sealed class MicrogridInfoDto: IDto<Guid>
	{
		public Guid Id { get; set; }

		//public Guid UserId { get; set; }

		public string MicrogridName { get; set; }

		public decimal? MaxGridPower { get; set; }

		public decimal? MaxPEPower { get; set; }

		public decimal? PVPower { get; set; }

		public decimal? WPInstalledPower { get; set; }

		public decimal? DEInstalledPower { get; set; }

		public decimal? OPGInstalledPower { get; set; }

		public int? SystemInstalledCount { get; set; }

		public string SystemModel { get; set; }

		public decimal? SystemTotalPower { get; set; }

		public int? NumberOfCells { get; set; }

		public string BatteryModel { get; set; }

		public decimal? TotalInstalledCapacity { get; set; }

		public int? RunningState { get; set; }

		public string CustomerName { get; set; }

		public string CustomerContact { get; set; }

		public string CustomerPhone { get; set; }

		public string CustomerEmail { get; set; }

		public string CustomerCountry { get; set; }

		public string CustomerState { get; set; }

		public string CustomerCity { get; set; }

		public string CustomerAddress { get; set; }

		public DateTime? SetUpTime { get; set; }

		public decimal? SystemTimeSpan { get; set; }

		//public DateTime CreateTime { get; set; }

		//public DateTime? UpdateTime { get; set; }
	}
}
