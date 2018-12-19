using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Linq;

namespace AlphaEssWeb.Api_V2.Domain
{
	public static class SYS_SNRepositoryExtensions
	{
		public static SYS_SN GetSingleBySn(this IEntityRepository<SYS_SN, Guid> repository, string sn, System.Guid companyId)
		{
			return repository.GetAll().FirstOrDefault(x => x.CompanyId == companyId && x.SN_NO == sn && x.DELETE_FLAG == 0);
		}
	}
}
