using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace AlphaEssWeb.Api_V2.Domain
{
	public class ComplaintsRepository : ComplaintsProcessingEntityRepository<Complaints, long>, IComplaintsRepository
	{
		public ComplaintsRepository() : base(new AlphaComplaintsProcessingDbContext()) { }
		public IList<Complaints> QueryComplaintsForInstaller(long? id, string installerName, string licNo, int pageIndex, int pageSize, out int total)
		{
			total = 0;
			var result = new List<Complaints>();
			var sqlWhere = new StringBuilder(string.Format(@"where c.IsDelete=0 and (c.Creator='{0}' or c.Creator in (select USERNAME from ALphaCloudDb.dbo.VUserAndRoles where ROLENAME='customer')) ", installerName));
			if (id.HasValue && id.Value > 0)
				sqlWhere.AppendFormat("and c.Id={0} ", id.Value.ToString());

			var sqlQuery = string.Format(@"declare @syssn table(syssn nvarchar(64));
insert into @syssn 
select SYS_SN from ALphaCloudDb.dbo.VT_SYSTEM where LIC_NO='{3}' and DELETE_FLAG=0;
select count(*) from @syssn as sn 
inner join Complaints as c on sn.syssn=c.SysSn  
{0}; 
select c.* from @syssn as sn 
inner join Complaints as c on sn.syssn=c.SysSn  
{0} 
order by c.CreateTime desc 
OFFSET(({1}-1)*{2}) ROWS 
FETCH NEXT {2} ROWS ONLY", sqlWhere.ToString(), pageIndex.ToString(), pageSize.ToString(), licNo);

			total = ExecuteSqlWithData(sqlQuery, true, result);
			return result;
		}

		private int ExecuteSqlWithData(string sql, bool withTotal, List<Complaints> list)
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
								list.Add(ReadComplaints(r));
						}
				}
				else
				{
					while (r.Read())
					{
						if (r.HasRows)
							list.Add(ReadComplaints(r));
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

		private Complaints ReadComplaints(DbDataReader r)
		{
			Complaints result = new Complaints();

			result.Key = r["Id"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["Id"].ToString()) ? Convert.ToInt64(r["Id"]) : 0;
			result.Title = r["Title"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["Title"].ToString()) ? r["Title"].ToString() : string.Empty;
			result.Description = r["Description"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["Description"].ToString()) ? r["Description"].ToString() : string.Empty;
			if (r["CreateTime"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["CreateTime"].ToString()))
				result.CreateTime = Convert.ToDateTime(r["CreateTime"]);
			if (r["UpdateTime"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["UpdateTime"].ToString()))
				result.UpdateTime = Convert.ToDateTime(r["UpdateTime"]);
			result.Creator = r["Creator"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["Creator"].ToString()) ? r["Creator"].ToString() : string.Empty;
			result.Status = r["Status"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["Status"].ToString()) ? Convert.ToInt32(r["Status"]) : 0;
			result.Recipient = r["Recipient"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["Recipient"].ToString()) ? r["Recipient"].ToString() : string.Empty;
			if (r["ReceiveTime"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["ReceiveTime"].ToString()))
				result.ReceiveTime = Convert.ToDateTime(r["ReceiveTime"]);
			result.CurrentProcessor = r["CurrentProcessor"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["CurrentProcessor"].ToString()) ? r["CurrentProcessor"].ToString() : string.Empty;
			result.Area = r["Area"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["Area"].ToString()) ? r["Area"].ToString() : string.Empty;
			result.Attachment = r["Attachment"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["Attachment"].ToString()) ? r["Attachment"].ToString() : string.Empty;
			result.Attachment2 = r["Attachment2"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["Attachment2"].ToString()) ? r["Attachment2"].ToString() : string.Empty;
			result.Attachment3 = r["Attachment3"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["Attachment3"].ToString()) ? r["Attachment3"].ToString() : string.Empty;
			result.IsDelete = r["IsDelete"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["IsDelete"].ToString()) ? Convert.ToInt32(r["IsDelete"]) : 0;
			result.SysSn = r["SysSn"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["SysSn"].ToString()) ? r["SysSn"].ToString() : string.Empty;
			result.OnsiteHandler = r["OnsiteHandler"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["OnsiteHandler"].ToString()) ? r["OnsiteHandler"].ToString() : string.Empty;
			result.SystemLicense = r["SystemLicense"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["SystemLicense"].ToString()) ? r["SystemLicense"].ToString() : string.Empty;
			result.SystemPostcode = r["SystemPostcode"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["SystemPostcode"].ToString()) ? r["SystemPostcode"].ToString() : string.Empty;
			result.SystemMinv = r["SystemMinv"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["SystemMinv"].ToString()) ? r["SystemMinv"].ToString() : string.Empty;
			result.ComplaintsType = r["ComplaintsType"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["ComplaintsType"].ToString()) ? r["ComplaintsType"].ToString() : string.Empty;
			result.Email = r["Email"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["Email"].ToString()) ? r["Email"].ToString() : string.Empty;
			result.ContactNumber = r["ContactNumber"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["ContactNumber"].ToString()) ? r["ContactNumber"].ToString() : string.Empty;
			result.ProcessingPriority = r["ProcessingPriority"] != DBNull.Value && !string.IsNullOrWhiteSpace(r["ProcessingPriority"].ToString()) ? Convert.ToInt32(r["ProcessingPriority"]) : 0;

			return result;
		}
	}
}
