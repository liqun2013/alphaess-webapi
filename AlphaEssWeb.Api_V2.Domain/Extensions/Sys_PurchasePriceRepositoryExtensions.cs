using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlphaEssWeb.Api_V2.Domain
{
	public static class Sys_PurchasePriceRepositoryExtensions
	{
		public static List<Sys_PurchasePrice> GetBySn(this IEntityRepository<Sys_PurchasePrice, Guid> repository, string sn)
		{
			return repository.GetAll().Where(x => x.PurchasePriceSn == sn && x.PurchasePriceDeleteFlag == 0).ToList();
		}
	}
}
