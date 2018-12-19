using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Linq;

namespace AlphaEssWeb.Api_V2.Domain
{
	public static class DevDataRepositoryExtensions
	{
		public static DevData GetLastUpdateTimeByDeviceID(this IEntityRepository<DevData, Guid> repository, string deviceID)
		{
			return repository.GetAll().Where(x => x.DeviceID == deviceID.Trim()).OrderByDescending(x => x.Upload_DateTime).FirstOrDefault();
		}

		public static IQueryable<DevData> GetDeviceCVByDeviceID(this IEntityRepository<DevData, Guid> repository, string deviceID, string localDate)
		{
			DateTime dt = DateTime.MinValue;
			DateTime.TryParse(localDate.Trim(), out dt);
			DateTime dateE = dt.AddDays(1);

			return repository.FindBy(x => x.DeviceID == deviceID.Trim()).Where(x => x.Upload_DateTime >= dt).Where(x => x.Upload_DateTime < dateE).OrderByDescending(x => x.Upload_DateTime);
		}
	}
}
