namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class VT_SYSTEM : IEntity<Guid>
	{
		[Key]
		//public Guid SYSTEM_ID { get; set; }
		[Column("SYSTEM_ID")]
		public Guid Key { get; set; }

		[StringLength(50)]
		[Column("SYS_SN")]
		public string SysSn { get; set; }
		[StringLength(50)]
		[Column("LIC_NO")]
		public string LicNo { get; set; }
		[StringLength(50)]
		[Column("SYS_NAME")]
		public string SysName { get; set; }
		[StringLength(200)]
		[Column("RemarkSM")]
		public string RemarkSM { get; set; }
		[StringLength(50)]
		[Column("COUNTRY_CODE")]
		public string CountryCode { get; set; }
		[StringLength(50)]
		[Column("STATE_CODE")]
		public string StateCode { get; set; }
		[StringLength(50)]
		[Column("CITY_CODE")]
		public string CityCode { get; set; }
		[StringLength(200)]
		[Column("ADDRESS")]
		public string Address { get; set; }
		[StringLength(50)]
		[Column("POST_CODE")]
		public string PostCode { get; set; }
		[StringLength(50)]
		[Column("LINKMAN")]
		public string Linkman { get; set; }
		[StringLength(50)]
		[Column("CELL_PHONE")]
		public string CellPhone { get; set; }
		[StringLength(50)]
		[Column("FAX")]
		public string Fax { get; set; }
		[StringLength(50)]
		[Column("MONEY_TYPE")]
		public string MoneyType { get; set; }
		[Column("CHECK_TIME")]
		public DateTime? CheckTime { get; set; }
		[Column("POPV")]
		public decimal? Popv { get; set; }
		[StringLength(50)]
		[Column("INVENTER_SN")]
		public string InventerSn { get; set; }
		[StringLength(50)]
		[Column("MINV")]
		public string Minv { get; set; }
		[Column("POINV")]
		public decimal? Poinv { get; set; }
		[Column("COBAT")]
		public decimal? Cobat { get; set; }
		[StringLength(50)]
		[Column("MMETER")]
		public string Mmeter { get; set; }
		[StringLength(50)]
		[Column("MBAT")]
		public string Mbat { get; set; }
		[Column("USCAPACITY")]
		public decimal? Uscapacity { get; set; }
		[StringLength(50)]
		[Column("BatterySN1")]
		public string Batterysn1 { get; set; }
		[StringLength(50)]
		[Column("BatterySN2")]
		public string Batterysn2 { get; set; }
		[StringLength(50)]
		[Column("BatterySN3")]
		public string Batterysn3 { get; set; }
		[StringLength(50)]
		[Column("BatterySN4")]
		public string Batterysn4 { get; set; }
		[StringLength(50)]
		[Column("BatterySN5")]
		public string Batterysn5 { get; set; }
		[StringLength(50)]
		[Column("BatterySN6")]
		public string Batterysn6 { get; set; }
		[StringLength(50)]
		[Column("BatterySN7")]
		public string Batterysn7 { get; set; }
		[StringLength(50)]
		[Column("BatterySN8")]
		public string Batterysn8 { get; set; }
		[StringLength(50)]
		[Column("BatterySN9")]
		public string Batterysn9 { get; set; }
		[StringLength(50)]
		[Column("BatterySN10")]
		public string Batterysn10 { get; set; }
		[StringLength(50)]
		[Column("BatterySN11")]
		public string Batterysn11 { get; set; }
		[StringLength(50)]
		[Column("BatterySN12")]
		public string Batterysn12 { get; set; }
		[StringLength(50)]
		[Column("BatterySN13")]
		public string Batterysn13 { get; set; }
		[StringLength(50)]
		[Column("BatterySN14")]
		public string Batterysn14 { get; set; }
		[StringLength(50)]
		[Column("BatterySN15")]
		public string Batterysn15 { get; set; }
		[StringLength(50)]
		[Column("BatterySN16")]
		public string Batterysn16 { get; set; }
		[StringLength(50)]
		[Column("BatterySN17")]
		public string Batterysn17 { get; set; }
		[StringLength(50)]
		[Column("BatterySN18")]
		public string Batterysn18 { get; set; }
		public int? Workmode { get; set; }
		[StringLength(50)]
		[Column("BMSVersion")]
		public string Bmsversion { get; set; }
		[StringLength(50)]
		[Column("EMSVersion")]
		public string Emsversion { get; set; }
		[StringLength(50)]
		[Column("InvVersion")]
		public string InvVersion { get; set; }
		[Column("DELETE_FLAG")]
		public int? DeleteFlag { get; set; }
		[StringLength(100)]
		[Column("CREATE_ACCOUNT")]
		public string CreateAccount { get; set; }
		[Column("CREATE_DATETIME")]
		public DateTime? CreateDatetime { get; set; }
		[StringLength(100)]
		[Column("LASTUPDATE_ACCOUNT")]
		public string LastupdateAccount { get; set; }
		[Column("LASTUPDATE_DATETIME")]
		public DateTime? LastupdateDatetime { get; set; }
		[Column("AllowAutoUpdate")]
		public int? AllowAutoUpdate { get; set; }
		[StringLength(64)]
		[Column("EMSStatus")]
		public string EmsStatus { get; set; }
		[StringLength(64)]
		[Column("Latitude")]
		public string Latitude { get; set; }
		[StringLength(64)]
		[Column("Longitude")]
		public string Longitude { get; set; }
		[StringLength(64)]
		public string RemarkI { get; set; }
		[Column("USER_ID")]
		public Guid? UserId { get; set; }

		[StringLength(50)]
		[Column("ACDC")]
		public string Acdc { get; set; }
		[Column("InputCost")]
		public decimal? Inputcost { get; set; }
		[Column("OutputCost")]
		public decimal? Outputcost { get; set; }
		[Column("GridCharge")]
		public short? Gridcharge { get; set; }
		[Column("TimeChaF1")]
		public int Timechaf1 { get; set; }
		[Column("TimeChaE1")]
		public int Timechae1 { get; set; }
		[Column("TimeChaF2")]
		public int Timechaf2 { get; set; }
		[Column("TimeChaE2")]
		public int Timechae2 { get; set; }
		[Column("TimeDisF1")]
		public int Timedisf1 { get; set; }
		[Column("TimeDisE1")]
		public int Timedise1 { get; set; }
		[Column("TimeDisF2")]
		public int Timedisf2 { get; set; }
		[Column("TimeDisE2")]
		public int Timedise2 { get; set; }
		[Column("BatHighCap")]
		public decimal? Bathighcap { get; set; }
		[Column("BatUseCap")]
		public decimal? Batusecap { get; set; }
		[Column("SellPrice")]
		public decimal? Sellprice { get; set; }
		[Column("SalePrice0")]
		public decimal? Saleprice0 { get; set; }
		[Column("SalePrice1")]
		public decimal? Saleprice1 { get; set; }
		[Column("SalePrice2")]
		public decimal? Saleprice2 { get; set; }
		[Column("SalePrice3")]
		public decimal? Saleprice3 { get; set; }
		[Column("SalePrice4")]
		public decimal? Saleprice4 { get; set; }
		[Column("SalePrice5")]
		public decimal? Saleprice5 { get; set; }
		[Column("SalePrice6")]
		public decimal? Saleprice6 { get; set; }
		[Column("SalePrice7")]
		public decimal? Saleprice7 { get; set; }
		[Column("SaleTimeS0")]
		public int SaletimeS0 { get; set; }
		[Column("SaleTimeS1")]
		public int SaletimeS1 { get; set; }
		[Column("SaleTimeS2")]
		public int SaletimeS2 { get; set; }
		[Column("SaleTimeS3")]
		public int SaletimeS3 { get; set; }
		[Column("SaleTimeS4")]
		public int SaletimeS4 { get; set; }
		[Column("SaleTimeS5")]
		public int SaletimeS5 { get; set; }
		[Column("SaleTimeS6")]
		public int SaletimeS6 { get; set; }
		[Column("SaleTimeS7")]
		public int SaletimeS7 { get; set; }
		[Column("SaleTimeE0")]
		public int SaletimeE0 { get; set; }
		[Column("SaleTimeE1")]
		public int SaletimeE1 { get; set; }
		[Column("SaleTimeE2")]
		public int SaletimeE2 { get; set; }
		[Column("SaleTimeE3")]
		public int SaletimeE3 { get; set; }
		[Column("SaleTimeE4")]
		public int SaletimeE4 { get; set; }
		[Column("SaleTimeE5")]
		public int SaletimeE5 { get; set; }
		[Column("SaleTimeE6")]
		public int SaletimeE6 { get; set; }
		[Column("SaleTimeE7")]
		public int SaletimeE7 { get; set; }
		[Column("SetMode")]
		public int Setmode { get; set; }
		[Column("SetEmail")]
		public string Setemail { get; set; }
		[Column("SetTime")]
		public string Settime { get; set; }
		[Column("SetPhase")]
		public int SetPhase { get; set; }
		[Column("SetFeed")]
		public int SetFeed { get; set; }
		[NotMapped]
		public string StateStr { get; set; }
		[Column("CtrDis")]
		public short? CtrDis { get; set; }
		[Column("Generator")]
		public short? Generator { get; set; }
		[Column("BackUpBox")]
		public int? BackUpBox { get; set; }
		[Column("Fan")]
		public int? Fan { get; set; }
		[Column("CTRate")]
		public int? CTRate { get; set; }
		public Guid? CompanyId { get; set; }
		[StringLength(64)]
		[Column("BakBoxSN")]
		public string BakBoxSN { get; set; }
		[StringLength(64)]
		public string SCBSN { get; set; }
		[StringLength(64)]
		public string BakBoxVer { get; set; }
		[StringLength(64)]
		public string SCBVer { get; set; }
		[StringLength(128)]
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
		[StringLength(64)]
		public string PowerSource { get; set; }
		[StringLength(16)]
		public string EmsLanguage { get; set; }
		[StringLength(128)]
		public string SysTimezone { get; set; }
		public short? L1Priority { get; set; }
		public short? L2Priority { get; set; }
		public short? L3Priority { get; set; }
		public decimal? L1SocLimit { get; set; }
		public decimal? L2SocLimit { get; set; }
		public decimal? L3SocLimit { get; set; }
		[Column("OFS_EpvTotal")]
		public decimal? OFSEpvTotal { get; set; }
		[Column("OFS_Einput")]
		public decimal? OFSEinput { get; set; }
		[Column("OFS_Eoutput")]
		public decimal? OFSEoutput { get; set; }
		[Column("OFS_Echarge")]
		public decimal? OFSEcharge { get; set; }
		[Column("OFS_EGridCharge")]
		public decimal? OFSEGridCharge { get; set; }
		[Column("OFS_Edischarge")]
		public decimal? OFSEdischarge { get; set; }

		public decimal? OnGridCap { get; set; }
		public decimal? StorageCap { get; set; }
		[StringLength(1)]
		public string BatReady { get; set; }
		[StringLength(1)]
		public string MeterDCNegate { get; set; }
		[StringLength(1)]
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
		[StringLength(1)]
		public string Channel1 { get; set; }
		[StringLength(4)]
		public string ControlMode1 { get; set; }
		[StringLength(5)]
		public string StartTime1A { get; set; }
		[StringLength(5)]
		public string EndTime1A { get; set; }
		[StringLength(5)]
		public string StartTime1B { get; set; }
		[StringLength(5)]
		public string EndTime1B { get; set; }
		[StringLength(7)]
		public string Date1 { get; set; }
		[StringLength(4)]
		public string ChargeSOC1 { get; set; }
		[Column("ChargeMode1")]
		public int? ChargeModel1 { get; set; }
		[StringLength(1)]
		public string UPS1 { get; set; }
		public int? SwitchOn1 { get; set; }
		public int? SwitchOff1 { get; set; }
		[StringLength(16)]
		public string Delay1 { get; set; }
		[StringLength(16)]
		public string Duration1 { get; set; }
		[StringLength(16)]
		public string Pause1 { get; set; }
		[StringLength(1)]
		public string Channel2 { get; set; }
		[StringLength(4)]
		public string ControlMode2 { get; set; }
		[StringLength(5)]
		public string StartTime2A { get; set; }
		[StringLength(5)]
		public string EndTime2A { get; set; }
		[StringLength(5)]
		public string StartTime2B { get; set; }
		[StringLength(5)]
		public string EndTime2B { get; set; }
		[StringLength(7)]
		public string Date2 { get; set; }
		[StringLength(4)]
		public string ChargeSOC2 { get; set; }
		[Column("ChargeMode2")]
		public int? ChargeModel2 { get; set; }
		[StringLength(1)]
		public string UPS2 { get; set; }
		public int? SwitchOn2 { get; set; }
		public int? SwitchOff2 { get; set; }
		[StringLength(16)]
		public string Delay2 { get; set; }
		[StringLength(16)]
		public string Duration2 { get; set; }
		[StringLength(16)]
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
		[StringLength(7)]
		public string ChargeWorkDays { get; set; }
		[StringLength(7)]
		public string ChargeWeekend { get; set; }
		public decimal? MaxGridCharge { get; set; }
		public string TOPBMUVer { get; set; }
		public string ISOVer { get; set; }

		public virtual SYS_USER SYS_USER { get; set; }

		[NotMapped]
		public List<SysWeatherForecast> ListWeatherForecast { get; set; }
	}
}
