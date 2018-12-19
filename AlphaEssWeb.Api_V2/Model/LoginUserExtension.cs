using AlphaEss.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Model.Dtos;
using AlphaEssWeb.Api_V2.Model.ExternalResponseModels;

using AlphaEssWeb.Api_V2.Model.ExternalRequestModels;

using System.Collections.Generic;

namespace AlphaEssWeb.Api_V2.Model
{
	internal static class LoginUserExtension
	{
		public static UserLoginDto ToExternalLoginUserResponseModel(this OperationResult<UserLogin> ms)
		{
			return new UserLoginDto
			{
				userType = ms.Entity == null ? "" : ms.Entity.userType,
				Token = ms.Entity == null ? "" : ms.Entity.token.ToString(),
				ReturnCode = ms.ReturnCode
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
