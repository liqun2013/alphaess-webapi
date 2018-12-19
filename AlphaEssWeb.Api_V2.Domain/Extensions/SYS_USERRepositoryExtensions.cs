using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Linq;

namespace AlphaEssWeb.Api_V2.Domain
{
	public static class SYS_USERRepositoryExtensions
	{
		public static SYS_USER GetSingleByUserName(this IEntityRepository<SYS_USER, Guid> repository, string userName)
		{
			return repository.GetAll().FirstOrDefault(u => u.USERNAME.Equals(userName, StringComparison.OrdinalIgnoreCase) && u.DELETE_FLAG == 0);
		}

		public static SYS_USER GetSingleByUserName(this IEntityRepository<SYS_USER, Guid> repository, string userName, System.Guid companyId)
		{
			return repository.GetAll().FirstOrDefault(u => u.USERNAME.Equals(userName, StringComparison.OrdinalIgnoreCase) && u.DELETE_FLAG == 0 && u.CompanyId == companyId);
		}
		public static SYS_USER GetSingleByKey(this IEntityRepository<SYS_USER, Guid> repository, Guid userId)
		{
			return repository.GetAll().FirstOrDefault(u => u.Key.Equals(userId) && u.DELETE_FLAG == 0);
		}

		public static SYS_USER GetSingleByLicenseNo(this IEntityRepository<SYS_USER, Guid> repository, string licenseNo)
		{
			return repository.GetAll().FirstOrDefault(x => x.LICNO == licenseNo && x.DELETE_FLAG == 0);
		}
	}
}
