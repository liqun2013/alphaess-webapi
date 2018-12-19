using AlphaEssWeb.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Model.Dtos;
using System;
using System.Linq;

namespace AlphaEssWeb.Api_V2.Model
{
	internal static class VT_SYSTEMExtensions
	{
		public static VtSystemDto ToVtSystemDto(this VT_SYSTEM item)
		{
			var result = new VtSystemDto
			{
				Id = item.Key,
				Sn = item.SysSn,
				SystemModel = item.SysName,
				Cobat = item.Cobat,
				UsableCapacity = item.Uscapacity,
				Mbat = item.Mbat,
				Poinv = item.Poinv,
				Popv = item.Popv,
				Remark = item.RemarkSM,
				Solution = item.Acdc,
				EmsVersion = item.Emsversion,
				BmsVersion = item.Bmsversion,
				InvVersion = item.InvVersion,
				InvModel = item.Minv,
				MeterModel = item.Mmeter,
				MeterPhase = item.SetPhase,
				SetFeed = item.SetFeed, TransFrequency = item.TransFrequency,
				EndUser = item.SYS_USER == null ? "" : item.SYS_USER.USERNAME,
				ListWeatherForecast = item.ListWeatherForecast != null && item.ListWeatherForecast.Any() ? item.ListWeatherForecast.Select(x => x.ToSysWeatherForecastDto()).ToList() : null
			};

			if (!string.IsNullOrEmpty(item.EmsStatus))
			{
				if (item.LastUploadTime >= DateTime.Now.AddMinutes(-30) && item.EmsStatus == "Normal")
				{
					result.NetWorkStatus = 1;
					result.State = "Normal";
				}
				else if (item.LastUploadTime >= DateTime.Now.AddMinutes(-30) && (item.EmsStatus == "Warning" || item.EmsStatus == "Protection"))
				{
					result.NetWorkStatus = 1;
					result.State = "Protection";
				}
				else if (item.LastUploadTime >= DateTime.Now.AddMinutes(-30) && item.EmsStatus == "Fault")
				{
					result.NetWorkStatus = 1;
					result.State = "Fault";
				}
				else
				{
					result.NetWorkStatus = 0;
					result.State = "Offline";
				}
			}
			else
			{
				result.NetWorkStatus = 0;
				result.State = "Offline";
			}

			if (item.Cobat.HasValue)
			{
				if (item.Uscapacity.HasValue && item.Uscapacity > 0)
				{
					result.UsableCapacity = item.Cobat.Value * item.Uscapacity.Value / 100.00m;
				}
				else if (!string.IsNullOrWhiteSpace(item.Mbat))
				{
					if (item.Mbat.Equals("Lead Crystal", System.StringComparison.OrdinalIgnoreCase))
					{
						result.UsableCapacity = item.Cobat.Value / 2.00m;
					}
					else if (item.Mbat.Equals("M4850-M", System.StringComparison.OrdinalIgnoreCase))
					{
						result.UsableCapacity = item.Cobat.Value * 0.8m;
					}
					else if (item.Mbat.Equals("M4860-M", System.StringComparison.OrdinalIgnoreCase))
					{
						result.UsableCapacity = item.Cobat.Value * 0.9m;
					}
				}
			}

			return result;
		}

		public static VTSYSTEMDto ToVTSYSTEMDto(this VT_SYSTEM s)
		{
			return new VTSYSTEMDto
			{
				Acdc = s.Acdc, Address = s.Address, AllowAutoUpdate = s.AllowAutoUpdate, BackUpBox = s.BackUpBox,
				BakBoxSN = s.BakBoxSN, BakBoxVer = s.BakBoxVer, BatHighCap = s.Bathighcap, BatterySN1 = s.Batterysn1,
				BatterySN10 = s.Batterysn10, BatterySN11 = s.Batterysn11, BatterySN12 = s.Batterysn12,
				BatterySN13 = s.Batterysn13, BatterySN14 = s.Batterysn14, BatterySN15 = s.Batterysn15,
				BatterySN16 = s.Batterysn16, BatterySN17 = s.Batterysn17, BatterySN18 = s.Batterysn18,
				BatterySN2 = s.Batterysn2, BatterySN3 = s.Batterysn3, BatterySN4 = s.Batterysn4, BatterySN5 = s.Batterysn5,
				BatterySN6 = s.Batterysn6, BatterySN7 = s.Batterysn7, BatterySN8 = s.Batterysn8, BatterySN9 = s.Batterysn9,
				BatUseCap = s.Batusecap, Bmsversion = s.Bmsversion, BMUModel = s.BMUModel, CellPhone = s.CellPhone,
				CheckTime = s.CheckTime, CityCode = s.CityCode, Cobat = s.Cobat, CompanyId = s.CompanyId,
				CountryCode = s.CountryCode, Create_Account = s.CreateAccount, Create_Datetime = s.CreateDatetime,
				CTRate = s.CTRate, CtrDis = s.CtrDis, Delete_Flag = s.DeleteFlag, EmsLanguage = s.EmsLanguage,
				EmsStatus = s.EmsStatus, Emsversion = s.Emsversion, Fan = s.Fan, Fax = s.Fax, GCChargePower = s.GCChargePower,
				GCOutputMode = s.GCOutputMode, GCRatedPower = s.GCRatedPower, GCSOCEnd = s.GCSOCEnd,
				GCSOCStart = s.GCSOCStart, GCTimeEnd = s.GCTimeEnd, GCTimeStart = s.GCTimeStart, Generator = s.Generator,
				GeneratorMode = s.GeneratorMode, Gridcharge = s.Gridcharge, GridType = s.GridType, Inputcost = s.Inputcost,
				InventerSn = s.InventerSn, InvVersion = s.InvVersion, L1Priority = s.L1Priority, L1SocLimit = s.L1SocLimit,
				L2Priority = s.L2Priority, L2SocLimit = s.L2SocLimit, L3Priority = s.L3Priority, L3SocLimit = s.L3SocLimit,
				Lastupdate_Account = s.LastupdateAccount, Lastupdate_Datetime = s.LastupdateDatetime,
				LastUploadTime = s.LastUploadTime, Latitude = s.Latitude, LicNo = s.LicNo, Linkman = s.Linkman,
				Longitude = s.Longitude, Mbat = s.Mbat, Minv = s.Minv, Mmeter = s.Mmeter,
				MoneyType = s.MoneyType, OFS_Echarge = s.OFSEcharge, OFS_Edischarge = s.OFSEdischarge,
				OFS_EGridCharge = s.OFSEGridCharge, OFS_Einput = s.OFSEinput, OFS_Eoutput = s.OFSEoutput,
				OFS_EpvTotal = s.OFSEpvTotal, Outputcost = s.Outputcost, Poinv = s.Poinv, Popv = s.Popv,
				PostCode = s.PostCode, PowerSource = s.PowerSource,
				RemarkI = s.RemarkI, RemarkSM = s.RemarkSM, Saleprice0 = s.Saleprice0, Saleprice1 = s.Saleprice1,
				Saleprice2 = s.Saleprice2, Saleprice3 = s.Saleprice3, Saleprice4 = s.Saleprice4, Saleprice5 = s.Saleprice5,
				Saleprice6 = s.Saleprice6, Saleprice7 = s.Saleprice7, SaletimeE0 = s.SaletimeE0, SaletimeE1 = s.SaletimeE1,
				SaletimeE2 = s.SaletimeE2, SaletimeE3 = s.SaletimeE3, SaletimeE4 = s.SaletimeE4, SaletimeE5 = s.SaletimeE5,
				SaletimeE6 = s.SaletimeE6, SaletimeE7 = s.SaletimeE7, SaletimeS0 = s.SaletimeS0, SaletimeS1 = s.SaletimeS1,
				SaletimeS2 = s.SaletimeS2, SaletimeS3 = s.SaletimeS3, SaletimeS4 = s.SaletimeS4, SaletimeS5 = s.SaletimeS5,
				SaletimeS6 = s.SaletimeS6, SaletimeS7 = s.SaletimeS7, SCBSN = s.SCBSN, SCBVer = s.SCBVer,
				Sellprice = s.Sellprice, Setemail = s.Setemail, SetFeed = s.SetFeed, Setmode = s.Setmode,
				SetPhase = s.SetPhase, Settime = s.Settime, StateCode = s.StateCode, Sys_Name = s.SysName,
				Sys_Sn = s.SysSn, SystemId = s.Key, SysTimezone = s.SysTimezone, TimeChaE1 = s.Timechae1,
				TimeChaE2 = s.Timechae2, TimeChaF1 = s.Timechaf1, TimeChaF2 = s.Timechaf2, TimeDisE1 = s.Timedise1,
				TimeDisE2 = s.Timedise2, TimeDisF1 = s.Timedisf1, TimeDisF2 = s.Timedisf2, Uscapacity = s.Uscapacity,
				User_Id = s.UserId, Workmode = s.Workmode, FirmwareVersion = s.FirmwareVersion,
				LastUploadTimeLocal = s.LastUploadTimeLocal, ActiveTime = s.ActiveTime, TransFrequency = s.TransFrequency,
				AllowSyncTime = s.AllowSyncTime, BatReady = s.BatReady, Channel1 = s.Channel1, Channel2 = s.Channel2,
				ChargeBoostCur = s.ChargeBoostCur, ControlMode1 = s.ControlMode1, ControlMode2 = s.ControlMode2,
				DCI = s.DCI, Delay1 = s.Delay1, Delay2 = s.Delay2,
				Duration1 = s.Duration1, Duration2 = s.Duration2, EndTime1A = s.EndTime1A, EndTime1B = s.EndTime1B,
				EndTime2A = s.EndTime2A, EndTime2B = s.EndTime2B, MeterDCNegate = s.MeterDCNegate,
				MeterACNegate = s.MeterACNegate, OnGridCap = s.OnGridCap, OutCurProtect = s.OutCurProtect,
				Pause1 = s.Pause1, Pause2 = s.Pause2, PowerFact = s.PowerFact, PvISO = s.PvISO, RCD = s.RCD,
				Safe = s.Safe, StartTime1A = s.StartTime1A, StartTime1B = s.StartTime1B, StartTime2A = s.StartTime2A,
				StartTime2B = s.StartTime2B, ChargeSOC1 = s.ChargeSOC1, ChargeSOC2 = s.ChargeSOC2,
				StorageCap = s.StorageCap, SwitchOff1 = s.SwitchOff1, SwitchOff2 = s.SwitchOff2,
				SwitchOn1 = s.SwitchOn1, SwitchOn2 = s.SwitchOn2, TempThreshold = s.TempThreshold, UPS1 = s.UPS1,
				UPS2 = s.UPS2, Volt10MinAvg = s.Volt10MinAvg, Volt5MinAvg = s.Volt5MinAvg, ChargeModel1 = s.ChargeModel1,
				ChargeModel2 = s.ChargeModel2, OEM_flag = s.OEM_flag, OEM_Plant_id = s.OEM_Plant_id, BatHighCapWE = s.BatHighCapWE, BatUseCapWE = s.BatUseCapWE,
				CtrDisWE = s.CtrDisWE, GridChargeWE = s.GridChargeWE, TimeChaEWE1 = s.TimeChaEWE1, TimeChaEWE2 = s.TimeChaEWE2,
				TimeChaFWE1 = s.TimeChaFWE1, TimeChaFWE2 = s.TimeChaFWE2, TimeDisEWE1 = s.TimeDisEWE1, TimeDisEWE2 = s.TimeDisEWE2, TimeDisFWE1 = s.TimeDisFWE1, TimeDisFWE2 = s.TimeDisFWE2,
				MaxGridCharge = s.MaxGridCharge, ISOVer = s.ISOVer, TOPBMUVer = s.TOPBMUVer,
				ChargeWeekend = !string.IsNullOrWhiteSpace(s.ChargeWeekend) && s.ChargeWeekend.Length == 7 ? s.ChargeWeekend : "0000000",
				ChargeWorkDays = !string.IsNullOrWhiteSpace(s.ChargeWorkDays) && s.ChargeWorkDays.Length == 7 ? s.ChargeWorkDays : "0000000",
				Date1 = !string.IsNullOrWhiteSpace(s.Date1) && s.Date1.Length == 7 ? s.Date1 : "0000000",
				Date2 = !string.IsNullOrWhiteSpace(s.Date2) && s.Date2.Length == 7 ? s.Date2 : "0000000",
			};

		}

		public static void CloneFromVTSYSTEMDto(this VT_SYSTEM dbSystem, VTSYSTEMDto s)
		{
			dbSystem.Acdc = s.Acdc;
			dbSystem.Address = s.Address;
			dbSystem.AllowAutoUpdate = s.AllowAutoUpdate;
			dbSystem.BackUpBox = s.BackUpBox;
			dbSystem.BakBoxSN = s.BakBoxSN;
			dbSystem.BakBoxVer = s.BakBoxVer;
			dbSystem.Bathighcap = s.BatHighCap;
			dbSystem.Batterysn1 = s.BatterySN1;
			dbSystem.Batterysn10 = s.BatterySN10;
			dbSystem.Batterysn11 = s.BatterySN11;
			dbSystem.Batterysn12 = s.BatterySN12;
			dbSystem.Batterysn13 = s.BatterySN13;
			dbSystem.Batterysn14 = s.BatterySN14;
			dbSystem.Batterysn15 = s.BatterySN15;
			dbSystem.Batterysn16 = s.BatterySN16;
			dbSystem.Batterysn17 = s.BatterySN17;
			dbSystem.Batterysn18 = s.BatterySN18;
			dbSystem.Batterysn2 = s.BatterySN2;
			dbSystem.Batterysn3 = s.BatterySN3;
			dbSystem.Batterysn4 = s.BatterySN4;
			dbSystem.Batterysn5 = s.BatterySN5;
			dbSystem.Batterysn6 = s.BatterySN6;
			dbSystem.Batterysn7 = s.BatterySN7;
			dbSystem.Batterysn8 = s.BatterySN8;
			dbSystem.Batterysn9 = s.BatterySN9;
			dbSystem.Batusecap = s.BatUseCap;
			dbSystem.Bmsversion = s.Bmsversion;
			dbSystem.BMUModel = s.BMUModel;
			dbSystem.CellPhone = s.CellPhone;
			dbSystem.CheckTime = s.CheckTime;
			dbSystem.CityCode = s.CityCode;
			dbSystem.Cobat = s.Cobat;
			dbSystem.CompanyId = s.CompanyId;
			dbSystem.CountryCode = s.CountryCode;
			dbSystem.CreateAccount = s.Create_Account;
			dbSystem.CreateDatetime = s.Create_Datetime;
			dbSystem.CTRate = s.CTRate;
			dbSystem.CtrDis = s.CtrDis;
			dbSystem.DeleteFlag = s.Delete_Flag;
			dbSystem.EmsLanguage = s.EmsLanguage;
			dbSystem.EmsStatus = s.EmsStatus;
			dbSystem.Emsversion = s.Emsversion;
			dbSystem.Fan = s.Fan;
			dbSystem.Fax = s.Fax;
			dbSystem.GCChargePower = s.GCChargePower;
			dbSystem.GCOutputMode = s.GCOutputMode;
			dbSystem.GCRatedPower = s.GCRatedPower;
			dbSystem.GCSOCEnd = s.GCSOCEnd;
			dbSystem.GCSOCStart = s.GCSOCStart;
			dbSystem.GCTimeEnd = s.GCTimeEnd;
			dbSystem.GCTimeStart = s.GCTimeStart;
			dbSystem.Generator = s.Generator;
			dbSystem.GeneratorMode = s.GeneratorMode;
			dbSystem.Gridcharge = s.Gridcharge;
			dbSystem.GridType = s.GridType;
			dbSystem.Inputcost = s.Inputcost;
			dbSystem.InventerSn = s.InventerSn;
			dbSystem.InvVersion = s.InvVersion;
			dbSystem.L1Priority = s.L1Priority;
			dbSystem.L1SocLimit = s.L1SocLimit;
			dbSystem.L2Priority = s.L2Priority;
			dbSystem.L2SocLimit = s.L2SocLimit;
			dbSystem.L3Priority = s.L3Priority;
			dbSystem.L3SocLimit = s.L3SocLimit;
			dbSystem.LastupdateAccount = s.Lastupdate_Account;
			dbSystem.LastupdateDatetime = s.Lastupdate_Datetime;
			dbSystem.LastUploadTime = s.LastUploadTime;
			dbSystem.Latitude = s.Latitude;
			dbSystem.LicNo = s.LicNo;
			dbSystem.Linkman = s.Linkman;
			dbSystem.Longitude = s.Longitude;
			dbSystem.Mbat = s.Mbat;
			dbSystem.Minv = s.Minv;
			dbSystem.Mmeter = s.Mmeter;
			dbSystem.MoneyType = s.MoneyType;
			dbSystem.OFSEcharge = s.OFS_Echarge;
			dbSystem.OFSEdischarge = s.OFS_Edischarge;
			dbSystem.OFSEGridCharge = s.OFS_EGridCharge;
			dbSystem.OFSEinput = s.OFS_Einput;
			dbSystem.OFSEoutput = s.OFS_Eoutput;
			dbSystem.OFSEpvTotal = s.OFS_EpvTotal;
			dbSystem.Outputcost = s.Outputcost;
			dbSystem.Poinv = s.Poinv;
			dbSystem.Popv = s.Popv;
			dbSystem.PostCode = s.PostCode;
			dbSystem.PowerSource = s.PowerSource;
			dbSystem.RemarkI = s.RemarkI;
			dbSystem.RemarkSM = s.RemarkSM;
			dbSystem.Saleprice0 = s.Saleprice0;
			dbSystem.Saleprice1 = s.Saleprice1;
			dbSystem.Saleprice2 = s.Saleprice2;
			dbSystem.Saleprice3 = s.Saleprice3;
			dbSystem.Saleprice4 = s.Saleprice4;
			dbSystem.Saleprice5 = s.Saleprice5;
			dbSystem.Saleprice6 = s.Saleprice6;
			dbSystem.Saleprice7 = s.Saleprice7;
			dbSystem.SaletimeE0 = s.SaletimeE0;
			dbSystem.SaletimeE1 = s.SaletimeE1;
			dbSystem.SaletimeE2 = s.SaletimeE2;
			dbSystem.SaletimeE3 = s.SaletimeE3;
			dbSystem.SaletimeE4 = s.SaletimeE4;
			dbSystem.SaletimeE5 = s.SaletimeE5;
			dbSystem.SaletimeE6 = s.SaletimeE6;
			dbSystem.SaletimeE7 = s.SaletimeE7;
			dbSystem.SaletimeS0 = s.SaletimeS0;
			dbSystem.SaletimeS1 = s.SaletimeS1;
			dbSystem.SaletimeS2 = s.SaletimeS2;
			dbSystem.SaletimeS3 = s.SaletimeS3;
			dbSystem.SaletimeS4 = s.SaletimeS4;
			dbSystem.SaletimeS5 = s.SaletimeS5;
			dbSystem.SaletimeS6 = s.SaletimeS6;
			dbSystem.SaletimeS7 = s.SaletimeS7;
			dbSystem.SCBSN = s.SCBSN;
			dbSystem.SCBVer = s.SCBVer;
			dbSystem.Sellprice = s.Sellprice;
			dbSystem.Setemail = s.Setemail;
			dbSystem.SetFeed = s.SetFeed;
			dbSystem.Setmode = s.Setmode;
			dbSystem.SetPhase = s.SetPhase;
			dbSystem.Settime = s.Settime;
			dbSystem.StateCode = s.StateCode;
			dbSystem.SysName = s.Sys_Name;
			dbSystem.SysSn = s.Sys_Sn;
			dbSystem.Key = s.SystemId;
			dbSystem.SysTimezone = s.SysTimezone;
			dbSystem.Timechae1 = s.TimeChaE1;
			dbSystem.Timechae2 = s.TimeChaE2;
			dbSystem.Timechaf1 = s.TimeChaF1;
			dbSystem.Timechaf2 = s.TimeChaF2;
			dbSystem.Timedise1 = s.TimeDisE1;
			dbSystem.Timedise2 = s.TimeDisE2;
			dbSystem.Timedisf1 = s.TimeDisF1;
			dbSystem.Timedisf2 = s.TimeDisF2;
			dbSystem.Uscapacity = s.Uscapacity;
			dbSystem.UserId = s.User_Id;
			dbSystem.Workmode = s.Workmode;
			dbSystem.FirmwareVersion = s.FirmwareVersion;
			dbSystem.LastUploadTimeLocal = s.LastUploadTimeLocal;
			dbSystem.ActiveTime = s.ActiveTime;
			dbSystem.TransFrequency = s.TransFrequency;
			dbSystem.AllowSyncTime = s.AllowSyncTime;
			dbSystem.BatReady = s.BatReady;
			dbSystem.Channel1 = s.Channel1;
			dbSystem.Channel2 = s.Channel2;
			dbSystem.ChargeBoostCur = s.ChargeBoostCur;
			dbSystem.ControlMode1 = s.ControlMode1;
			dbSystem.ControlMode2 = s.ControlMode2;
			dbSystem.Date1 = s.Date1;
			dbSystem.Date2 = s.Date2;
			dbSystem.DCI = s.DCI;
			dbSystem.Delay1 = s.Delay1;
			dbSystem.Delay2 = s.Delay2;
			dbSystem.Duration1 = s.Duration1;
			dbSystem.Duration2 = s.Duration2;
			dbSystem.EndTime1A = s.EndTime1A;
			dbSystem.EndTime1B = s.EndTime1B;
			dbSystem.EndTime2A = s.EndTime2A;
			dbSystem.EndTime2B = s.EndTime2B;
			dbSystem.MeterDCNegate = s.MeterDCNegate;
			dbSystem.MeterACNegate = s.MeterACNegate;
			dbSystem.OnGridCap = s.OnGridCap;
			dbSystem.OutCurProtect = s.OutCurProtect;
			dbSystem.Pause1 = s.Pause1;
			dbSystem.Pause2 = s.Pause2;
			dbSystem.PowerFact = s.PowerFact;
			dbSystem.PvISO = s.PvISO;
			dbSystem.RCD = s.RCD;
			dbSystem.Safe = s.Safe;
			dbSystem.StartTime1A = s.StartTime1A;
			dbSystem.StartTime1B = s.StartTime1B;
			dbSystem.StartTime2A = s.StartTime2A;
			dbSystem.StartTime2B = s.StartTime2B;
			dbSystem.ChargeSOC1 = s.ChargeSOC1;
			dbSystem.ChargeSOC2 = s.ChargeSOC2;
			dbSystem.StorageCap = s.StorageCap;
			dbSystem.SwitchOff1 = s.SwitchOff1;
			dbSystem.SwitchOff2 = s.SwitchOff2;
			dbSystem.SwitchOn1 = s.SwitchOn1;
			dbSystem.SwitchOn2 = s.SwitchOn2;
			dbSystem.TempThreshold = s.TempThreshold;
			dbSystem.UPS1 = s.UPS1;
			dbSystem.UPS2 = s.UPS2;
			dbSystem.Volt10MinAvg = s.Volt10MinAvg;
			dbSystem.Volt5MinAvg = s.Volt5MinAvg;
			dbSystem.ChargeModel1 = s.ChargeModel1;
			dbSystem.ChargeModel2 = s.ChargeModel2;
			dbSystem.OEM_flag = s.OEM_flag;
			dbSystem.OEM_Plant_id = s.OEM_Plant_id;
			dbSystem.BatHighCapWE = s.BatHighCapWE;
			dbSystem.BatUseCapWE = s.BatUseCapWE;
			dbSystem.ChargeWeekend = s.ChargeWeekend;
			dbSystem.ChargeWorkDays = s.ChargeWorkDays;
			dbSystem.CtrDisWE = s.CtrDisWE;
			dbSystem.GridChargeWE = s.GridChargeWE;
			dbSystem.TimeChaEWE1 = s.TimeChaEWE1;
			dbSystem.TimeChaEWE2 = s.TimeChaEWE2;
			dbSystem.TimeChaFWE1 = s.TimeChaFWE1;
			dbSystem.TimeChaFWE2 = s.TimeChaFWE2;
			dbSystem.TimeDisEWE1 = s.TimeDisEWE1;
			dbSystem.TimeDisEWE2 = s.TimeDisEWE2;
			dbSystem.TimeDisFWE1 = s.TimeDisFWE1;
			dbSystem.TimeDisFWE2 = s.TimeDisFWE2;
			dbSystem.MaxGridCharge = s.MaxGridCharge;
			dbSystem.TOPBMUVer = s.TOPBMUVer;
			dbSystem.ISOVer = s.ISOVer;
		}
	}
}
