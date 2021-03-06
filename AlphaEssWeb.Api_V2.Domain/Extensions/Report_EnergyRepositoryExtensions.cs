﻿using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlphaEssWeb.Api_V2.Domain
{
	public static class Report_EnergyRepositoryExtensions
	{
		public static List<Report_Energy> GetTodayEnergyReport(this IEntityRepository<Report_Energy, long> repository, string sn, DateTime d, decimal cobat)
		{
			var sql = string.Format(@"EXECUTE [dbo].[GetOneDayEnergyReport] '{0}','{1}',{2}", d.Date.ToString("yyyy-MM-dd"), sn, cobat.ToString());
			
			return repository.ExecSql(sql);
		}

		public static List<Report_Energy> GetEnergyReportBySns(this IEntityRepository<Report_Energy, long> repository, List<string> sns, DateTime d, short? workmode)
		{
			List<Report_Energy> result = null;

			if (sns != null && sns.Any())
			{
				var query = repository.GetAll().Where(x => sns.Contains(x.SysSn) && x.TheDate == d);
				if (query != null && query.Any())
				{
					if (workmode.HasValue)
					{
						query = query.Where(x => x.WorkMode == workmode.Value);
						if (query != null && query.Any())
							result = query.ToList();
					}
					else
						result = query.ToList();
				}
			}

			return result;
		}

		public static List<Report_Energy> GetReportEnergyByPeriod(this IEntityRepository<Report_Energy, long> repository, string sn, DateTime begin, DateTime end, short? workmode)
		{
			List<Report_Energy> result = null;
			var query = repository.GetAll().Where(x => x.SysSn == sn && x.TheDate >= begin && x.TheDate <= end);

			if (query != null && query.Any())
			{
				if (workmode.HasValue)
				{
					query = query.Where(x => x.WorkMode == workmode.Value);
					if (query != null && query.Any())
						result = query.ToList();
				}
				else
					result = query.ToList();
			}

			return result;
		}
	}
}
