using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Collections.Generic;

namespace AlphaEssWeb.Api_V2.Domain.Services
{
	public interface IComplaintsService
	{
		Complaints AddNewComplaint(Complaints complaint);
		OperationResult AddNewComplaints(string api_account, long timeStamp, string sign, string token, string title, string description, string complaintsTyps, string email, string contactNumber, string sn, string attachment1, string attachment2, string attachment3);
		OperationResult<PaginatedList<Complaints>> GetComplaintsList(string api_account, long timeStamp, string sign, string token, int pi, int ps);
		OperationResult EvaluateComplaints(string api_account, long timeStamp, string sign, string token, long cpid, int satsf, int satsf1, int satsf2, string content);
	}
}
