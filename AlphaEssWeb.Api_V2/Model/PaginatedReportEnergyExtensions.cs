using AlphaEss.Api_V2.Infrastructure;
using AlphaEssWeb.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Model.Dtos;
using System;
using System.Collections.Generic;

namespace AlphaEssWeb.Api_V2.Model
{
	public static class PaginatedReportEnergyExtensions
	{
		internal static List<ReportEnergyDto> ToPaginatedReportEnergyDto(this PaginatedList<Report_Energy> pls)
		{
			List<ReportEnergyDto> result = new List<ReportEnergyDto>();
			foreach (var item in pls)
			{
				ReportEnergyDto dto = new ReportEnergyDto();
				dto.Id = Guid.NewGuid();
				dto.Sn = item.SysSn;
				dto.Ebat = item.Ebat;
				dto.Echarge = item.Echarge;
				dto.Eeff = item.Eeff;
				dto.EGrid2Load = item.EGrid2Load;
				dto.EGridCharge = item.EGridCharge;
				dto.Einput = item.Einput;
				dto.Eload = item.Eload;
				dto.Eoutput = item.Eoutput;
				dto.Epv2Load = item.Epv2Load;
				dto.Epvtotal = item.Epvtotal;
				dto.EselfConsumption = item.EselfConsumption;
				dto.EselfSufficiency = item.EselfSufficiency;

				result.Add(dto);
			}

			return result;
		}

	}
}
