using AlphaEss.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Model.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlphaEssWeb.Api_V2.Model
{
	public static class PaginatedRunningDataExtensions
	{
		internal static List<VtColDataDto> ToPaginatedRunningDataDto(this PaginatedList<VT_COLDATA> pls)
		{
			List<VtColDataDto> result = new List<VtColDataDto>();
			foreach (var item in pls)
			{
				VtColDataDto dto = new VtColDataDto();
				dto.Id = item.Key;
				dto.Sn = item.SysSn;
				dto.UploadTime = item.UploadDatetime;
				dto.Ppv1 = item.Ppv1;
				dto.Ppv2 = item.Ppv2;
				dto.Upv1 = item.Upv1;
				dto.Upv1 = item.Upv2;
				dto.Ua = item.Ua;
				dto.Ub = item.Ub;
				dto.Uc = item.Uc;
				dto.Fac = item.Fac;
				dto.Ubus = item.Ubus;
				dto.PrealL1 = item.PrealL1;
				dto.PrealL2 = item.PrealL2;
				dto.PrealL3 = item.PrealL3;
				dto.Tinv = item.Tinv;
				dto.PacL1 = item.PacL1;
				dto.PacL2 = item.PacL2;
				dto.PacL3 = item.PacL3;
				dto.InvworkMode = item.InvworkMode;
				dto.EpvTotal = item.EpvTotal;
				dto.Einput = item.Einput;
				dto.Eoutput = item.Eoutput;
				dto.Echarge = item.Echarge;
				dto.PmeterL1 = item.PmeterL1;
				dto.PmeterL2 = item.PmeterL2;
				dto.PmeterL3 = item.PmeterL3;
				dto.PmeterDc = item.PmeterDc;
				dto.Pbat = item.Pbat;
				dto.Soc = item.Soc;
				dto.Batv = item.Batv;
				dto.Batc = item.Batc;
				dto.FlagBms = item.FlagBms;
				dto.BmsWork = item.BmsWork;
				dto.Pcharge = item.Pcharge;
				dto.Pdischarge = item.Pdischarge;
				dto.BmsRelay = item.MsRelay;
				dto.BmsNum = item.BmsNum;
				dto.VcellLow = item.VcellLow;
				dto.VcellHigh = item.VcellHigh;
				dto.TcellLow = item.TcellLow;
				dto.TcellHigh = item.TcellHigh;
				dto.IdTempLover = item.IdTempLover;
				dto.IdTempEover = item.IdTempEover;
				dto.IdTempediffe = item.IdTempediffe;
				dto.IdChargcurre = item.IdChargcurre;
				dto.IdDischcurre = item.IdDischcurre;
				dto.IdCellvolover = item.IdCellvolover;
				dto.IdCellvollower = item.IdCellvollower;
				dto.IdSoclower = item.IdSoclower;
				dto.IdCellvoldiffe = item.IdCellvoldiffe;
				dto.BatC1 = item.BatC1;
				dto.BatC2 = item.BatC2;
				dto.BatC3 = item.BatC3;
				dto.BatC4 = item.BatC4;
				dto.BatC5 = item.BatC5;
				dto.BatC6 = item.BatC6;
				dto.ErrInv = item.ErrInv;
				dto.WarInv = item.WarInv;
				dto.ErrEms = item.ErrEms;
				dto.ErrBms = item.ErrBms;
				dto.ErrMeter = item.ErrMeter;
				dto.ErrBackupBox = item.ErrBackupBox;
				dto.EGridCharge = item.Egridcharge;
				dto.EmsStatus = item.EmsStatus;
				dto.EDischarge = item.EDischarge;
				dto.Soc1 = item.Soc1;
				dto.Soc2 = item.Soc2;
				dto.Soc3 = item.Soc3;
				dto.Soc4 = item.Soc4;
				dto.Soc5 = item.Soc5;
				dto.Soc6 = item.Soc6;
				dto.VcellLowValue = item.VcellLowValue;
				dto.VcellHighValue = item.VcellHighValue;
				dto.TcellLowValue = item.TcellLowValue;
				dto.TcellHighValue = item.TcellHighValue;
				dto.InvBatV = item.InvBatV;
				dto.BmsShutdown = item.BmsShutdown;
				dto.BmuRelay = item.BmuRelay;
				dto.BmsHardVer1 = item.BmsHardVer1;
				dto.BmsHardVer2 = item.BmsHardVer2;
				dto.BmsHardVer3 = item.BmsHardVer3;
				dto.DispatchSwitch = item.DispatchSwitch;
				dto.Pdispatch = item.Pdispatch;
				dto.DispatchSoc = item.DispatchSoc;
				dto.DispatchMode = item.DispatchMode;

				result.Add(dto);
			}

			return result;
		}

	}
}
