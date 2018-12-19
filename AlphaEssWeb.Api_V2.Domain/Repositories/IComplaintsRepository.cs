using AlphaEss.Api_V2.Infrastructure;
using System.Collections.Generic;

namespace AlphaEssWeb.Api_V2.Domain
{
	public interface IComplaintsRepository : IEntityRepository<Complaints, long>
	{
		IList<Complaints> QueryComplaintsForInstaller(long? id, string installerName, string licNo, int pageIndex, int pageSize, out int total);
	}
}
