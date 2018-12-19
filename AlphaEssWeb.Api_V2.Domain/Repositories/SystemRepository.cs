using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace AlphaEssWeb.Api_V2.Domain
{
	public class SystemRepository : EntityRepository<VT_SYSTEM, Guid>, ISystemRepository
	{
		public SystemRepository() : base(new AlphaEssDbContext()) { }
		public List<VT_SYSTEM> QueryVtSystemForReseller(Guid? companyId, string resellerId, int pageIndex, int pageSize, out int total)
		{
			total = 0;
			List<VT_SYSTEM> result = new List<VT_SYSTEM>();

			var sqlWhere = string.Empty;
			if (companyId.HasValue)
				sqlWhere += string.Format(" and s.CompanyId='{0}'", companyId.ToString());

			var sql = string.Format(@"select count(s.SYS_SN) 
from VT_SYSTEM as s 
inner join Sys_ResellerLicense as rl on s.LIC_NO=rl.LicenseNo 
left join SYS_USER as u on rl.UserId=u.[USER_ID] 
where rl.UserId='{0}' and s.DELETE_FLAG=0 and s.SYS_SN is not null and s.SYS_SN <> '' ;
select s.*,u.* from VT_SYSTEM as s 
inner join Sys_ResellerLicense as rl on s.LIC_NO=rl.LicenseNo 
left join SYS_USER as u on rl.UserId=u.[USER_ID] 
where rl.UserId='{0}' and s.DELETE_FLAG=0 and s.SYS_SN is not null and s.SYS_SN <> ''  
{3} 
OFFSET(({1}-1)*{2}) ROWS 
FETCH NEXT {2} ROWS ONLY", resellerId, pageIndex.ToString(), pageSize.ToString(), sqlWhere);

			total = ExecuteSqlWithPartData(sql, true, result);

			return result;

		}

		public List<VT_SYSTEM> GetVtSystemByLicense(Guid? companyId, string license, int pageIndex, int pageSize, out int total)
		{
			if (string.IsNullOrWhiteSpace(license))
				throw new ArgumentNullException("license");
			total = 0;
			List<VT_SYSTEM> result = new List<VT_SYSTEM>();

			var sqlWhere = new StringBuilder(string.Format(@"where s.[DELETE_FLAG]=0 and s.SYS_SN is not null and s.SYS_SN <> '' and s.[LIC_NO]='{0}' ", license));
			if (companyId.HasValue)
				sqlWhere.AppendFormat(" and s.CompanyId = '{0}' ", companyId.ToString());

			var sql = string.Format(@"select count(*) from [dbo].[VT_SYSTEM] as s 
left join dbo.SYS_USER as u on s.USER_ID=u.USER_ID 
{0}; 
select s.*,u.* from [dbo].[VT_SYSTEM] as s 
left join dbo.SYS_USER as u on s.USER_ID=u.USER_ID 
{0} 
order by s.CREATE_DATETIME desc,s.[LIC_NO]
OFFSET(({1}-1)*{2}) ROWS 
FETCH NEXT {2} ROWS ONLY", sqlWhere, pageIndex.ToString(), pageSize.ToString());

			total = ExecuteSqlWithPartData(sql, true, result);

			return result;
		}

		public decimal CalculateTotalEarnings(string sn)
		{
			if (string.IsNullOrWhiteSpace(sn))
				throw new ArgumentNullException("sn");

			var p = new SqlParameter("@sn", sn);

			var result = _entitiesContext.Database.SqlQuery<decimal?>("CalculateTotalEarnings @sn", p);

			return result.FirstOrDefault() ?? 0;
		}
		public SystemSummaryStatisticsData GetSummaryInfoByUser(Guid? companyId, Guid userId)
		{
			SystemSummaryStatisticsData result = null;
			try
			{
				var data = _entitiesContext.Database.SqlQuery<SystemSummaryStatisticsData>(string.Format(@"exec GetSummaryInfoByUser '{0}','{1}'", companyId, userId));

				if (data != null)
					result = data.FirstOrDefault();
			}
			catch (Exception)
			{
				throw;
			}

			return result;
		}
		public SystemSummaryStatisticsData GetVtSystemStatusCountByLicense(Guid? companyId, string licenseNo)
		{
			SystemSummaryStatisticsData result = new SystemSummaryStatisticsData();

			var sqlWhere = string.Empty;
			if (companyId.HasValue)
				sqlWhere += string.Format(" and s.CompanyId='{0}'", companyId.ToString());
			if (!string.IsNullOrWhiteSpace(licenseNo))
				sqlWhere += string.Format(" and s.[LIC_NO]='{0}' ", licenseNo);

			var sql = string.Format(@"declare @OfflineCount int;
declare @NormalCount int;
declare @ProtectionCount int; 
declare @FaultCount int;
select @OfflineCount=count(SYSTEM_ID) from VT_SYSTEM as s with(nolock) 
where DELETE_FLAG=0 and (LastUploadTime < Dateadd(minute, -30,GETDATE()) or LastUploadTime is null) {0} 
select @NormalCount=count(SYSTEM_ID) from VT_SYSTEM as s with(nolock) 
where DELETE_FLAG=0 and LastUploadTime >= Dateadd(minute, -30,GETDATE()) and EmsStatus='Normal' {0} 
select @ProtectionCount=count(SYSTEM_ID) from VT_SYSTEM as s with(nolock) 
where DELETE_FLAG=0 and LastUploadTime >= Dateadd(minute, -30,GETDATE()) and (EmsStatus='Warning' or EmsStatus='Protection') {0} 
select @FaultCount=count(SYSTEM_ID) from VT_SYSTEM as s with(nolock) 
where DELETE_FLAG=0 and LastUploadTime >= Dateadd(minute, -30,GETDATE()) and EmsStatus='Fault' {0} 
select @OfflineCount as OfflineCount,@NormalCount as NormalCount,@ProtectionCount as ProtectionCount,@FaultCount as FaultCount", sqlWhere);

			var q = _entitiesContext.Database.SqlQuery<SystemSummaryStatisticsData>(sql);

			if (q.Any())
				result = q.First();

			return result;
		}

		private int ExecuteSqlWithPartData(string sql, bool withTotal, IList<VT_SYSTEM> list)
		{
			int total = 0;

			var conn = _entitiesContext.Database.Connection;
			try
			{
				var cmd = conn.CreateCommand();
				cmd.CommandText = sql;
				cmd.CommandType = System.Data.CommandType.Text;
				if (conn.State != System.Data.ConnectionState.Open)
					conn.Open();

				var r = cmd.ExecuteReader();
				if (withTotal && r.Read())
				{
					if (r.HasRows)
						total = r.GetInt32(0);
				}
				if (withTotal)
				{
					if (total > 0 && r.NextResult())
						while (r.Read())
						{
							if (r.HasRows)
								list.Add(ReadPartVtSystem(r));
						}
				}
				else
				{
					while (r.Read())
					{
						if (r.HasRows)
							list.Add(ReadPartVtSystem(r));
					}
				}
			}
			catch { throw; }
			finally
			{
				if (conn.State != System.Data.ConnectionState.Closed)
					conn.Close();
			}

			return total;
		}

		private VT_SYSTEM ReadPartVtSystem(DbDataReader r)
		{
			VT_SYSTEM result = new VT_SYSTEM();

			result.Key = r["SYSTEM_ID"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["SYSTEM_ID"].ToString()) ? new Guid(r["SYSTEM_ID"].ToString()) : Guid.Empty;
			if (r["CompanyId"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["CompanyId"].ToString()))
				result.CompanyId = new Guid(r["CompanyId"].ToString());
			result.SysSn = r["SYS_SN"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["SYS_SN"].ToString()) ? r["SYS_SN"].ToString() : string.Empty;
			result.CountryCode = r["COUNTRY_CODE"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["COUNTRY_CODE"].ToString()) ? r["COUNTRY_CODE"].ToString() : string.Empty;
			result.StateCode = r["STATE_CODE"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["STATE_CODE"].ToString()) ? r["STATE_CODE"].ToString() : string.Empty;
			result.CityCode = r["CITY_CODE"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["CITY_CODE"].ToString()) ? r["CITY_CODE"].ToString() : string.Empty;
			result.Address = r["ADDRESS"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["ADDRESS"].ToString()) ? r["ADDRESS"].ToString() : string.Empty;
			result.PostCode = r["POST_CODE"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["POST_CODE"].ToString()) ? r["POST_CODE"].ToString() : string.Empty;
			result.CellPhone = r["CELL_PHONE"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["CELL_PHONE"].ToString()) ? r["CELL_PHONE"].ToString() : string.Empty;
			if (r["USER_ID"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["USER_ID"].ToString()))
				result.UserId = new Guid(r["USER_ID"].ToString());
			result.MoneyType = r["MONEY_TYPE"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["MONEY_TYPE"].ToString()) ? r["MONEY_TYPE"].ToString() : string.Empty;
			if (r["POINV"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["POINV"].ToString()))
				result.Poinv = Convert.ToDecimal(r["POINV"]);
			result.Minv = r["MINV"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["MINV"].ToString()) ? r["MINV"].ToString() : string.Empty;
			if (r["POPV"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["POPV"].ToString()))
				result.Popv = Convert.ToDecimal(r["POPV"]);
			if (r["COBAT"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["COBAT"].ToString()))
				result.Cobat = Convert.ToDecimal(r["COBAT"]);
			if (r["USCAPACITY"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["USCAPACITY"].ToString()))
				result.Uscapacity = Convert.ToDecimal(r["USCAPACITY"]);
			result.Mbat = r["MBAT"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["MBAT"].ToString()) ? r["MBAT"].ToString() : string.Empty;
			result.RemarkSM = r["RemarkSM"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["RemarkSM"].ToString()) ? r["RemarkSM"].ToString() : string.Empty;
			result.AllowAutoUpdate = r["AllowAutoUpdate"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["AllowAutoUpdate"].ToString()) ? Convert.ToInt32(r["AllowAutoUpdate"]) : 0;
			result.EmsStatus = r["EMSStatus"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["EMSStatus"].ToString()) ? r["EMSStatus"].ToString() : string.Empty;
			result.Latitude = r["Latitude"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["Latitude"].ToString()) ? r["Latitude"].ToString() : string.Empty;
			result.Longitude = r["Longitude"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["Longitude"].ToString()) ? r["Longitude"].ToString() : string.Empty;
			result.RemarkI = r["RemarkI"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["RemarkI"].ToString()) ? r["RemarkI"].ToString() : string.Empty;
			result.LicNo = r["LIC_NO"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["LIC_NO"].ToString()) ? r["LIC_NO"].ToString() : string.Empty;
			if (r["CREATE_DATETIME"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["CREATE_DATETIME"].ToString()))
				result.CreateDatetime = Convert.ToDateTime(r["CREATE_DATETIME"]);
			result.Bmsversion = r["BMSVersion"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["BMSVersion"].ToString()) ? r["BMSVersion"].ToString() : string.Empty;
			result.Emsversion = r["EMSVersion"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["EMSVersion"].ToString()) ? r["EMSVersion"].ToString() : string.Empty;
			result.BMUModel = r["BMUModel"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["BMUModel"].ToString()) ? r["BMUModel"].ToString() : string.Empty;
			result.InventerSn = r["INVENTER_SN"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["INVENTER_SN"].ToString()) ? r["INVENTER_SN"].ToString() : string.Empty;
			if (r["GridType"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["GridType"].ToString()))
				result.GridType = Convert.ToDecimal(r["GridType"]);
			result.InvVersion = r["InvVersion"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["InvVersion"].ToString()) ? r["InvVersion"].ToString() : string.Empty;
			result.Acdc = r["ACDC"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["ACDC"].ToString()) ? r["ACDC"].ToString() : string.Empty;
			if (r["LastUploadTime"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["LastUploadTime"].ToString()))
				result.LastUploadTime = Convert.ToDateTime(r["LastUploadTime"]);
			if (r["ActiveTime"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["ActiveTime"].ToString()))
				result.ActiveTime = Convert.ToDateTime(r["ActiveTime"]);
			if (r["TransFrequency"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["TransFrequency"].ToString()))
				result.TransFrequency = Convert.ToInt32(r["TransFrequency"]);

			if (r["OEM_flag"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["OEM_flag"].ToString()))
				result.OEM_flag = Convert.ToInt32(r["OEM_flag"]);
			result.OEM_Plant_id = r["OEM_Plant_id"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["OEM_Plant_id"].ToString()) ? r["OEM_Plant_id"].ToString() : string.Empty;

			if (result.UserId.HasValue && r["USERNAME"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["USERNAME"].ToString()))
			{
				var u = new SYS_USER() { Key = result.UserId.Value };
				u.USERNAME = r["USERNAME"].ToString();
				u.CITYCODE = r["CITYCODE"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["CITYCODE"].ToString()) ? r["CITYCODE"].ToString() : string.Empty;
				u.STATECODE = r["STATECODE"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["STATECODE"].ToString()) ? r["STATECODE"].ToString() : string.Empty;
				u.COUNTRYCODE = r["COUNTRYCODE"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["COUNTRYCODE"].ToString()) ? r["COUNTRYCODE"].ToString() : string.Empty;
				u.POSTCODE = r["POSTCODE"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["POSTCODE"].ToString()) ? r["POSTCODE"].ToString() : string.Empty;
				u.LANGUAGECODE = r["LANGUAGECODE"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["LANGUAGECODE"].ToString()) ? r["LANGUAGECODE"].ToString() : string.Empty;

				result.SYS_USER = u;
			}

			return result;
		}
	}
}