using System;

namespace AlphaEssWeb.Api_V2.Model.Dtos
{
	public sealed class VtColDataDto : IDto<Guid>
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
		/// 
		/// </summary>
		public decimal? Ppv2 { get; set; }

		/// <summary>
		/// PV 电压 1 
		/// </summary>
		public decimal? Upv1 { get; set; }

		/// <summary>
		/// PV 电压 2
		/// </summary>
		public decimal? Upv2 { get; set; }

		/// <summary>
		/// L1 市电电压 
		/// </summary>
		public decimal? Ua { get; set; }

		/// <summary>
		/// L2 市电电压 
		/// </summary>
		public decimal? Ub { get; set; }

		/// <summary>
		/// L3 市电电压 
		/// </summary>
		public decimal? Uc { get; set; }

		/// <summary>
		/// 市电频率
		/// </summary>
		public decimal? Fac { get; set; }

		/// <summary>
		/// 母线电压
		/// </summary>
		public decimal? Ubus { get; set; }

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
		/// 逆变器温度
		/// </summary>
		public decimal? Tinv { get; set; }

		/// <summary>
		/// EMS 控制功率指令
		/// </summary>
		public decimal? PacL1 { get; set; }

		/// <summary>
		/// EMS 控制功率指令
		/// </summary>
		public decimal? PacL2 { get; set; }

		/// <summary>
		/// EMS 控制功率指令
		/// </summary>
		public decimal? PacL3 { get; set; }

		public int? InvworkMode { get; set; }
		
		public decimal? EpvTotal { get; set; }
		
		public decimal? Einput { get; set; }
	
		public decimal? Eoutput { get; set; }
	
		public decimal? Echarge { get; set; }
	
		public decimal? PmeterL1 { get; set; }
	
		public decimal? PmeterL2 { get; set; }
		
		public decimal? PmeterL3 { get; set; }
	
		public decimal? PmeterDc { get; set; }
		
		public decimal? Pbat { get; set; }
		
		public decimal? Soc { get; set; }
	
		public decimal? Batv { get; set; }

		public decimal? Batc { get; set; }

		public string FlagBms { get; set; }
		
		public int? BmsWork { get; set; }

		public int? Pcharge { get; set; }

		public int? Pdischarge { get; set; }

		public string BmsRelay { get; set; }

		public string BmsNum { get; set; }

		public string VcellLow { get; set; }

		public string VcellHigh { get; set; }

		public string TcellLow { get; set; }

		public string TcellHigh { get; set; }

		public string IdTempLover { get; set; }

		public string IdTempEover { get; set; }

		public string IdTempediffe { get; set; }

		public string IdChargcurre { get; set; }

		public string IdDischcurre { get; set; }

		public string IdCellvolover { get; set; }

		public string IdCellvollower { get; set; }

		public string IdSoclower { get; set; }

		public string IdCellvoldiffe { get; set; }

		public decimal? BatC1 { get; set; }

		public decimal? BatC2 { get; set; }

		public decimal? BatC3 { get; set; }

		public decimal? BatC4 { get; set; }

		public decimal? BatC5 { get; set; }

		public decimal? BatC6 { get; set; }

		public string ErrInv { get; set; }

		public string WarInv { get; set; }

		public string ErrEms { get; set; }

		public string ErrBms { get; set; }

		public string ErrMeter { get; set; }

		public string ErrBackupBox { get; set; }

		public decimal? EGridCharge { get; set; }

		public string EmsStatus { get; set; }

		public decimal? EDischarge { get; set; }

		public decimal? Soc1 { get; set; }

		public decimal? Soc2 { get; set; }

		public decimal? Soc3 { get; set; }

		public decimal? Soc4 { get; set; }

		public decimal? Soc5 { get; set; }

		public decimal? Soc6 { get; set; }
		public decimal? VcellLowValue { get; set; }
		public decimal? VcellHighValue { get; set; }
		public decimal? TcellLowValue { get; set; }
		public decimal? TcellHighValue { get; set; }
		public decimal? InvBatV { get; set; }
		public int? BmsShutdown { get; set; }
		public int? BmuRelay { get; set; }
		public int? BmsHardVer1 { get; set; }
		public int? BmsHardVer2 { get; set; }
		public int? BmsHardVer3 { get; set; }
		public decimal? DispatchSwitch { get; set; }
		public decimal? Pdispatch { get; set; }
		public decimal? DispatchSoc { get; set; }
		public decimal? DispatchMode { get; set; }
	}
}
