using AlphaEss.Api_V2.Infrastructure;
using AlphaEssWeb.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Model.Dtos;
using AlphaEssWeb.Api_V2.Model.ExternalResponseModels;

namespace AlphaEssWeb.Api_V2.Model
{
	internal static class LastPowerDataExtension
	{
		internal static ExternalLastPowerDataResponseModel<PaginatedDto<PowerDataDto>> ToExternaLastPowerDataResponseModel(this OperationResult<PaginatedList<PowerData>> op)
		{
			var result = new ExternalLastPowerDataResponseModel<PaginatedDto<PowerDataDto>>() { ReturnCode = op.ReturnCode };
			if (op.Entity != null && op.Entity.TotalCount > 0)
				result.Result = op.Entity.ToPaginatedPowerDataDto();

			return result;
		}
	}
}
