namespace AlphaEssWeb.Api_V2.Domain
{
	public sealed class DeviceInfo
	{
		/// <summary>
		/// 返回状态
		/// </summary>
		public string Status { get; set; }
		/// <summary>
		/// 设备编号 
		/// </summary>
		public string DeviceID { get; set; }
		/// <summary>
		/// 用户电压（V）
		/// </summary>
		public decimal Voltage { get; set; }
		/// <summary>
		/// 当地时区 
		/// </summary>
		public string TimeZone { get; set; }
		/// <summary>
		/// 最后一次 上传时间
		/// </summary>
		public string UpdateTime { get; set; }
		
	}
}
