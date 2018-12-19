using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Collections.Generic;

namespace AlphaEssWeb.Api_V2.Domain.Services
{
	public interface ICompanyContactDetailService
	{
		OperationResult<IEnumerable<CompanyContactDetail>> GetCompanyContacts(string api_account, long timeStamp, string sign, int flag);
	}
}
