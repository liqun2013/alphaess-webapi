using AlphaEss.Api_V2.Infrastructure;
using AlphaEssWeb.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Model.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace AlphaEssWeb.Api_V2.Model
{
	public static class PaginatedListExtensions
	{
		internal static PaginatedDto<VtSystemDto> ToPaginatedListVtSystemDto(this PaginatedList<VT_SYSTEM> pls)
		{
			return new PaginatedDto<VtSystemDto>
			{
				HasNextPage = pls.HasNextPage,
				HasPreviousPage = pls.HasPreviousPage,
				PageIndex = pls.PageIndex,
				PageSize = pls.PageSize,
				TotalCount = pls.TotalCount,
				TotalPageCount = pls.TotalPageCount,
				Items = pls.Select(x => x.ToVtSystemDto())
			};
		}

		internal static PaginatedDto<EnergyReportDataDto> ToPaginatedListEnergyDataDto(this PaginatedList<EnergyReportData> pl)
		{
			return new PaginatedDto<EnergyReportDataDto>
			{
				HasNextPage = pl.HasNextPage, HasPreviousPage = pl.HasPreviousPage, PageIndex = pl.PageIndex,
				PageSize = pl.PageSize, TotalCount = pl.TotalCount, TotalPageCount = pl.TotalPageCount, Items = pl.Select(x => x.ToEnergyReportDataDto())
			};
		}

		internal static PaginatedDto<ReportEnergyDto> ToPaginatedListReportEnergyDto(this PaginatedList<Report_Energy> pl)
		{
			return new PaginatedDto<ReportEnergyDto>
			{
				HasNextPage = pl.HasNextPage, HasPreviousPage = pl.HasPreviousPage, PageIndex = pl.PageIndex,
				PageSize = pl.PageSize, TotalCount = pl.TotalCount, TotalPageCount = pl.TotalPageCount, Items = pl.TotalCount > 0 ? pl.Select(x => x.ToReportEnergDto()).ToList() : new List<ReportEnergyDto>()
			};
		}

		internal static PaginatedDto<VtColDataDto> ToPaginatedRunningDataDto(this PaginatedList<VT_COLDATA> pls)
		{
			return new PaginatedDto<VtColDataDto>
			{
				HasNextPage = pls.HasNextPage, HasPreviousPage = pls.HasPreviousPage, PageIndex = pls.PageIndex,
				PageSize = pls.PageSize, TotalCount = pls.TotalCount, TotalPageCount = pls.TotalPageCount, Items = pls.Select(x => x.ToVtColDataDto())
			};
		}

		internal static PaginatedDto<PowerDataDto> ToPaginatedPowerDataDto(this PaginatedList<PowerData> pls)
		{
			return new PaginatedDto<PowerDataDto>
			{
				HasNextPage = pls.HasNextPage, HasPreviousPage = pls.HasPreviousPage, PageIndex = pls.PageIndex,
				PageSize = pls.PageSize, TotalCount = pls.TotalCount, TotalPageCount = pls.TotalPageCount, Items = pls.Select(x => x.ToPowerDataDto())
			};
		}

		internal static PaginatedDto<ComplaintsDto> ToPaginatedComplaintsDto(this PaginatedList<Complaints> cls)
		{
			return new PaginatedDto<ComplaintsDto>
			{
				HasNextPage = cls.HasNextPage, HasPreviousPage = cls.HasPreviousPage, PageIndex = cls.PageIndex,
				PageSize = cls.PageSize, TotalCount = cls.TotalCount, TotalPageCount = cls.TotalPageCount, Items = cls.Select(x => x.ToComplaintsDto())
			};
		}

		internal static PaginatedDto<SysMsgDto> ToPaginatedMsgDto(this PaginatedList<SYS_MSGUSER> cls)
		{
			return new PaginatedDto<SysMsgDto>
			{
				HasNextPage = cls.HasNextPage, HasPreviousPage = cls.HasPreviousPage, PageIndex = cls.PageIndex,
				PageSize = cls.PageSize, TotalCount = cls.TotalCount, TotalPageCount = cls.TotalPageCount, Items = cls.Select(x => x.ToSysMsgDto())
			};
		}
	}
}
