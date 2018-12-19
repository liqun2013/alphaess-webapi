using AlphaEssWeb.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Model.Dtos;

namespace AlphaEssWeb.Api_V2.Model
{
	internal static class RemoteDispatchExtension
	{
		public static RemoteDispatchDto ToRemoteDispatchDto(this Sys_RemoteDispatch ms)
		{
			return new RemoteDispatchDto
			{
				Id = ms.Key, ActivePower = ms.ActivePower, ControlMode = ms.ControlMode, CreateTime = ms.CreateTime, DELETE_FLAG = ms.DELETE_FLAG, ReactivePower = ms.ReactivePower, SN = ms.SN, SOC = ms.SOC, Status = ms.Status, UpdateTime = ms.UpdateTime, UserName = ms.UserName
			};
		}
	}
}
