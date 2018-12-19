using System;

namespace AlphaEssWeb.Api_V2.Model.Dtos
{
	public sealed class SysWeatherForecastDto : IDto<Guid>
	{
		public Guid Id { get; set; }
		public int temp_min { get; set; }
		public int temp_max { get; set; }
		public decimal? pressure { get; set; }
		public decimal? humidity { get; set; }
		public string weather { get; set; }
		public decimal? clouds { get; set; }
		public decimal? wind_speed { get; set; }
		public DateTime? DATETIME { get; set; }
		public string remark { get; set; }
	}
}
