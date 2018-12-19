using AlphaEssWeb.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Model.Dtos;

namespace AlphaEssWeb.Api_V2.Model
{
	internal static class SYS_USERExtension
	{
		public static SysUserDto ToSysUserDto(this SYS_USER u)
		{
			return new SysUserDto
			{
				UserName = u.USERNAME,
				Id = u.Key
			};
		}
	}
}
