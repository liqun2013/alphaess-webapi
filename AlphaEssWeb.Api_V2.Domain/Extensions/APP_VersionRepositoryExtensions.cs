using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Linq;

namespace AlphaEssWeb.Api_V2.Domain
{
	public static class APP_VersionRepositoryExtensions
	{
		public static IQueryable<APP_Version> GetAppVersionByAppType(this IEntityRepository<APP_Version, Guid> repository, string appType)
		{
			return repository.GetAll().Where(x => x.AppType == appType).OrderByDescending(x => x.AppVersionCode);
		}

		public static APP_Version GetLastAppVersionByAppType(this IEntityRepository<APP_Version, Guid> repository, string appType)
		{
			return repository.GetAll().Where(x => x.AppType == appType).OrderByDescending(x => x.AppVersionCode).FirstOrDefault();
		}
	}
}
