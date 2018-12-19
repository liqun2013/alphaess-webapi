using AlphaEssWeb.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Model.Dtos;

namespace AlphaEssWeb.Api_V2.Model
{
	internal static class App_VersionExtensions
	{
		public static AppVersionDto ToAppVersionDto(this APP_Version appVersion)
		{
			return new AppVersionDto
			{
				AppCreateTime = appVersion.AppCreateTime,
				AppDownloadUrl = appVersion.AppDownloadUrl,
				AppForcedUpdates = appVersion.AppForcedUpdates,
				AppType = appVersion.AppType,
				AppVersion = appVersion.AppVersion,
				AppVersionCode = appVersion.AppVersionCode,
				Id = appVersion.Key
			};
		}
	}
}
