using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Collections;
using System.Text;

namespace AlphaEssWeb.Api_V2.Domain.Services
{
	public class DevInfoService : IDevInfoService
	{
		private readonly IEntityRepository<DevInfo, Guid> _devInfoRepository;
		private readonly IEntityRepository<DevData, Guid> _devDataRepository;
		private readonly IParameterValidateService _parameterValidateService;
		private readonly ICryptoService _cryptoService;
		public DevInfoService(IEntityRepository<DevInfo, Guid> devInfoRepository, IEntityRepository<DevData, Guid> devDataRepository, IParameterValidateService parameterValidateService, ICryptoService cryptoService)
		{
			_devInfoRepository = devInfoRepository;
			_devDataRepository = devDataRepository;
			_parameterValidateService = parameterValidateService;
			_cryptoService = cryptoService;
		}

		public OperationResult<DeviceInfo> GetDeviceInfo(string api_account, long timeStamp, string sign, string deviceID)
		{
			if (!checkSignForGetDeviceInfo(api_account, timeStamp, sign, deviceID))
			{
				var result4 = new OperationResult<DeviceInfo>(OperationCode.Error_Sign);
				result4.Entity = null;

				return result4;
			}


			if (!_parameterValidateService.CheckTimestamp(timeStamp))
			{
				var result2 = new OperationResult<DeviceInfo>(OperationCode.Error_TimeStamp);
				result2.Entity = null;

				return result2;
			}

			Guid theCompanyId;
			if (!_parameterValidateService.ApiAccountExist(api_account, out theCompanyId))
			{
				var result3 = new OperationResult<DeviceInfo>(OperationCode.Error_ApiAccountNotExist);
				result3.Entity = null;

				return result3;
			}

			DeviceInfo inf = new DeviceInfo();

			DevInfo info = _devInfoRepository.GetDeviceInfoByDeviceID(deviceID);
			DevData data = _devDataRepository.GetLastUpdateTimeByDeviceID(deviceID);

			if (data != null)
			{
				inf.UpdateTime = data.Upload_DateTime.ToString();
			}
			else {
				inf.UpdateTime = string.Empty;
				inf.Status = "Failed2";
			}

			if (info != null)
			{
				inf.DeviceID = info.DeviceID;
				inf.Voltage = info.Voltage;
				inf.TimeZone = info.TimeZone;
			}
			else {
				inf.DeviceID = string.Empty;
				inf.Voltage = 0;
				inf.TimeZone = string.Empty;
				inf.Status = "Failed1";
			}

			if (string.IsNullOrEmpty(inf.Status))
			{
				inf.Status = "Success";
			}


			var result = new OperationResult<DeviceInfo>(OperationCode.Success);
			result.Entity = inf;

			return result;
		}

		private bool checkSignForGetDeviceInfo(string api_account, long timeStamp, string sign, string deviceID)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timeStamp", timeStamp.ToString());
			slParams.Add("DeviceID", deviceID);
			slParams.Add("secretkey", _cryptoService.SecretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}
	}
}
