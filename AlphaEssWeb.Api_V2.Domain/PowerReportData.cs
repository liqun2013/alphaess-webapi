namespace AlphaEssWeb.Api_V2.Domain
{
	public sealed class PowerReportData
	{
		/// <summary>
		/// 各时间点对应的电池剩余电量值
		/// </summary>
		public decimal[] Cbat { get; set; }
		/// <summary>
		/// 各时间点对应的并网能量值
		/// </summary>
		public decimal[] FeedIn { get; set; }
		/// <summary>
		/// 各时间点对应的市电充电能量值
		/// </summary>
		public decimal[] GridCharge { get; set; }
		/// <summary>
		/// 各时间点对应的总发电量值
		/// </summary>
		public decimal[] Ppv { get; set; }
		/// <summary>
		/// 各时间点对应的用户负载(Pload)值
		/// </summary>
		public decimal[] UsePower { get; set; }
		/// <summary>
		/// 一天的时间点
		/// </summary>
		public string[] Time { get; set; }
		/// <summary>
		/// 以上数组最后一个有值的索引
		/// </summary>
		public int LastIndex { get; set; }
		/// <summary>
		/// 一天总的并网能量值
		/// </summary>
		public decimal EFeedIn { get; set; }
		/// <summary>
		/// 一天总的市电充电能量值
		/// </summary>
		public decimal EGridCharge { get; set; }
		/// <summary>
		/// 一天总的负载消耗能量
		/// </summary>
		public decimal ELoad { get; set; }
		/// <summary>
		/// 一天总的消耗的电池电量
		/// </summary>
		public decimal EBat { get; set; }
		/// <summary>
		/// 一天总的市电充电能量值
		/// </summary>
		public decimal ECharge { get; set; }
		/// <summary>
		/// 一天总的PV发电量
		/// </summary>
		public decimal EPvTotal { get; set; }
	}
}
