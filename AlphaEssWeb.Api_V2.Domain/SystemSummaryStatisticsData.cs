namespace AlphaEssWeb.Api_V2.Domain
{
	public sealed class SystemSummaryStatisticsData
	{
		public int OfflineCount { get; set; }
		public int NormalCount { get; set; }
		public int ProtectionCount { get; set; }
		public int FaultCount { get; set; }

		public decimal Epvtotal { get; set; }
		public decimal Eoutput { get; set; }
		public decimal Ebat { get; set; }
		public decimal Einput { get; set; }
		public decimal Eload { get; set; }
		public decimal EchargeT { get; set; }
		public decimal Epv2Load { get; set; }
		public decimal EselfConsumption { get; set; }
		public decimal EselfSufficiency { get; set; }
		public decimal Cobat { get; set; }
		public decimal Poinv { get; set; }
	}
}
