namespace AlphaEssWeb.Api_V2.Domain
{
	public sealed class EnergyReportData
	{
		/// <summary>
		/// 电网供负载总能量值
		/// </summary>
		public decimal[] EGrid2Load { get; set; }
		/// <summary>
		/// 电网给电池充电能量值
		/// </summary>
		public decimal[] EGridCharge { get; set; }
		/// <summary>
		/// 电池用电量
		/// </summary>
		public decimal[] Ebat { get; set; }
		/// <summary>
		/// pv给电池总的充电能量值
		/// </summary>
		public decimal[] Echarge { get; set; }
		/// <summary>
		/// 自发自用能量值
		/// </summary>
		public decimal[] Eeff { get; set; }
		/// <summary>
		/// 电表入户能量值
		/// </summary>
		public decimal[] Einput { get; set; }
		/// <summary>
		/// 负载总消耗能量
		/// </summary>
		public decimal[] Eload { get; set; }
		/// <summary>
		/// 电表出户能量
		/// </summary>
		public decimal[] Eout { get; set; }
		/// <summary>
		/// PV供负载总能量值
		/// </summary>
		public decimal[] Epv2load { get; set; }
		/// <summary>
		/// PV输入累计能量
		/// </summary>
		public decimal[] EpvT { get; set; }
		/// <summary>
		/// 自发自用比例
		/// </summary>
		public decimal EselfConsumption { get; set; }
		/// <summary>
		/// 自给自足比例
		/// </summary>
		public decimal EselfSufficiency { get; set; }
		public string[] Timeline { get; set; }

		public void Init(int l)
		{
			Ebat = new decimal[l];
			EGrid2Load = new decimal[l];
			EGridCharge = new decimal[l];
			Echarge = new decimal[l];
			Eeff = new decimal[l];
			Einput = new decimal[l];
			Eload = new decimal[l];
			Eout = new decimal[l];
			Epv2load = new decimal[l];
			EpvT = new decimal[l];
			EselfConsumption = 0;
			EselfSufficiency = 0;
			Timeline = new string[l];
		}
	}
}
