using AlphaEss.Api_V2.Infrastructure;
using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlphaEssWeb.Api_V2.Domain.Services
{
	public class DevDataService : IDevDataService
	{
		private readonly IEntityRepository<DevData, Guid> _devDataRepository;
		private readonly IEntityRepository<DevInfo, Guid> _devInfoRepository;
		private readonly IParameterValidateService _parameterValidateService;
		private readonly ICryptoService _cryptoService;

		public DevDataService(IEntityRepository<DevData, Guid> devDataRepository, IEntityRepository<DevInfo, Guid> devInfoRepository, IParameterValidateService parameterValidateService, ICryptoService cryptoService)
		{
			_devDataRepository = devDataRepository;
			_devInfoRepository = devInfoRepository;
			_parameterValidateService = parameterValidateService;
			_cryptoService = cryptoService;
		}

		public OperationResult<DeviceCV> GetDeviceCV(string api_account, long timeStamp, string sign, string deviceID, string localDate)
		{
			if (!checkSignForGetDeviceCV(api_account, timeStamp, sign, deviceID, localDate))
			{
				var result4 = new OperationResult<DeviceCV>(OperationCode.Error_Sign);
				result4.Entity = null;

				return result4;
			}


			if (!_parameterValidateService.CheckTimestamp(timeStamp))
			{
				var result2 = new OperationResult<DeviceCV>(OperationCode.Error_TimeStamp);
				result2.Entity = null;

				return result2;
			}

			Guid theCompanyId;
			if (!_parameterValidateService.ApiAccountExist(api_account, out theCompanyId))
			{
				var result3 = new OperationResult<DeviceCV>(OperationCode.Error_ApiAccountNotExist);
				result3.Entity = null;

				return result3;
			}

			DeviceCV cv = new DeviceCV();
			DevInfo info = _devInfoRepository.FindBy(x => x.DeviceID == deviceID).Where(x => x.Delete_Flag == 0).FirstOrDefault();
			var result = new OperationResult<DeviceCV>(OperationCode.Success);

			if (info == null)
			{
				result = new OperationResult<DeviceCV>(OperationCode.Success);
				cv.Status = "Failed1";
				cv.Time = null;
				cv.CVTotal = null;
				cv.CV1 = null;
				cv.CV2 = null;
				cv.CV3 = null;

				result.Entity = cv;

				return result;
			}

			var dataLst = _devDataRepository.GetDeviceCVByDeviceID(deviceID, localDate).ToList();

			if (dataLst == null || dataLst.Count == 0)
			{
				result = new OperationResult<DeviceCV>(OperationCode.Success);
				cv.Status = "Failed2";
				cv.Time = null;
				cv.CVTotal = null;
				cv.CV1 = null;
				cv.CV2 = null;
				cv.CV3 = null;

				result.Entity = cv;

				return result;
			}

			int num = dataLst.Count;

			string[] times = new string[num];
			decimal[] CVTotals = new decimal[num];
			decimal[] CVs1 = new decimal[num];
			decimal[] CVs2 = new decimal[num];
			decimal[] CVs3 = new decimal[num];

			for (int i = 0; i < num; i++)
			{
				times[i] = dataLst[i].Upload_DateTime.ToString();
				CVTotals[i] = dataLst[i].CV;
				CVs1[i] = dataLst[i].CV1 == null ? 0 : (decimal)dataLst[i].CV1;
				CVs2[i] = dataLst[i].CV2 == null ? 0 : (decimal)dataLst[i].CV2;
				CVs3[i] = dataLst[i].CV3 == null ? 0 : (decimal)dataLst[i].CV3;
			}

			result = new OperationResult<DeviceCV>(OperationCode.Success);
			cv.Status = "Success";
			cv.Time = times;
			cv.CVTotal = CVTotals;
			cv.CV1 = CVs1;
			cv.CV2 = CVs2;
			cv.CV3 = CVs3;

			result.Entity = cv;

			return result;
		}

		private bool checkSignForGetDeviceCV(string api_account, long timeStamp, string sign, string deviceID, string localDate)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timeStamp", timeStamp.ToString());
			slParams.Add("DeviceID", deviceID);
			slParams.Add("localDate", localDate);
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
