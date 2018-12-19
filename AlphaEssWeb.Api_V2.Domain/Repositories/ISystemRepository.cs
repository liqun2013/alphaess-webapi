using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Collections.Generic;

namespace AlphaEssWeb.Api_V2.Domain
{
	public interface ISystemRepository : IEntityRepository<VT_SYSTEM, Guid>
	{
		List<VT_SYSTEM> QueryVtSystemForReseller(Guid? companyId, string resellerId, int pageIndex, int pageSize, out int total);
		List<VT_SYSTEM> GetVtSystemByLicense(Guid? companyId, string license, int pageIndex, int pageSize, out int total);
		decimal CalculateTotalEarnings(string sn);
		SystemSummaryStatisticsData GetVtSystemStatusCountByLicense(Guid? companyId, string licenseNo);
		SystemSummaryStatisticsData GetSummaryInfoByUser(Guid? companyId, Guid userId);
	}
}
