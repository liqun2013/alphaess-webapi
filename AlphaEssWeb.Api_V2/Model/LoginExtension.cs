using AlphaEss.Api_V2.Infrastructure;
using AlphaEssWeb.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Model.Dtos;
using AlphaEssWeb.Api_V2.Model.ExternalResponseModels;

namespace AlphaEssWeb.Api_V2.Model
{
	internal static class LoginExtension
	{
		public static UserLoginDto ToExternalLoginResponseModel(this OperationResult or, string userType, string token)
		{
			return new UserLoginDto
			{
				UserType = userType,
				Token = token,
				ReturnCode = or.ReturnCode
			};
		}

		internal static ExternalSystemListResponseModel<PaginatedDto<VtSystemDto>> ToExternalSystemListResponseModel(this OperationResult<PaginatedList<VT_SYSTEM>> op)
		{
			var result = new ExternalSystemListResponseModel<PaginatedDto<VtSystemDto>>() { ReturnCode = op.ReturnCode };
			if (op.Entity != null)
				result.Result = op.Entity.ToPaginatedListVtSystemDto();

			return result;
    }
	}
}
