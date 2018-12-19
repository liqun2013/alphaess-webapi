using System;

namespace AlphaEssWeb.Api_V2.Model.Dtos
{
	public sealed class PowerDataDto : IDto<Guid>
	{
		public Guid Id { get; set; }

		/// <summary>
		/// 储能系统 S/N
		/// </summary>
		public string Sn { get; set; }

		/// <summary>
		/// 数据上传时间
		/// </summary>
		public DateTime? UploadTime { get; set; }

		/// <summary>
		/// PV 输入功率 1 
		/// </summary>
		public decimal? Ppv1 { get; set; }

		/// <summary>
		///  PV 输入功率 2
		/// </summary>
		public decimal? Ppv2 { get; set; }

		/// <summary>
		/// 逆变器 L1 实时输出功率，该参数有正负
		/// </summary>
		public decimal? PrealL1 { get; set; }

		/// <summary>
		/// 逆变器 L2 实时输出功率，该参数有正负
		/// </summary>
		public decimal? PrealL2 { get; set; }

		/// <summary>
		/// 逆变器 L3 实时输出功率，该参数有正负
		/// </summary>
		public decimal? PrealL3 { get; set; }

		/// <summary>
		/// 电表 L1 实时功率
		/// </summary>
		public decimal? PmeterL1 { get; set; }

		/// <summary>
		/// 电表 L2 实时功率
		/// </summary>
		public decimal? PmeterL2 { get; set; }

		/// <summary>
		/// 电表 L3 实时功率
		/// </summary>
		public decimal? PmeterL3 { get; set; }

		/// <summary>
		/// 电表的实时功率
		/// </summary>
		public decimal? PmeterDc { get; set; }

		/// <summary>
		/// 电池实时功率
		/// </summary>
		public decimal? Pbat { get; set; }

		/// <summary>
		/// 逆变器视在功率 
		/// </summary>
		public decimal? Pva { get; set; }

		/// <summary>
		/// 入网电表无功功率 
		/// </summary>
		public decimal? VarAC { get; set; }

		/// <summary>
		/// 并网逆变器测电表无功功率 
		/// </summary>
		public decimal? VarDC { get; set; }
	}

	public sealed class PowerReportDataDto
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
