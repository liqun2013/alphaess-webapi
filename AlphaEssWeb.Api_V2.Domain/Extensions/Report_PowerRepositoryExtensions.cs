using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace AlphaEssWeb.Api_V2.Domain
{
	public static class Report_PowerRepositoryExtensions
	{
		public static List<Report_Power> GetTodayReportPower(this IEntityRepository<Report_Power, long> repository, string sn, DateTime d, decimal gridType)
		{
			var sql = string.Format(@"EXECUTE [dbo].[GetOneDayPowerReport] '{0}','{1}',{2}", d.Date.ToString("yyyy-MM-dd"), sn, gridType.ToString());

			return repository.ExecSql(sql);
		}

		public static List<Report_Power> GetByDate(this IEntityRepository<Report_Power, long> repository, string sn, DateTime d, short? workmode)
		{
			List<Report_Power> result = null;

			var q = repository.GetAll().Where(x => x.SysSn == sn && x.TheDate == d.Date);
			if (q != null && q.Any())
			{
				if (workmode.HasValue)
				{
					q = q.Where(x => x.WorkMode == workmode.Value);
					if (q != null && q.Any())
						result = q.ToList();
				}
				else
					result = q.ToList();
			}

			return result;
		}
	}
}
