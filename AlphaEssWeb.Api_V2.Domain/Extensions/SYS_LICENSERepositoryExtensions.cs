using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Linq;

namespace AlphaEssWeb.Api_V2.Domain
{
	public static class SYS_LICENSERepositoryExtensions
	{
		public static SYS_LICENSE GetSingleByLicenseNo(this IEntityRepository<SYS_LICENSE, Guid> repository, string licenseNo, System.Guid companyId)
		{
			return repository.GetAll().FirstOrDefault(x => x.LICENSE_NO == licenseNo && x.CompanyId == companyId);
		}
  }
}
