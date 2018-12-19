using System;

namespace AlphaEssWeb.Api_V2.Model.Dtos
{
	public sealed class ReportEnergyDto : IDto<Guid>
	{
		public Guid Id { get; set; }
		/// <summary>
		/// 系统sn号
		/// </summary>
		public string Sn { get; set; }

		/// <summary>
		/// 当天电网供负载总能量值
		/// </summary>
		public decimal? EGrid2Load { get; set; }

		/// <summary>
		/// 当天电网给电池充电能量值
		/// </summary>
		public decimal? EGridCharge { get; set; }

		/// <summary>
		/// 当天电池最后电量
		/// </summary>
		public decimal? Ebat { get; set; }

		/// <summary>
		/// 当天充电电量
		/// </summary>
		public decimal? Echarge { get; set; }

		/// <summary>
		/// 当天自发自用能量值
		/// </summary>
		public decimal? Eeff { get; set; }

		/// <summary>
		/// 当天买电电量
		/// </summary>
		public decimal? Einput { get; set; }

		/// <summary>
		/// 当天负载总消耗能量
		/// </summary>
		public decimal? Eload { get; set; }

		/// <summary>
		/// 当天卖电量
		/// </summary>
		public decimal? Eoutput { get; set; }

		/// <summary>
		/// 当天PV供负载总能量值
		/// </summary>
		public decimal? Epv2Load { get; set; }

		/// <summary>
		/// 当天逆变器发电量
		/// </summary>
		public decimal? Epvtotal { get; set; }

		/// <summary>
		/// 当天自发自用比例
		/// </summary>
		public decimal? EselfConsumption { get; set; }

		/// <summary>
		/// 当天自给自足比例
		/// </summary>
		public decimal? EselfSufficiency { get; set; }
		/// <summary>
		/// 当天收益
		/// </summary>
		public decimal? TotalIncome { get; set; }
	}

	public sealed class EnergyReportDataDto : IDto<Guid>
	{
		public Guid Id { get; set; }
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
	}
}
