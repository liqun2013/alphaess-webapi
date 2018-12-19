using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Linq;

namespace AlphaEssWeb.Api_V2.Domain
{
	public static class SYS_APIRepositoryExtensions
	{
		public static SYS_API GetSingleByAccount(this IEntityRepository<SYS_API, Guid> repository, string account)
		{
			return repository.GetAll().FirstOrDefault(x => x.Api_Account == account);
		}
	}
}
