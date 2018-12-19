using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Collections.Generic;

namespace AlphaEssWeb.Api_V2.Domain.Services
{
	public interface IRemoteDispatchService
	{
		OperationResult<Sys_RemoteDispatch> AddRemoteDispatch(string api_Account, long timeStamp, string sign, string token, string sn, int activePower, int reactivePower, decimal soc, int status, int controlMode, string ipAddress);
	}
}
