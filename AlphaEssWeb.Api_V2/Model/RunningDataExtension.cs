using AlphaEss.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Model.Dtos;
using AlphaEssWeb.Api_V2.Model.ExternalResponseModels;

using AlphaEssWeb.Api_V2.Model.ExternalRequestModels;

using System.Collections.Generic;

namespace AlphaEssWeb.Api_V2.Model
{
	internal static class RunningDataExtension
	{
		internal static ExternalVtColDataResponseModel<List<VtColDataDto>> ToExternaRunningDataResponseModel(this OperationResult<PaginatedList<VT_COLDATA>> op)
		{
			var result = new ExternalVtColDataResponseModel<List<VtColDataDto>>() { ReturnCode = op.ReturnCode };
			if (op.Entity != null && op.Entity.TotalCount > 0)
				result.Result = op.Entity.ToPaginatedRunningDataDto();

			return result;
		}
	}
}
