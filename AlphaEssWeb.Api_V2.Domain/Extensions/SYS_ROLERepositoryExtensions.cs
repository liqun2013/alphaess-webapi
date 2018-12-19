using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Linq;

namespace AlphaEssWeb.Api_V2.Domain
{
	public static class SYS_ROLERepositoryExtensions
	{
		public static SYS_ROLE GetSingleByRoleName(this IEntityRepository<SYS_ROLE, Guid> repository, string roleName)
		{
			return repository.GetAll().FirstOrDefault(x => x.ROLENAME == roleName);
		}
	}
}
