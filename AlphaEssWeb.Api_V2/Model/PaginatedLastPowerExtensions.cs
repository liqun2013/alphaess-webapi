using AlphaEss.Api_V2.Infrastructure;
using AlphaEssWeb.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Model.Dtos;
using System.Collections.Generic;

namespace AlphaEssWeb.Api_V2.Model
{
	public static class PaginatedLastPowerExtensions
	{
		internal static List<PowerDataDto> ToPaginatedLastPowerDataDto(this PaginatedList<PowerData> pls)
		{
			List<PowerDataDto> result = new List<PowerDataDto>();
			foreach (var item in pls)
			{
				PowerDataDto dto = new PowerDataDto();
				dto.Id = item.Key;
				dto.Sn = item.Sn;
				dto.UploadTime = item.UploadTime;
				dto.Ppv1 = item.Ppv1;
				dto.Ppv2 = item.Ppv2;
				dto.PrealL1 = item.PrealL1;
				dto.PrealL2 = item.PrealL2;
				dto.PrealL3 = item.PrealL3;
				dto.PmeterL1 = item.PmeterL1;
				dto.PmeterL2 = item.PmeterL2;
				dto.PmeterL3 = item.PmeterL3;
				dto.PmeterDc = item.PmeterDc;
				dto.Pbat = item.Pbat;
				dto.Pva = item.Sva;
				dto.VarAC = item.VarAC;
				dto.VarDC = item.VarDC;

				result.Add(dto);
			}

			return result;
		}

	}
}
