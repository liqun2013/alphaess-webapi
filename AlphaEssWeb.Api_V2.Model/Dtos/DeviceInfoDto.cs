namespace AlphaEssWeb.Api_V2.Model.Dtos
{
	public sealed class DeviceInfoDto
	{
		public string Status { get; set; }
		public string DeviceID { get; set; }
		public decimal Voltage { get; set; }
		public string TimeZone { get; set; }
		public string UpdateTime { get; set; }
	}
}
