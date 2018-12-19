using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Linq;

namespace AlphaEssWeb.Api_V2.Domain
{
	public static class SchedulingStrategyExtensions
	{
		public static SchedulingStrategy GetSingleByMicrogridId(this IEntityRepository<SchedulingStrategy, Guid> repository, Guid microgridId)
		{
			return repository.GetAll().FirstOrDefault(x => x.MicrogridId == microgridId && x.IsDelete == 0);
		}
	}
}
