namespace AlphaEssWeb.Api_V2.Domain
{
	public sealed class DeviceCV
	{
		/// <summary>
		/// 返回状态
		/// </summary>
		public string Status { get; set; }
		/// <summary>
		/// 上传数据时间点(HH:mm:ss)
		/// </summary>
		public string[] Time { get; set; }
		/// <summary>
		/// 电流总和
		/// </summary>
		public decimal[] CVTotal { get; set; }
		/// <summary>
		/// L1 电流值
		/// </summary>
		public decimal[] CV1 { get; set; }
		/// <summary>
		/// L2 电流值
		/// </summary>
		public decimal[] CV2 { get; set; }
		/// <summary>
		/// L3 电流值
		/// </summary>
		public decimal[] CV3 { get; set; }
	
	}
}
