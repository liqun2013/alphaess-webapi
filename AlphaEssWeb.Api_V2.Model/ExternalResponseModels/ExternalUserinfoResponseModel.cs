using AlphaEssWeb.Api_V2.Model.Dtos;

namespace AlphaEssWeb.Api_V2.Model.ExternalResponseModels
{
	public class ExternalUserinfoResponseModel: ExternalBaseResponseModel
	{
		public SysUserDto Result { get; set; }
	}
}
