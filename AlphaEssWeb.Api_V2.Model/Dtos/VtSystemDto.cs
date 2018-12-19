using AlphaEssWeb.Api_V2.Model.ExternalRequestModels;
using System;
using System.Collections.Generic;

namespace AlphaEssWeb.Api_V2.Model.Dtos
{
	/// <summary>
	/// 轻量
	/// </summary>
	public sealed class VtSystemDto : IDto<Guid>
	{
		public Guid Id { get; set; }
		public string Sn { get; set; }
		/// <summary>
		/// 系统型号 
		/// </summary>
		public string SystemModel { get; set; }
		/// <summary>
		/// 电池容量 
		/// </summary>
		public decimal? Cobat { get; set; }
		/// <summary>
		/// 电池可用容量
		/// </summary>
		public decimal? UsableCapacity { get; set; }
		/// <summary>
		/// 电池型号 
		/// </summary>
		public string Mbat { get; set; }
		/// <summary>
		/// 逆变器额定输出功率 
		/// </summary>
		public decimal? Poinv { get; set; }
		/// <summary>
		/// 额定 PV 装机容量 
		/// </summary>
		public decimal? Popv { get; set; }
		/// <summary>
		/// 系统备注名称 
		/// </summary>
		public string Remark { get; set; }
		/// <summary>
		/// 系统模式(1:AC,2:DC,3: hybrid) 
		/// </summary>
		public string Solution { get; set; }
		/// <summary>
		/// EMS 版本
		/// </summary>
		public string EmsVersion { get; set; }
		/// <summary>
		/// BMS 版本
		/// </summary>
		public string BmsVersion { get; set; }
		/// <summary>
		/// 逆变器版本
		/// </summary>
		public string InvVersion { get; set; }
		/// <summary>
		/// 逆变器型号 
		/// </summary>
		public string InvModel { get; set; }
		/// <summary>
		/// 电表型号
		/// </summary>
		public string MeterModel { get; set; }
		/// <summary>
		/// 电表相位
		/// </summary>
		public int? MeterPhase { get; set; }
		/// <summary>
		/// 并网系数
		/// </summary>
		public int? SetFeed { get; set; }
		/// <summary>
		/// 设备是否在线（1：online，0： offline） 
		/// </summary>
		public int? NetWorkStatus { get; set; }
		/// <summary>
		/// 系统状态
		/// </summary>
		public string State { get; set; }
		/// <summary>
		/// 终端用户名称
		/// </summary>
		public string EndUser { get; set; }
		public int? TransFrequency { get; set; }
		public List<SysWeatherForecastDto> ListWeatherForecast {get;set;}
	}

	/// <summary>
	/// 包括全部字段
	/// </summary>
	public class VTSYSTEMDto : ExternalBaseRequestModel
	{
		public Guid SystemId { get; set; }
		public string Sys_Sn { get; set; }
		public string LicNo { get; set; }
		public string Sys_Name { get; set; }
		public string RemarkSM { get; set; }
		public string CountryCode { get; set; }
		public string StateCode { get; set; }
		public string CityCode { get; set; }
		public string Address { get; set; }
		public string PostCode { get; set; }
		public string Linkman { get; set; }
		public string CellPhone { get; set; }
		public string Fax { get; set; }
		public string MoneyType { get; set; }
		public DateTime? CheckTime { get; set; }
		public decimal? Popv { get; set; }
		public string InventerSn { get; set; }
		public string Minv { get; set; }
		public decimal? Poinv { get; set; }
		public decimal? Cobat { get; set; }
		public string Mmeter { get; set; }
		public string Mbat { get; set; }
		public decimal? Uscapacity { get; set; }
		public string BatterySN1 { get; set; }
		public string BatterySN2 { get; set; }
		public string BatterySN3 { get; set; }
		public string BatterySN4 { get; set; }
		public string BatterySN5 { get; set; }
		public string BatterySN6 { get; set; }
		public string BatterySN7 { get; set; }
		public string BatterySN8 { get; set; }
		public string BatterySN9 { get; set; }
		public string BatterySN10 { get; set; }
		public string BatterySN11 { get; set; }
		public string BatterySN12 { get; set; }
		public string BatterySN13 { get; set; }
		public string BatterySN14 { get; set; }
		public string BatterySN15 { get; set; }
		public string BatterySN16 { get; set; }
		public string BatterySN17 { get; set; }
		public string BatterySN18 { get; set; }
		public int? Workmode { get; set; }
		public string Bmsversion { get; set; }
		public string Emsversion { get; set; }
		public string InvVersion { get; set; }
		public int? Delete_Flag { get; set; }
		public string Create_Account { get; set; }
		public DateTime? Create_Datetime { get; set; }
		public string Lastupdate_Account { get; set; }
		public DateTime? Lastupdate_Datetime { get; set; }
		public int? AllowAutoUpdate { get; set; }
		public string EmsStatus { get; set; }
		public string Latitude { get; set; }
		public string Longitude { get; set; }
		public string RemarkI { get; set; }
		public Guid? User_Id { get; set; }
		public string Acdc { get; set; }
		public decimal? Inputcost { get; set; }
		public decimal? Outputcost { get; set; }
		public short? Gridcharge { get; set; }
		public int TimeChaF1 { get; set; }
		public int TimeChaE1 { get; set; }
		public int TimeChaF2 { get; set; }
		public int TimeChaE2 { get; set; }
		public int TimeDisF1 { get; set; }
		public int TimeDisE1 { get; set; }
		public int TimeDisF2 { get; set; }
		public int TimeDisE2 { get; set; }
		public decimal? BatHighCap { get; set; }
		public decimal? BatUseCap { get; set; }
		public decimal? Sellprice { get; set; }
		public decimal? Saleprice0 { get; set; }
		public decimal? Saleprice1 { get; set; }
		public decimal? Saleprice2 { get; set; }
		public decimal? Saleprice3 { get; set; }
		public decimal? Saleprice4 { get; set; }
		public decimal? Saleprice5 { get; set; }
		public decimal? Saleprice6 { get; set; }
		public decimal? Saleprice7 { get; set; }
		public int SaletimeS0 { get; set; }
		public int SaletimeS1 { get; set; }
		public int SaletimeS2 { get; set; }
		public int SaletimeS3 { get; set; }
		public int SaletimeS4 { get; set; }
		public int SaletimeS5 { get; set; }
		public int SaletimeS6 { get; set; }
		public int SaletimeS7 { get; set; }
		public int SaletimeE0 { get; set; }
		public int SaletimeE1 { get; set; }
		public int SaletimeE2 { get; set; }
		public int SaletimeE3 { get; set; }
		public int SaletimeE4 { get; set; }
		public int SaletimeE5 { get; set; }
		public int SaletimeE6 { get; set; }
		public int SaletimeE7 { get; set; }
		public int Setmode { get; set; }
		public string Setemail { get; set; }
		public string Settime { get; set; }
		public int SetPhase { get; set; }
		public int SetFeed { get; set; }
		public short? CtrDis { get; set; }
		public short? Generator { get; set; }
		public int? BackUpBox { get; set; }
		public int? Fan { get; set; }
		public int? CTRate { get; set; }
		public Guid? CompanyId { get; set; }
		public string BakBoxSN { get; set; }
		public string SCBSN { get; set; }
		public string BakBoxVer { get; set; }
		public string SCBVer { get; set; }
		public string BMUModel { get; set; }
		public DateTime? LastUploadTime { get; set; }
		public int? GeneratorMode { get; set; }
		public int? GCSOCStart { get; set; }
		public int? GCSOCEnd { get; set; }
		public int? GCTimeStart { get; set; }
		public int? GCTimeEnd { get; set; }
		public int? GCOutputMode { get; set; }
		public int? GCChargePower { get; set; }
		public int? GCRatedPower { get; set; }
		public decimal? GridType { get; set; }
		public string PowerSource { get; set; }
		public string EmsLanguage { get; set; }
		public string SysTimezone { get; set; }
		public short? L1Priority { get; set; }
		public short? L2Priority { get; set; }
		public short? L3Priority { get; set; }
		public decimal? L1SocLimit { get; set; }
		public decimal? L2SocLimit { get; set; }
		public decimal? L3SocLimit { get; set; }
		public decimal? OFS_EpvTotal { get; set; }
		public decimal? OFS_Einput { get; set; }
		public decimal? OFS_Eoutput { get; set; }
		public decimal? OFS_Echarge { get; set; }
		public decimal? OFS_EGridCharge { get; set; }
		public decimal? OFS_Edischarge { get; set; }
		public decimal? OnGridCap { get; set; }
		public decimal? StorageCap { get; set; }
		public string BatReady { get; set; }
		public string MeterDCNegate { get; set; }
		public string MeterACNegate { get; set; }
		public int? Safe { get; set; }
		public int? PowerFact { get; set; }
		public int? Volt5MinAvg { get; set; }
		public int? Volt10MinAvg { get; set; }
		public int? TempThreshold { get; set; }
		public int? OutCurProtect { get; set; }
		public int? DCI { get; set; }
		public int? RCD { get; set; }
		public int? PvISO { get; set; }
		public int? ChargeBoostCur { get; set; }
		public string Channel1 { get; set; }
		public string ControlMode1 { get; set; }
		public string StartTime1A { get; set; }
		public string EndTime1A { get; set; }
		public string StartTime1B { get; set; }
		public string EndTime1B { get; set; }
		public string Date1 { get; set; }
		public string ChargeSOC1 { get; set; }
		public int? ChargeModel1 { get; set; }
		public string UPS1 { get; set; }
		public int? SwitchOn1 { get; set; }
		public int? SwitchOff1 { get; set; }

		public string Delay1 { get; set; }
		public string Duration1 { get; set; }
		public string Pause1 { get; set; }
		public string Channel2 { get; set; }
		public string ControlMode2 { get; set; }
		public string StartTime2A { get; set; }
		public string EndTime2A { get; set; }
		public string StartTime2B { get; set; }
		public string EndTime2B { get; set; }
		public string Date2 { get; set; }
		public string ChargeSOC2 { get; set; }
		public int? ChargeModel2 { get; set; }
		public string UPS2 { get; set; }
		public int? SwitchOn2 { get; set; }
		public int? SwitchOff2 { get; set; }
		public string Delay2 { get; set; }
		public string Duration2 { get; set; }
		public string Pause2 { get; set; }
		public short? AllowSyncTime { get; set; }
		public string FirmwareVersion { get; set; }
		public DateTime? LastUploadTimeLocal { get; set; }
		public DateTime? ActiveTime { get; set; }
		public int? OEM_flag { get; set; }
		public string OEM_Plant_id { get; set; }
		public int? TransFrequency { get; set; }
		public short? GridChargeWE { get; set; }
		public int? TimeChaFWE1 { get; set; }
		public int? TimeChaEWE1 { get; set; }
		public int? TimeChaFWE2 { get; set; }
		public int? TimeChaEWE2 { get; set; }
		public int? TimeDisFWE1 { get; set; }
		public int? TimeDisEWE1 { get; set; }
		public int? TimeDisFWE2 { get; set; }
		public int? TimeDisEWE2 { get; set; }
		public decimal? BatHighCapWE { get; set; }
		public decimal? BatUseCapWE { get; set; }
		public short? CtrDisWE { get; set; }
		public string ChargeWorkDays { get; set; }
		public string ChargeWeekend { get; set; }
		public decimal? MaxGridCharge { get; set; }
		public string TOPBMUVer { get; set; }
		public string ISOVer { get; set; }

	}
}
