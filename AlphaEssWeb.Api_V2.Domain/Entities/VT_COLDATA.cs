namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class VT_COLDATA : IEntity<Guid>
	{
		[Key]
		[Column("UPLOAD_ID")]
		public Guid Key { get; set; }

		[Column("CREATE_DATETIME")]
		public DateTime? CreateDatetime { get; set; }
		[Column("UPLOAD_DATETIME")]
		public DateTime? UploadDatetime { get; set; }
		[StringLength(50)]
		[Column("SYS_SN")]
		public string SysSn { get; set; }
		[Column("PPV1")]
		public decimal? Ppv1 { get; set; }
		[Column("PPV2")]
		public decimal? Ppv2 { get; set; }
		[Column("UPV1")]
		public decimal? Upv1 { get; set; }
		[Column("UPV2")]
		public decimal? Upv2 { get; set; }
		[Column("UA")]
		public decimal? Ua { get; set; }
		[Column("UB")]
		public decimal? Ub { get; set; }
		[Column("UC")]
		public decimal? Uc { get; set; }
		[Column("FAC")]
		public decimal? Fac { get; set; }
		[Column("UBUS")]
		public decimal? Ubus { get; set; }
		[Column("PREAL_L1")]
		public decimal? PrealL1 { get; set; }
		[Column("PREAL_L2")]
		public decimal? PrealL2 { get; set; }
		[Column("PREAL_L3")]
		public decimal? PrealL3 { get; set; }
		[Column("TINV")]
		public decimal? Tinv { get; set; }
		[Column("PAC_L1")]
		public decimal? PacL1 { get; set; }
		[Column("PAC_L2")]
		public decimal? PacL2 { get; set; }
		[Column("PAC_L3")]
		public decimal? PacL3 { get; set; }
		[Column("INVWORK_MODE")]
		public int? InvworkMode { get; set; }
		[Column("EPV_TOTAL")]
		public decimal? EpvTotal { get; set; }
		[Column("EINPUT")]
		public decimal? Einput { get; set; }
		[Column("EOUTPUT")]
		public decimal? Eoutput { get; set; }
		[Column("ECHARGE")]
		public decimal? Echarge { get; set; }
		[Column("PMETER_L1")]
		public decimal? PmeterL1 { get; set; }
		[Column("PMETER_L2")]
		public decimal? PmeterL2 { get; set; }
		[Column("PMETER_L3")]
		public decimal? PmeterL3 { get; set; }
		[Column("PMETER_DC")]
		public decimal? PmeterDc { get; set; }
		[Column("PBAT")]
		public decimal? Pbat { get; set; }
		[Column("SOC")]
		public decimal? Soc { get; set; }
		[Column("BATV")]
		public decimal? Batv { get; set; }
		[Column("BATC")]
		public decimal? Batc { get; set; }
		[StringLength(10)]
		[Column("FLAG_BMS")]
		public string FlagBms { get; set; }
		[Column("BMS_WORK")]
		public int? BmsWork { get; set; }
		[Column("PCHARGE")]
		public int? Pcharge { get; set; }
		[Column("PDISCHARGE")]
		public int? Pdischarge { get; set; }
		[StringLength(50)]
		[Column("BMS_RELAY")]
		public string MsRelay { get; set; }
		[StringLength(50)]
		[Column("BMS_NUM")]
		public string BmsNum { get; set; }
		[StringLength(50)]
		[Column("VCELL_LOW")]
		public string VcellLow { get; set; }
		[StringLength(50)]
		[Column("VCELL_HIGH")]
		public string VcellHigh { get; set; }
		[StringLength(50)]
		[Column("TCELL_LOW")]
		public string TcellLow { get; set; }
		[StringLength(50)]
		[Column("TCELL_HIGH")]
		public string TcellHigh { get; set; }
		[StringLength(50)]
		[Column("ID_TEMP_LOVER")]
		public string IdTempLover { get; set; }
		[StringLength(50)]
		[Column("ID_TEMP_EOVER")]
		public string IdTempEover { get; set; }
		[StringLength(50)]
		[Column("ID_TEMPEDIFFE")]
		public string IdTempediffe { get; set; }
		[StringLength(50)]
		[Column("ID_CHARGCURRE")]
		public string IdChargcurre { get; set; }
		[StringLength(50)]
		[Column("ID_DISCHCURRE")]
		public string IdDischcurre { get; set; }
		[StringLength(50)]
		[Column("ID_CELLVOLOVER")]
		public string IdCellvolover { get; set; }
		[StringLength(50)]
		[Column("ID_CELLVOLLOWER")]
		public string IdCellvollower { get; set; }
		[StringLength(50)]
		[Column("ID_SOCLOWER")]
		public string IdSoclower { get; set; }
		[StringLength(50)]
		[Column("ID_CELLVOLDIFFE")]
		public string IdCellvoldiffe { get; set; }
		[Column("BAT_C1")]
		public decimal? BatC1 { get; set; }
		[Column("BAT_C2")]
		public decimal? BatC2 { get; set; }
		[Column("BAT_C3")]
		public decimal? BatC3 { get; set; }
		[Column("BAT_C4")]
		public decimal? BatC4 { get; set; }
		[Column("BAT_C5")]
		public decimal? BatC5 { get; set; }
		[Column("BatC6")]
		public decimal? BatC6 { get; set; }
		[StringLength(50)]
		[Column("ERR_INV")]
		public string ErrInv { get; set; }
		[StringLength(50)]
		[Column("WAR_INV")]
		public string WarInv { get; set; }
		[StringLength(50)]
		[Column("ERR_EMS")]
		public string ErrEms { get; set; }
		[StringLength(50)]
		[Column("ERR_BMS")]
		public string ErrBms { get; set; }
		[StringLength(50)]
		[Column("ERR_METER")]
		public string ErrMeter { get; set; }
		[Column("FACTORY_FLAG")]
		public int? FactoryFlag { get; set; }
		[StringLength(50)]
		[Column("ERR_BACKUP_BOX")]
		public string ErrBackupBox { get; set; }
		[Column("DELETE_FLAG")]
		public int DeleteFlag { get; set; }
		[Column("EGridCharge")]
		public decimal? Egridcharge { get; set; }
		[StringLength(50)]
		[Column("EmsStatus")]
		public string EmsStatus { get; set; }
		[Column("EDischarge")]
		public decimal? EDischarge { get; set; }
		public Guid? CompanyId { get; set; }
		[Column("SOC1")]
		public decimal? Soc1 { get; set; }
		[Column("SOC2")]
		public decimal? Soc2 { get; set; }
		[Column("SOC3")]
		public decimal? Soc3 { get; set; }
		[Column("SOC4")]
		public decimal? Soc4 { get; set; }
		[Column("SOC5")]
		public decimal? Soc5 { get; set; }
		[Column("SOC6")]
		public decimal? Soc6 { get; set; }
		[Column("VcellLowValue")]
		public decimal? VcellLowValue { get; set; }
		[Column("VcellHighValue")]
		public decimal? VcellHighValue { get; set; }
		[Column("TcellLowValue")]
		public decimal? TcellLowValue { get; set; }
		[Column("TcellHighValue")]
		public decimal? TcellHighValue { get; set; }
		[Column("InvBatV")]
		public decimal? InvBatV { get; set; }
		[Column("BmsShutdown")]
		public int? BmsShutdown { get; set; }
		[Column("BmuRelay")]
		public int? BmuRelay { get; set; }
		[Column("BmsHardVer1")]
		public int? BmsHardVer1 { get; set; }
		[Column("BmsHardVer2")]
		public int? BmsHardVer2 { get; set; }
		[Column("BmsHardVer3")]
		public int? BmsHardVer3 { get; set; }
		[Column("DispatchSwitch")]
		public decimal? DispatchSwitch { get; set; }
		[Column("Pdispatch")]
		public decimal? Pdispatch { get; set; }
		[Column("DispatchSoc")]
		public decimal? DispatchSoc { get; set; }
		[Column("DispatchMode")]
		public decimal? DispatchMode { get; set; }
	}
}
