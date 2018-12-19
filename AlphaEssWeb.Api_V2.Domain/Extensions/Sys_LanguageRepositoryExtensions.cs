using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Linq;

namespace AlphaEssWeb.Api_V2.Domain
{
	public static class Sys_LanguageRepositoryExtensions
	{
		public static Sys_Language GetByKey(this IEntityRepository<Sys_Language, Guid> repository, string k, string l)
		{
			return repository.GetAll().FirstOrDefault(x => x.LanguageKey == k && x.LanguageCategory == l);
		}
		public static IQueryable<Sys_Language> GetByLanguage(this IEntityRepository<Sys_Language, Guid> repository, string l)
		{
			return repository.GetAll().Where(x => x.LanguageCategory == l);
		}
	}
}
