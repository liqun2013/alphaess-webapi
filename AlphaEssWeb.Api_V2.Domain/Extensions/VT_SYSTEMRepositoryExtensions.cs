using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Linq;

namespace AlphaEssWeb.Api_V2.Domain
{
	public static class VT_SYSTEMRepositoryExtensions
	{
		public static VT_SYSTEM GetSystemByUser(this IEntityRepository<VT_SYSTEM, Guid> repository, Guid userId)
		{
			return repository.GetAll().FirstOrDefault(x => x.UserId == userId && x.DeleteFlag == 0);
		}

		public static VT_SYSTEM GetSystemBySn(this IEntityRepository<VT_SYSTEM, Guid> repository, string sn)
		{
			return repository.GetAll().FirstOrDefault(x => x.SysSn == sn && x.DeleteFlag == 0);
		}

		public static VT_SYSTEM GetSystemBySn(this IEntityRepository<VT_SYSTEM, Guid> repository, string sn, Guid companyId)
		{
			return repository.GetAll().FirstOrDefault(x => x.SysSn == sn && x.DeleteFlag == 0 && x.CompanyId == companyId);
		}

		public static IQueryable<VT_SYSTEM> GetSystemsByUser(this IEntityRepository<VT_SYSTEM, Guid> repository, Guid userId)
		{
			return repository.GetAll().Where(x => x.UserId == userId && x.DeleteFlag == 0);
		}

		public static IQueryable<VT_SYSTEM> GetSystemsByUser(this IEntityRepository<VT_SYSTEM, Guid> repository, Guid userId, Guid companyId)
		{
			return repository.GetAll().Where(x => x.UserId == userId && x.CompanyId == companyId && x.DeleteFlag == 0);
		}

		public static IQueryable<VT_SYSTEM> GetSystemBySns(this IEntityRepository<VT_SYSTEM, Guid> repository, string[] sns)
		{
			if (sns == null || sns.Length <= 0)
				throw new ArgumentException("invalid parameter", "sns");

			return repository.GetAll().Where(x => sns.Contains(x.SysSn) && x.DeleteFlag == 0);
		}

		public static VT_SYSTEM GetSystemById(this IEntityRepository<VT_SYSTEM, Guid> repository, Guid id)
		{
			return repository.GetAll().FirstOrDefault(x => x.Key == id);
		}

		//public static IQueryable<VT_SYSTEM> LightQuery(this IEntityRepository<VT_SYSTEM> repository)
		//{
		//	return repository.AlphaEssDbContext.VT_SYSTEM;
		//}
	}
}
