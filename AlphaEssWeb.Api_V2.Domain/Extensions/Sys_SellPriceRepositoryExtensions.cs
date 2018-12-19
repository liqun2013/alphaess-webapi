using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Linq;

namespace AlphaEssWeb.Api_V2.Domain
{
	public static class Sys_SellPriceRepositoryExtensions
	{
		public static IQueryable<Sys_SellPrice> GetBySn(this IEntityRepository<Sys_SellPrice, Guid> repository, string sn)
		{
			return repository.GetAll().Where(x => x.SellPriceSn == sn && x.SellPriceDeleteFlag == 0);
		}
	}
}
