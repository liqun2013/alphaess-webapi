using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Linq;

namespace AlphaEssWeb.Api_V2.Domain
{
	public static class DeviceInfoRepositoryExtensions
	{
		public static DevInfo GetDeviceInfoByDeviceID(this IEntityRepository<DevInfo, Guid> repository, string deviceID)
		{
			return repository.GetAll().FirstOrDefault(x => x.DeviceID == deviceID.Trim() && x.Delete_Flag == 0);
		}
	}
}
