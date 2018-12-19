using AlphaEss.Api_V2.Infrastructure;
using AlphaEssWeb.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Model.Dtos;
using AlphaEssWeb.Api_V2.Model.ExternalResponseModels;
using System.Collections.Generic;

namespace AlphaEssWeb.Api_V2.Model
{
	internal static class ReportInfoExtension
	{
		internal static ExternalReportEnergyResponseModel<List<ReportEnergyDto>> ToExternaReportEnergyResponseModel(this OperationResult<PaginatedList<Report_Energy>> op)
		{
			var result = new ExternalReportEnergyResponseModel<List<ReportEnergyDto>>() { ReturnCode = op.ReturnCode };
			if (op.Entity != null && op.Entity.TotalCount > 0)
				result.Result = op.Entity.ToPaginatedReportEnergyDto();

			return result;
		}
	}
}
