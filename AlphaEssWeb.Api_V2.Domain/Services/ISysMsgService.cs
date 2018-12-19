using AlphaEss.Api_V2.Infrastructure;

namespace AlphaEssWeb.Api_V2.Domain.Services
{
	public interface ISysMsgService
	{
		OperationResult<PaginatedList<SYS_MSGUSER>> GetMsg(string api_account, long timeStamp, string sign, string token, int onlyUnread, int pi, int ps);
		OperationResult UpdateMsgFlag(string api_account, long timeStamp, string sign, string token, int flag, string msgId);
	}
}
