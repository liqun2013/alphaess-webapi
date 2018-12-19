using AlphaEssWeb.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Model.Dtos;

namespace AlphaEssWeb.Api_V2.Model
{
	internal static class SysMsgUserExtensions
	{
		public static SysMsgDto ToSysMsgDto(this SYS_MSGUSER mu)
		{
			return new SysMsgDto
			{
				CreateDatetime = mu.CREATE_DATETIME.HasValue ? mu.CREATE_DATETIME.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
				Id = mu.Key, Msgcont = mu.SYS_MSG.MSGCONT, MsgId = mu.SYS_MSG.Key.ToString(), Msgtitle = mu.SYS_MSG.MSGTITLE, MsgType = mu.SYS_MSG.MsgType ?? 0, ReadFlag = mu.USERREAD_FLAG, Sender = mu.SYS_USER.USERNAME
			};
		}
	}
}
