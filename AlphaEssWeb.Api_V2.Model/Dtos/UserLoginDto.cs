using System;
using System.Collections.Generic;

namespace AlphaEssWeb.Api_V2.Model.Dtos
{
	public sealed class UserLoginDto : ExternalResponseModels.ExternalBaseResponseModel
	{
		public string UserType { get; set; }
		public string Token { get; set; }
	}
}
