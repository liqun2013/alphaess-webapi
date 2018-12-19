using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AlphaEssWeb.Api_V2.Domain.Services
{
	public class SystemService : ISystemService
	{
		private readonly IEntityRepository<SYS_USER, Guid> _userRepository;
		private readonly ISystemRepository _systemRepository;
		private readonly IEntityRepository<VT_SYSINSTALL, Guid> _sysInstallRepository;
		private readonly IEntityRepository<VT_COLDATA, Guid> _coldataRepository;
		private readonly IEntityRepository<Report_Energy, long> _reportEnergyRepository;
		private readonly IEntityRepository<Report_Power, long> _reportPowerRepository;
		private readonly IEntityRepository<Report_Income, long> _reportIncomeRepository;
		private readonly IEntityRepository<SYS_API, Guid> _apiRepository;
		private readonly IEntityRepository<PowerData, Guid> _powerDataRepository;
		private readonly IEntityRepository<Sys_RemoteDispatch, Guid> _remotedispatchRepository;
		private readonly IEntityRepository<Sys_PurchasePrice, Guid> _sysPurchasePriceRepository;
		private readonly IEntityRepository<Sys_SellPrice, Guid> _sysSellPriceRepository;
		private readonly IEntityRepository<SysWeatherForecast, Guid> _weatherForecastRepository;
		private readonly IEntityRepository<SYS_ROLEUSER, Guid> _roleUserRepository;
		private readonly IEntityRepository<SYS_ROLE, Guid> _roleRepository;
		private readonly IEntityRepository<SYS_SN, Guid> _snRepository;
		private readonly IEntityRepository<BASE_COUNTRY, int> _baseCountryRepository;
		private readonly IEntityRepository<SYS_APPVERSION, Guid> _appversionRepository;
		private readonly IEntityRepository<SYS_APPVERSIONREC, Guid> _appversionRecRepository;
		private readonly IParameterValidateService _parameterValidateService;
		private readonly ICryptoService _cryptoService;
		private readonly IAlphaRemotingService _alphaRemotingService;
		private readonly IEntityRepository<SYS_LOG, Guid> _syslogRepository;

		public SystemService(IEntityRepository<SYS_USER, Guid> userRepository, ISystemRepository systemRepository,
			IEntityRepository<VT_SYSINSTALL, Guid> sysInstallRepository, IEntityRepository<VT_COLDATA, Guid> coldataRepository, IEntityRepository<Report_Energy, long> reRepository,
			IEntityRepository<Report_Power, long> rpRepository, IEntityRepository<SYS_API, Guid> apiRepository, IEntityRepository<PowerData, Guid> powerDataRepository,
			IEntityRepository<Sys_RemoteDispatch, Guid> remotedispatchRepository, IEntityRepository<Sys_PurchasePrice, Guid> sysPurchasePriceRepository, IEntityRepository<Sys_SellPrice, Guid> sysSellPriceRepository,
			IEntityRepository<SysWeatherForecast, Guid> weatherForecastRepository, IEntityRepository<SYS_ROLEUSER, Guid> roleUserRepository, IEntityRepository<SYS_ROLE, Guid> roleRepository, IEntityRepository<Report_Income, long> reportIncomeRepository,
			IEntityRepository<SYS_SN, Guid> snRepository, IEntityRepository<BASE_COUNTRY, int> baseCountryRepository, IEntityRepository<SYS_LOG, Guid> syslogRepository, IEntityRepository<SYS_APPVERSION, Guid> appversionRepository, IEntityRepository<SYS_APPVERSIONREC, Guid> appversionRecRepository,
		IParameterValidateService parameterValidateService, ICryptoService cryptoService, IAlphaRemotingService alphaRemotingService)
		{
			_appversionRecRepository = appversionRecRepository;
			_appversionRepository = appversionRepository;
			_syslogRepository = syslogRepository;
			_baseCountryRepository = baseCountryRepository;
			_userRepository = userRepository;
			_systemRepository = systemRepository;
			_sysInstallRepository = sysInstallRepository;
			_coldataRepository = coldataRepository;
			_reportEnergyRepository = reRepository;
			_reportPowerRepository = rpRepository;
			_powerDataRepository = powerDataRepository;
			_parameterValidateService = parameterValidateService;
			_remotedispatchRepository = remotedispatchRepository;
			_sysSellPriceRepository = sysSellPriceRepository;
			_sysPurchasePriceRepository = sysPurchasePriceRepository;
			_cryptoService = cryptoService;
			_apiRepository = apiRepository;
			_alphaRemotingService = alphaRemotingService;
			_weatherForecastRepository = weatherForecastRepository;
			_roleUserRepository = roleUserRepository;
			_roleRepository = roleRepository;
			_snRepository = snRepository;
			_reportIncomeRepository = reportIncomeRepository;
		}

		/// <summary>
		/// 获取已安装的设备
		/// </summary>
		/// <returns>IQueryable&lt;VT_SYSTEM&gt;</returns>
		private IQueryable<VT_SYSTEM> QueryInstalledSystem(Guid companyId)
		{
			var installedSn = _sysInstallRepository.GetAll().Where(x => x.CompanyId == companyId && x.DELETE_FLAG == 0).Select(x => x.SYS_SN).Distinct();
			return _systemRepository.AllIncluding(x => x.SYS_USER).Where(x => installedSn.Contains(x.SysSn));
		}

		/// <summary>
		/// 为根据用户名获取系统列表检查签名
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="pageIndex">指定页</param>
		/// <param name="pageSize">每页最大记录数</param>
		/// <param name="username">用户名</param>
		/// <returns>签名正确:true/签名错误:false</returns>
		private bool checkSignForGetSystemByUser(string api_account, long timeStamp, string sign, int pageIndex, int pageSize, string token, string secretKey)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("pageindex", pageIndex.ToString());
			slParams.Add("pagesize", pageSize.ToString());
			slParams.Add("Token", token);
			slParams.Add("secretkey", secretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		/// <summary>
		/// 为获取指定sn功率数据检查签名
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="sn">sn</param>
		/// <param name="username">用户名</param>
		/// <param name="date">日期</param>
		/// <returns>签名正确:true/签名错误:false</returns>
		private bool checkSignForGetPowerData(string api_account, long timeStamp, string sign, string sn, string username, string date, string secretKey, string token)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("Token", token);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("sn", string.IsNullOrEmpty(sn) ? string.Empty : sn);
			slParams.Add("date", date);
			slParams.Add("username", string.IsNullOrEmpty(username) ? string.Empty : username);
			slParams.Add("secretkey", secretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		/// <summary>
		/// 为获取指定sn的能量数据检查签名
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="sn">sn</param>
		/// <param name="username">用户名</param>
		/// <param name="start">开始日期</param>
		/// <param name="end">结束日期</param>
		/// <returns>签名正确:true/签名错误:false</returns>
		private bool checkSignForGetEnergeData(string api_account, long timeStamp, string sign, string sn, string username, string start, string end, string statisticsby, string secretKey, string token)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("sn", string.IsNullOrEmpty(sn) ? string.Empty : sn);
			slParams.Add("Token", token);
			slParams.Add("statisticsby", string.IsNullOrEmpty(statisticsby) ? string.Empty : statisticsby);
			slParams.Add("date_start", start);
			slParams.Add("date_end", end);
			slParams.Add("username", string.IsNullOrEmpty(username) ? string.Empty : username);
			slParams.Add("secretkey", secretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		private bool checkSignForGetProfitData(string api_account, long timeStamp, string sign, string sn, string username, string start, string end, string statisticsby, string api_SecretKey, string token)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("sn", string.IsNullOrEmpty(sn) ? string.Empty : sn);
			slParams.Add("Token", token);
			slParams.Add("statisticsby", string.IsNullOrEmpty(statisticsby) ? string.Empty : statisticsby);
			slParams.Add("date_start", start);
			slParams.Add("date_end", end);
			slParams.Add("username", string.IsNullOrEmpty(username) ? string.Empty : username);
			slParams.Add("secretkey", api_SecretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		/// <summary>
		/// 为获取系统状态检查签名
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="sn">sn</param>
		/// <returns>签名正确:true/签名错误:false</returns>
		private bool checkSignForGetSystemStatus(string api_account, long timeStamp, string sign, string token, string sn, string secretKey)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("sn", sn);
			slParams.Add("Token", token);
			slParams.Add("secretkey", secretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		private bool checkSignForGetEnergySummary(string api_account, long timeStamp, string sign, string sn, string theDate, string token, string secretKey)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("Sn", sn);
			slParams.Add("Token", token);
			slParams.Add("TheDate", theDate);
			slParams.Add("secretkey", secretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		private bool checkSignForRunningData(string api_Account, long timeStamp, string sign, string sn, string token, string secretKey)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_Account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("Sn", sn);
			slParams.Add("Token", token);
			slParams.Add("secretkey", secretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		private bool checkSignForLastPowerData(string api_Account, long timeStamp, string sign, string sn, string token, string secretKey)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_Account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("Sn", sn);
			slParams.Add("Token", token);
			slParams.Add("secretkey", secretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}
		private bool checkSignForRemoteDispatch(string api_Account, long timeStamp, string sign, string token, string sn, int activePower, int reactivePower, decimal soc, int status, int controlMode, string secretKey)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_Account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("Sn", sn);
			slParams.Add("Token", token);
			slParams.Add("ActivePower", activePower);
			slParams.Add("ReactivePower", reactivePower);
			slParams.Add("SOC", soc);
			slParams.Add("Status", status);
			slParams.Add("ControlMode", controlMode);
			slParams.Add("secretkey", secretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		private bool checkSignForGetSystemDetail(string api_account, long timeStamp, string sign, string sn, string token, string secretKey)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("sn", sn);
			slParams.Add("Token", token);
			slParams.Add("secretkey", secretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		private bool checkSignForUpdateSystem(string api_account, long timeStamp, string sign, string token, string secretKey, VT_SYSTEM s, Dictionary<string, string> editProperties)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("sys_sn", string.IsNullOrEmpty(s.SysSn) ? string.Empty : s.SysSn);
			slParams.Add("Token", token);

			foreach (var itm in editProperties)
				slParams.Add(itm.Key, itm.Value);
			slParams.Add("secretkey", secretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}
			//_syslogRepository.Add(new SYS_LOG { Key = Guid.NewGuid(), MESSAGE = sbParams.ToString(), LOGGER = "webapi", CREATE_DATETIME = DateTime.Now });
			//_syslogRepository.Save();
			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		private bool checkSignForBingNewSystem(string api_account, long timeStamp, string sign, string token, string api_SecretKey, string sn, string licNo, string username, string checkcode)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("newSN", sn);
			slParams.Add("checkCode", checkcode);
			slParams.Add("username", username);
			slParams.Add("license_no", licNo);
			slParams.Add("Token", token);
			slParams.Add("secretkey", api_SecretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		private bool checkSignForInstallNewSystem(string api_account, long timeStamp, string sign, string token, string api_SecretKey, string sn, string licNo, DateTime installationDate, string checkcode, string customerName, string contactNumber, string contactAddress)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("newSN", sn);
			slParams.Add("checkCode", checkcode);
			slParams.Add("installationDate", installationDate.ToString("yyyy-MM-dd"));
			slParams.Add("license_no", licNo);
			slParams.Add("customerName", customerName);
			slParams.Add("contactNumber", contactNumber);
			slParams.Add("contactAddress", contactAddress);
			slParams.Add("Token", token);
			slParams.Add("secretkey", api_SecretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		private bool checkSignForGetFirmwareUpdate(string api_account, long timeStamp, string sign, string token, string api_SecretKey, string sn)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("Sn", sn);
			slParams.Add("Token", token);
			slParams.Add("secretkey", api_SecretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		private bool checkSignForUpdateSystemFirmware(string api_account, long timeStamp, string sign, string token, string sn, string category, string api_SecretKey)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("Sn", sn);
			slParams.Add("category", category);
			slParams.Add("Token", token);
			slParams.Add("secretkey", api_SecretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}
		private bool checkSignForGetSystemSummaryStatisticsData(string api_account, long timeStamp, string sign, string token, string api_SecretKey)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("Token", token);
			slParams.Add("secretkey", api_SecretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}
		private List<Report_Power> ReportPowerDataFiveMinuteIntervals(List<Report_Power> rpData)
		{
			List<Report_Power> result = null;

			if (rpData != null && rpData.Any())
			{
				result = new List<Report_Power>();
				var first = rpData[0];
				result.Add(first);

				var prevTimeline = first.TimelineIndex;
				int currentTimeline;
				for (var i = 1; i < rpData.Count; i++)
				{
					currentTimeline = rpData[i].TimelineIndex;
					if ((currentTimeline - prevTimeline) * 60 > 290)
					{
						result.Add(rpData[i]);
						prevTimeline = currentTimeline;
					}
				}
			}

			return result;
		}

		private IEnumerable<SYS_ROLE> getUserRoles(Guid userKey)
		{
			var roleUsers = _roleUserRepository.FindBy(x => x.USERID == userKey).ToList();

			if (roleUsers != null && roleUsers.Count > 0)
			{
				var roleKeys = roleUsers.Select(x => x.ROLEID).ToArray();
				return _roleRepository.FindBy(x => roleKeys.Contains(x.Key));
			}

			return Enumerable.Empty<SYS_ROLE>();
		}
		private bool CheckTodayIsWithinRange(DateTime? lastUploadTimeServer, DateTime? lastUploadTimeLocal, DateTime begin, DateTime end, out DateTime localNow)
		{
			var result = false;
			localNow = DateTime.Now;
			if (lastUploadTimeLocal.HasValue && lastUploadTimeServer.HasValue)
			{
				localNow = DateTime.Now - (lastUploadTimeServer.Value - lastUploadTimeLocal.Value);
				result = localNow >= begin && localNow <= end;
			}

			return result;
		}

		private string GetVtSystemEditPropertiesLog(VT_SYSTEM orgSystem, VT_SYSTEM currentSystem)
		{
			var result = new StringBuilder();

			if (orgSystem.LicNo != currentSystem.LicNo)
				result.AppendFormat("LicNo: {0}->{1}; ", orgSystem.LicNo, currentSystem.LicNo);
			if (orgSystem.Address != currentSystem.Address)
				result.AppendFormat("Address: {0}->{1}; ", orgSystem.Address, currentSystem.Address);
			if (orgSystem.PostCode != currentSystem.PostCode)
				result.AppendFormat("PostCode: {0}->{1}; ", orgSystem.PostCode, currentSystem.PostCode);
			if (orgSystem.Setemail != currentSystem.Setemail)
				result.AppendFormat("Setemail: {0}->{1}; ", orgSystem.Setemail, currentSystem.Setemail);
			if (orgSystem.Linkman != currentSystem.Linkman)
				result.AppendFormat("Linkman: {0}->{1}; ", orgSystem.Linkman, currentSystem.Linkman);
			if (orgSystem.MoneyType != currentSystem.MoneyType)
				result.AppendFormat("MoneyType: {0}->{1}; ", orgSystem.MoneyType, currentSystem.MoneyType);
			if (orgSystem.CellPhone != currentSystem.CellPhone)
				result.AppendFormat("CellPhone: {0}->{1}; ", orgSystem.CellPhone, currentSystem.CellPhone);
			if (orgSystem.CountryCode != currentSystem.CountryCode)
				result.AppendFormat("CountryCode: {0}->{1}; ", orgSystem.CountryCode, currentSystem.CountryCode);
			if (orgSystem.StateCode != currentSystem.StateCode)
				result.AppendFormat("StateCode: {0}->{1}; ", orgSystem.StateCode, currentSystem.StateCode);
			if (orgSystem.CityCode != currentSystem.CityCode)
				result.AppendFormat("CityCode: {0}->{1}; ", orgSystem.CityCode, currentSystem.CityCode);
			if (orgSystem.Saleprice0 != currentSystem.Saleprice0)
				result.AppendFormat("Saleprice0: {0}->{1}; ", orgSystem.Saleprice0, currentSystem.Saleprice0);
			if (orgSystem.Saleprice1 != currentSystem.Saleprice1)
				result.AppendFormat("Saleprice1: {0}->{1}; ", orgSystem.Saleprice1, currentSystem.Saleprice1);
			if (orgSystem.Saleprice2 != currentSystem.Saleprice2)
				result.AppendFormat("Saleprice2: {0}->{1}; ", orgSystem.Saleprice2, currentSystem.Saleprice2);
			if (orgSystem.Saleprice3 != currentSystem.Saleprice3)
				result.AppendFormat("Saleprice3: {0}->{1}; ", orgSystem.Saleprice3, currentSystem.Saleprice3);
			if (orgSystem.Saleprice4 != currentSystem.Saleprice4)
				result.AppendFormat("Saleprice4: {0}->{1}; ", orgSystem.Saleprice4, currentSystem.Saleprice4);
			if (orgSystem.Saleprice5 != currentSystem.Saleprice5)
				result.AppendFormat("Saleprice5: {0}->{1}; ", orgSystem.Saleprice5, currentSystem.Saleprice5);
			if (orgSystem.Saleprice6 != currentSystem.Saleprice6)
				result.AppendFormat("Saleprice6: {0}->{1}; ", orgSystem.Saleprice6, currentSystem.Saleprice6);
			if (orgSystem.Saleprice7 != currentSystem.Saleprice7)
				result.AppendFormat("Saleprice7: {0}->{1}; ", orgSystem.Saleprice7, currentSystem.Saleprice7);
			if (orgSystem.SaletimeS0 != currentSystem.SaletimeS0)
				result.AppendFormat("SaletimeS0: {0}->{1}; ", orgSystem.SaletimeS0, currentSystem.SaletimeS0);
			if (orgSystem.SaletimeS1 != currentSystem.SaletimeS1)
				result.AppendFormat("SaletimeS1: {0}->{1}; ", orgSystem.SaletimeS1, currentSystem.SaletimeS1);
			if (orgSystem.SaletimeS2 != currentSystem.SaletimeS2)
				result.AppendFormat("SaletimeS2: {0}->{1}; ", orgSystem.SaletimeS2, currentSystem.SaletimeS2);
			if (orgSystem.SaletimeS3 != currentSystem.SaletimeS3)
				result.AppendFormat("SaletimeS3: {0}->{1}; ", orgSystem.SaletimeS3, currentSystem.SaletimeS3);
			if (orgSystem.SaletimeS4 != currentSystem.SaletimeS4)
				result.AppendFormat("SaletimeS4: {0}->{1}; ", orgSystem.SaletimeS4, currentSystem.SaletimeS4);
			if (orgSystem.SaletimeS5 != currentSystem.SaletimeS5)
				result.AppendFormat("SaletimeS5: {0}->{1}; ", orgSystem.SaletimeS5, currentSystem.SaletimeS5);
			if (orgSystem.SaletimeS6 != currentSystem.SaletimeS6)
				result.AppendFormat("SaletimeS6: {0}->{1}; ", orgSystem.SaletimeS6, currentSystem.SaletimeS6);
			if (orgSystem.SaletimeS7 != currentSystem.SaletimeS7)
				result.AppendFormat("SaletimeS7: {0}->{1}; ", orgSystem.SaletimeS7, currentSystem.SaletimeS7);
			if (orgSystem.SaletimeE0 != currentSystem.SaletimeE0)
				result.AppendFormat("SaletimeE0: {0}->{1}; ", orgSystem.SaletimeE0, currentSystem.SaletimeE0);
			if (orgSystem.SaletimeE1 != currentSystem.SaletimeE1)
				result.AppendFormat("SaletimeE1: {0}->{1}; ", orgSystem.SaletimeE1, currentSystem.SaletimeE1);
			if (orgSystem.SaletimeE2 != currentSystem.SaletimeE2)
				result.AppendFormat("SaletimeE2: {0}->{1}; ", orgSystem.SaletimeE2, currentSystem.SaletimeE2);
			if (orgSystem.SaletimeE3 != currentSystem.SaletimeE3)
				result.AppendFormat("SaletimeE3: {0}->{1}; ", orgSystem.SaletimeE3, currentSystem.SaletimeE3);
			if (orgSystem.SaletimeE4 != currentSystem.SaletimeE4)
				result.AppendFormat("SaletimeE4: {0}->{1}; ", orgSystem.SaletimeE4, currentSystem.SaletimeE4);
			if (orgSystem.SaletimeE5 != currentSystem.SaletimeE5)
				result.AppendFormat("SaletimeE5: {0}->{1}; ", orgSystem.SaletimeE5, currentSystem.SaletimeE5);
			if (orgSystem.SaletimeE6 != currentSystem.SaletimeE6)
				result.AppendFormat("SaletimeE6: {0}->{1}; ", orgSystem.SaletimeE6, currentSystem.SaletimeE6);
			if (orgSystem.SaletimeE7 != currentSystem.SaletimeE7)
				result.AppendFormat("SaletimeE7: {0}->{1}; ", orgSystem.SaletimeE7, currentSystem.SaletimeE7);
			if (orgSystem.Gridcharge != currentSystem.Gridcharge)
				result.AppendFormat("Gridcharge: {0}->{1}; ", orgSystem.Gridcharge, currentSystem.Gridcharge);
			if (orgSystem.Timechaf1 != currentSystem.Timechaf1)
				result.AppendFormat("Timechaf1: {0}->{1}; ", orgSystem.Timechaf1, currentSystem.Timechaf1);
			if (orgSystem.Timechae1 != currentSystem.Timechae1)
				result.AppendFormat("Timechae1: {0}->{1}; ", orgSystem.Timechae1, currentSystem.Timechae1);
			if (orgSystem.Timechaf2 != currentSystem.Timechaf2)
				result.AppendFormat("Timechaf2: {0}->{1}; ", orgSystem.Timechaf2, currentSystem.Timechaf2);
			if (orgSystem.Timechae2 != currentSystem.Timechae2)
				result.AppendFormat("Timechae2: {0}->{1}; ", orgSystem.Timechae2, currentSystem.Timechae2);
			if (orgSystem.Bathighcap != currentSystem.Bathighcap)
				result.AppendFormat("Bathighcap: {0}->{1}; ", orgSystem.Bathighcap, currentSystem.Bathighcap);
			if (orgSystem.CtrDis != currentSystem.CtrDis)
				result.AppendFormat("CtrDis: {0}->{1}; ", orgSystem.CtrDis, currentSystem.CtrDis);
			if (orgSystem.Timedisf1 != currentSystem.Timedisf1)
				result.AppendFormat("Timedisf1: {0}->{1}; ", orgSystem.Timedisf1, currentSystem.Timedisf1);
			if (orgSystem.Timedise1 != currentSystem.Timedise1)
				result.AppendFormat("Timedise1: {0}->{1}; ", orgSystem.Timedise1, currentSystem.Timedise1);
			if (orgSystem.Timedisf2 != currentSystem.Timedisf2)
				result.AppendFormat("Timedisf2: {0}->{1}; ", orgSystem.Timedisf2, currentSystem.Timedisf2);
			if (orgSystem.Timedise2 != currentSystem.Timedise2)
				result.AppendFormat("Timedise2: {0}->{1}; ", orgSystem.Timedise2, currentSystem.Timedise2);
			if (orgSystem.Batusecap != currentSystem.Batusecap)
				result.AppendFormat("Batusecap: {0}->{1}; ", orgSystem.Batusecap, currentSystem.Batusecap);
			if (orgSystem.Generator != currentSystem.Generator)
				result.AppendFormat("Generator: {0}->{1}; ", orgSystem.Generator, currentSystem.Generator);
			if (orgSystem.GeneratorMode != currentSystem.GeneratorMode)
				result.AppendFormat("GeneratorMode: {0}->{1}; ", orgSystem.GeneratorMode, currentSystem.GeneratorMode);
			if (orgSystem.GCSOCStart != currentSystem.GCSOCStart)
				result.AppendFormat("GCSOCStart: {0}->{1}; ", orgSystem.GCSOCStart, currentSystem.GCSOCStart);
			if (orgSystem.GCSOCEnd != currentSystem.GCSOCEnd)
				result.AppendFormat("GCSOCEnd: {0}->{1}; ", orgSystem.GCSOCEnd, currentSystem.GCSOCEnd);
			if (orgSystem.GCTimeStart != currentSystem.GCTimeStart)
				result.AppendFormat("GCTimeStart: {0}->{1}; ", orgSystem.GCTimeStart, currentSystem.GCTimeStart);
			if (orgSystem.GCTimeEnd != currentSystem.GCTimeEnd)
				result.AppendFormat("GCTimeEnd: {0}->{1}; ", orgSystem.GCTimeEnd, currentSystem.GCTimeEnd);
			if (orgSystem.GCChargePower != currentSystem.GCChargePower)
				result.AppendFormat("GCChargePower: {0}->{1}; ", orgSystem.GCChargePower, currentSystem.GCChargePower);
			if (orgSystem.GCRatedPower != currentSystem.GCRatedPower)
				result.AppendFormat("GCRatedPower: {0}->{1}; ", orgSystem.GCRatedPower, currentSystem.GCRatedPower);
			if (orgSystem.GCOutputMode != currentSystem.GCOutputMode)
				result.AppendFormat("GCOutputMode: {0}->{1}; ", orgSystem.GCOutputMode, currentSystem.GCOutputMode);
			if (orgSystem.Workmode != currentSystem.Workmode)
				result.AppendFormat("Workmode: {0}->{1}; ", orgSystem.Workmode, currentSystem.Workmode);
			if (orgSystem.AllowAutoUpdate != currentSystem.AllowAutoUpdate)
				result.AppendFormat("AllowAutoUpdate: {0}->{1}; ", orgSystem.AllowAutoUpdate, currentSystem.AllowAutoUpdate);
			if (orgSystem.Settime != currentSystem.Settime)
				result.AppendFormat("Settime: {0}->{1}; ", orgSystem.Settime, currentSystem.Settime);
			if (orgSystem.Acdc != currentSystem.Acdc)
				result.AppendFormat("Acdc: {0}->{1}; ", orgSystem.Acdc, currentSystem.Acdc);
			if (orgSystem.SetFeed != currentSystem.SetFeed)
				result.AppendFormat("SetFeed: {0}->{1}; ", orgSystem.SetFeed, currentSystem.SetFeed);
			if (orgSystem.SetPhase != currentSystem.SetPhase)
				result.AppendFormat("SetPhase: {0}->{1}; ", orgSystem.SetPhase, currentSystem.SetPhase);
			if (orgSystem.Setmode != currentSystem.Setmode)
				result.AppendFormat("Setmode: {0}->{1}; ", orgSystem.Setmode, currentSystem.Setmode);
			if (orgSystem.PowerSource != currentSystem.PowerSource)
				result.AppendFormat("PowerSource: {0}->{1}; ", orgSystem.PowerSource, currentSystem.PowerSource);
			if (orgSystem.BackUpBox != currentSystem.BackUpBox)
				result.AppendFormat("BackUpBox: {0}->{1}; ", orgSystem.BackUpBox, currentSystem.BackUpBox);
			if (orgSystem.L1SocLimit != currentSystem.L1SocLimit)
				result.AppendFormat("L1SocLimit: {0}->{1}; ", orgSystem.L1SocLimit, currentSystem.L1SocLimit);
			if (orgSystem.L2SocLimit != currentSystem.L2SocLimit)
				result.AppendFormat("L2SocLimit: {0}->{1}; ", orgSystem.L2SocLimit, currentSystem.L2SocLimit);
			if (orgSystem.L3SocLimit != currentSystem.L3SocLimit)
				result.AppendFormat("L3SocLimit: {0}->{1}; ", orgSystem.L3SocLimit, currentSystem.L3SocLimit);
			if (orgSystem.L1Priority != currentSystem.L1Priority)
				result.AppendFormat("L1Priority: {0}->{1}; ", orgSystem.L1Priority, currentSystem.L1Priority);
			if (orgSystem.L2Priority != currentSystem.L2Priority)
				result.AppendFormat("L2Priority: {0}->{1}; ", orgSystem.L2Priority, currentSystem.L2Priority);
			if (orgSystem.L3Priority != currentSystem.L3Priority)
				result.AppendFormat("L3Priority: {0}->{1}; ", orgSystem.L3Priority, currentSystem.L3Priority);
			if (orgSystem.EmsLanguage != currentSystem.EmsLanguage)
				result.AppendFormat("EmsLanguage: {0}->{1}; ", orgSystem.EmsLanguage, currentSystem.EmsLanguage);
			if (orgSystem.SysTimezone != currentSystem.SysTimezone)
				result.AppendFormat("SysTimezone: {0}->{1}; ", orgSystem.SysTimezone, currentSystem.SysTimezone);
			if (orgSystem.GridType != currentSystem.GridType)
				result.AppendFormat("GridType: {0}->{1}; ", orgSystem.GridType, currentSystem.GridType);
			if (orgSystem.FirmwareVersion != currentSystem.FirmwareVersion)
				result.AppendFormat("FirmwareVersion: {0}->{1}; ", orgSystem.FirmwareVersion, currentSystem.FirmwareVersion);
			if (orgSystem.OFSEpvTotal != currentSystem.OFSEpvTotal)
				result.AppendFormat("OFSEpvTotal: {0}->{1}; ", orgSystem.OFSEpvTotal, currentSystem.OFSEpvTotal);
			if (orgSystem.OFSEinput != currentSystem.OFSEinput)
				result.AppendFormat("OFSEinput: {0}->{1}; ", orgSystem.OFSEinput, currentSystem.OFSEinput);
			if (orgSystem.OFSEoutput != currentSystem.OFSEoutput)
				result.AppendFormat("OFSEoutput: {0}->{1}; ", orgSystem.OFSEoutput, currentSystem.OFSEoutput);
			if (orgSystem.OFSEcharge != currentSystem.OFSEcharge)
				result.AppendFormat("OFSEcharge: {0}->{1}; ", orgSystem.OFSEcharge, currentSystem.OFSEcharge);
			if (orgSystem.OFSEGridCharge != currentSystem.OFSEGridCharge)
				result.AppendFormat("OFSEGridCharge: {0}->{1}; ", orgSystem.OFSEGridCharge, currentSystem.OFSEGridCharge);
			if (orgSystem.OFSEdischarge != currentSystem.OFSEdischarge)
				result.AppendFormat("OFSEdischarge: {0}->{1}; ", orgSystem.OFSEdischarge, currentSystem.OFSEdischarge);
			if (orgSystem.OnGridCap != currentSystem.OnGridCap)
				result.AppendFormat("OnGridCap: {0}->{1}; ", orgSystem.OnGridCap, currentSystem.OnGridCap);
			if (orgSystem.StorageCap != currentSystem.StorageCap)
				result.AppendFormat("StorageCap: {0}->{1}; ", orgSystem.StorageCap, currentSystem.StorageCap);
			if (orgSystem.BatReady != currentSystem.BatReady)
				result.AppendFormat("BatReady: {0}->{1}; ", orgSystem.BatReady, currentSystem.BatReady);
			if (orgSystem.MeterDCNegate != currentSystem.MeterDCNegate)
				result.AppendFormat("MeterDCNegate: {0}->{1}; ", orgSystem.MeterDCNegate, currentSystem.MeterDCNegate);
			if (orgSystem.MeterACNegate != currentSystem.MeterACNegate)
				result.AppendFormat("MerterACNegate: {0}->{1}; ", orgSystem.MeterACNegate, currentSystem.MeterACNegate);
			if (orgSystem.Safe != currentSystem.Safe)
				result.AppendFormat("Safe: {0}->{1}; ", orgSystem.Safe, currentSystem.Safe);
			if (orgSystem.PowerFact != currentSystem.PowerFact)
				result.AppendFormat("PowerFact: {0}->{1}; ", orgSystem.PowerFact, currentSystem.PowerFact);
			if (orgSystem.Volt5MinAvg != currentSystem.Volt5MinAvg)
				result.AppendFormat("Volt5MinAvg: {0}->{1}; ", orgSystem.Volt5MinAvg, currentSystem.Volt5MinAvg);
			if (orgSystem.Volt10MinAvg != currentSystem.Volt10MinAvg)
				result.AppendFormat("Volt10MinAvg: {0}->{1}; ", orgSystem.Volt10MinAvg, currentSystem.Volt10MinAvg);
			if (orgSystem.TempThreshold != currentSystem.TempThreshold)
				result.AppendFormat("TempThreshold: {0}->{1}; ", orgSystem.TempThreshold, currentSystem.TempThreshold);
			if (orgSystem.OutCurProtect != currentSystem.OutCurProtect)
				result.AppendFormat("OutCurProtect: {0}->{1}; ", orgSystem.OutCurProtect, currentSystem.OutCurProtect);
			if (orgSystem.DCI != currentSystem.DCI)
				result.AppendFormat("DCI: {0}->{1}; ", orgSystem.DCI, currentSystem.DCI);
			if (orgSystem.RCD != currentSystem.RCD)
				result.AppendFormat("RCD: {0}->{1}; ", orgSystem.RCD, currentSystem.RCD);
			if (orgSystem.PvISO != currentSystem.PvISO)
				result.AppendFormat("PvISO: {0}->{1}; ", orgSystem.PvISO, currentSystem.PvISO);
			if (orgSystem.ChargeBoostCur != currentSystem.ChargeBoostCur)
				result.AppendFormat("ChargeBoostCur: {0}->{1}; ", orgSystem.ChargeBoostCur, currentSystem.ChargeBoostCur);
			if (orgSystem.Channel1 != currentSystem.Channel1)
				result.AppendFormat("Channel1: {0}->{1}; ", orgSystem.Channel1, currentSystem.Channel1);
			if (orgSystem.ControlMode1 != currentSystem.ControlMode1)
				result.AppendFormat("ControlMode1: {0}->{1}; ", orgSystem.ControlMode1, currentSystem.ControlMode1);
			if (orgSystem.StartTime1A != currentSystem.StartTime1A)
				result.AppendFormat("StartTime1A: {0}->{1}; ", orgSystem.StartTime1A, currentSystem.StartTime1A);
			if (orgSystem.EndTime1A != currentSystem.EndTime1A)
				result.AppendFormat("EndTime1A: {0}->{1}; ", orgSystem.EndTime1A, currentSystem.EndTime1A);
			if (orgSystem.StartTime1B != currentSystem.StartTime1B)
				result.AppendFormat("StartTime1B: {0}->{1}; ", orgSystem.StartTime1B, currentSystem.StartTime1B);
			if (orgSystem.EndTime1B != currentSystem.EndTime1B)
				result.AppendFormat("EndTime1B: {0}->{1}; ", orgSystem.EndTime1B, currentSystem.EndTime1B);
			if (orgSystem.Date1 != currentSystem.Date1)
				result.AppendFormat("Date1: {0}->{1}; ", orgSystem.Date1, currentSystem.Date1);
			if (orgSystem.ChargeSOC1 != currentSystem.ChargeSOC1)
				result.AppendFormat("ChargeSOC1: {0}->{1}; ", orgSystem.ChargeSOC1, currentSystem.ChargeSOC1);
			if (orgSystem.UPS1 != currentSystem.UPS1)
				result.AppendFormat("UPS1: {0}->{1}; ", orgSystem.UPS1, currentSystem.UPS1);
			if (orgSystem.SwitchOn1 != currentSystem.SwitchOn1)
				result.AppendFormat("SwitchOn1: {0}->{1}; ", orgSystem.SwitchOn1, currentSystem.SwitchOn1);
			if (orgSystem.SwitchOff1 != currentSystem.SwitchOff1)
				result.AppendFormat("SwitchOff1: {0}->{1}; ", orgSystem.SwitchOff1, currentSystem.SwitchOff1);
			if (orgSystem.Delay1 != currentSystem.Delay1)
				result.AppendFormat("Delay1: {0}->{1}; ", orgSystem.Delay1, currentSystem.Delay1);
			if (orgSystem.Duration1 != currentSystem.Duration1)
				result.AppendFormat("Duration1: {0}->{1}; ", orgSystem.Duration1, currentSystem.Duration1);
			if (orgSystem.Pause1 != currentSystem.Pause1)
				result.AppendFormat("Pause1: {0}->{1}; ", orgSystem.Pause1, currentSystem.Pause1);
			if (orgSystem.Channel2 != currentSystem.Channel2)
				result.AppendFormat("Channel2: {0}->{1}; ", orgSystem.Channel2, currentSystem.Channel2);
			if (orgSystem.ControlMode2 != currentSystem.ControlMode2)
				result.AppendFormat("ControlMode2: {0}->{1}; ", orgSystem.ControlMode2, currentSystem.ControlMode2);
			if (orgSystem.StartTime2A != currentSystem.StartTime2A)
				result.AppendFormat("StartTime2A: {0}->{1}; ", orgSystem.StartTime2A, currentSystem.StartTime2A);
			if (orgSystem.EndTime2A != currentSystem.EndTime2A)
				result.AppendFormat("EndTime2A: {0}->{1}; ", orgSystem.EndTime2A, currentSystem.EndTime2A);
			if (orgSystem.StartTime2B != currentSystem.StartTime2B)
				result.AppendFormat("StartTime2B: {0}->{1}; ", orgSystem.StartTime2B, currentSystem.StartTime2B);
			if (orgSystem.EndTime2B != currentSystem.EndTime2B)
				result.AppendFormat("EndTime2B: {0}->{1}; ", orgSystem.EndTime2B, currentSystem.EndTime2B);
			if (orgSystem.Date2 != currentSystem.Date2)
				result.AppendFormat("Date2: {0}->{1}; ", orgSystem.Date2, currentSystem.Date2);
			if (orgSystem.ChargeSOC2 != currentSystem.ChargeSOC2)
				result.AppendFormat("ChargeSOC2: {0}->{1}; ", orgSystem.ChargeSOC2, currentSystem.ChargeSOC2);
			if (orgSystem.UPS2 != currentSystem.UPS2)
				result.AppendFormat("UPS2: {0}->{1}; ", orgSystem.UPS2, currentSystem.UPS2);
			if (orgSystem.SwitchOn2 != currentSystem.SwitchOn2)
				result.AppendFormat("SwitchOn2: {0}->{1}; ", orgSystem.SwitchOn2, currentSystem.SwitchOn2);
			if (orgSystem.SwitchOff2 != currentSystem.SwitchOff2)
				result.AppendFormat("SwitchOff2: {0}->{1}; ", orgSystem.SwitchOff2, currentSystem.SwitchOff2);
			if (orgSystem.Delay2 != currentSystem.Delay2)
				result.AppendFormat("Delay2: {0}->{1}; ", orgSystem.Delay2, currentSystem.Delay2);
			if (orgSystem.Duration2 != currentSystem.Duration2)
				result.AppendFormat("Duration2: {0}->{1}; ", orgSystem.Duration2, currentSystem.Duration2);
			if (orgSystem.Pause2 != currentSystem.Pause2)
				result.AppendFormat("Pause2: {0}->{1}; ", orgSystem.Pause2, currentSystem.Pause2);
			if (orgSystem.AllowSyncTime != currentSystem.AllowSyncTime)
				result.AppendFormat("AllowSyncTime: {0}->{1}; ", orgSystem.AllowSyncTime, currentSystem.AllowSyncTime);
			if (orgSystem.BakBoxSN != currentSystem.BakBoxSN)
				result.AppendFormat("BakBoxSN: {0}->{1}; ", orgSystem.BakBoxSN, currentSystem.BakBoxSN);
			if (orgSystem.BakBoxVer != currentSystem.BakBoxVer)
				result.AppendFormat("BakBoxVer: {0}->{1}; ", orgSystem.BakBoxVer, currentSystem.BakBoxVer);
			if (orgSystem.GridChargeWE != currentSystem.GridChargeWE)
				result.AppendFormat("GridChargeWE: {0}->{1}; ", orgSystem.GridChargeWE, currentSystem.GridChargeWE);
			if (orgSystem.TimeChaFWE1 != currentSystem.TimeChaFWE1)
				result.AppendFormat("TimeChaFWE1: {0}->{1}; ", orgSystem.TimeChaFWE1, currentSystem.TimeChaFWE1);
			if (orgSystem.TimeChaEWE1 != currentSystem.TimeChaEWE1)
				result.AppendFormat("TimeChaEWE1: {0}->{1}; ", orgSystem.TimeChaEWE1, currentSystem.TimeChaEWE1);
			if (orgSystem.TimeChaFWE2 != currentSystem.TimeChaFWE2)
				result.AppendFormat("TimeChaFWE2: {0}->{1}; ", orgSystem.TimeChaFWE2, currentSystem.TimeChaFWE2);
			if (orgSystem.TimeChaEWE2 != currentSystem.TimeChaEWE2)
				result.AppendFormat("TimeChaEWE2: {0}->{1}; ", orgSystem.TimeChaEWE2, currentSystem.TimeChaEWE2);
			if (orgSystem.TimeDisFWE1 != currentSystem.TimeDisFWE1)
				result.AppendFormat("TimeDisFWE1: {0}->{1}; ", orgSystem.TimeDisFWE1, currentSystem.TimeDisFWE1);
			if (orgSystem.TimeDisEWE1 != currentSystem.TimeDisEWE1)
				result.AppendFormat("TimeDisEWE1: {0}->{1}; ", orgSystem.TimeDisEWE1, currentSystem.TimeDisEWE1);
			if (orgSystem.TimeDisFWE2 != currentSystem.TimeDisFWE2)
				result.AppendFormat("TimeDisFWE2: {0}->{1}; ", orgSystem.TimeDisFWE2, currentSystem.TimeDisFWE2);
			if (orgSystem.TimeDisEWE2 != currentSystem.TimeDisEWE2)
				result.AppendFormat("TimeDisEWE2: {0}->{1}; ", orgSystem.TimeDisEWE2, currentSystem.TimeDisEWE2);
			if (orgSystem.BatHighCapWE != currentSystem.BatHighCapWE)
				result.AppendFormat("BatHighCapWE: {0}->{1}; ", orgSystem.BatHighCapWE, currentSystem.BatHighCapWE);
			if (orgSystem.BatUseCapWE != currentSystem.BatUseCapWE)
				result.AppendFormat("BatUseCapWE: {0}->{1}; ", orgSystem.BatUseCapWE, currentSystem.BatUseCapWE);
			if (orgSystem.CtrDisWE != currentSystem.CtrDisWE)
				result.AppendFormat("CtrDisWE: {0}->{1}; ", orgSystem.CtrDisWE, currentSystem.CtrDisWE);
			if (orgSystem.ChargeWorkDays != currentSystem.ChargeWorkDays)
				result.AppendFormat("ChargeWorkDays: {0}->{1}; ", orgSystem.ChargeWorkDays, currentSystem.ChargeWorkDays);
			if (orgSystem.ChargeWeekend != currentSystem.ChargeWeekend)
				result.AppendFormat("ChargeWeekend: {0}->{1}; ", orgSystem.ChargeWeekend, currentSystem.ChargeWeekend);
			if (orgSystem.MaxGridCharge != currentSystem.MaxGridCharge)
				result.AppendFormat("MaxGridCharge: {0}->{1}; ", orgSystem.MaxGridCharge, currentSystem.MaxGridCharge);
			if (orgSystem.TOPBMUVer != currentSystem.TOPBMUVer)
				result.AppendFormat("TOPBMUVer: {0}->{1}; ", orgSystem.TOPBMUVer, currentSystem.TOPBMUVer);
			if (orgSystem.ISOVer != currentSystem.ISOVer)
				result.AppendFormat("ISOVer: {0}->{1}; ", orgSystem.ISOVer, currentSystem.ISOVer);

			return result.ToString();
		}

		private Dictionary<string, string> GetVtSystemEditProperties(VT_SYSTEM orgSystem, VT_SYSTEM s)
		{
			Dictionary<string, string> slParams = new Dictionary<string, string>();

			if (orgSystem != null)
			{
				if (s.CompanyId.HasValue)
					slParams.Add("CompanyId", s.CompanyId.HasValue ? s.CompanyId.Value.ToString() : string.Empty);
				if (s.LicNo != null)
					slParams.Add("licno", string.IsNullOrEmpty(s.LicNo) ? string.Empty : s.LicNo);
				if (s.Address != null)
					slParams.Add("address", string.IsNullOrEmpty(s.Address) ? string.Empty : s.Address);
				if (s.PostCode != null)
					slParams.Add("postcode", string.IsNullOrEmpty(s.PostCode) ? string.Empty : s.PostCode);
				if (s.Setemail != null)
					slParams.Add("Setemail", string.IsNullOrEmpty(s.Setemail) ? string.Empty : s.Setemail);
				if (s.Linkman != null)
					slParams.Add("linkman", string.IsNullOrEmpty(s.Linkman) ? string.Empty : s.Linkman);
				if (s.MoneyType != null)
					slParams.Add("moneytype", string.IsNullOrEmpty(s.MoneyType) ? string.Empty : s.MoneyType);
				if (s.CellPhone != null)
					slParams.Add("cellphone", string.IsNullOrEmpty(s.CellPhone) ? string.Empty : s.CellPhone);
				if (s.CountryCode != null)
					slParams.Add("countrycode", string.IsNullOrEmpty(s.CountryCode) ? string.Empty : s.CountryCode);
				if (s.StateCode != null)
					slParams.Add("statecode", string.IsNullOrEmpty(s.StateCode) ? string.Empty : s.StateCode);
				if (s.CityCode != null)
					slParams.Add("citycode", string.IsNullOrEmpty(s.CityCode) ? string.Empty : s.CityCode);
				if (s.Saleprice0 != null)
					slParams.Add("Saleprice0", s.Saleprice0.HasValue ? s.Saleprice0.Value.ToString() : "0");
				if (s.Saleprice1 != null)
					slParams.Add("Saleprice1", s.Saleprice1.HasValue ? s.Saleprice1.Value.ToString() : "0");
				if (s.Saleprice2 != null)
					slParams.Add("Saleprice2", s.Saleprice2.HasValue ? s.Saleprice2.Value.ToString() : "0");
				if (s.Saleprice3 != null)
					slParams.Add("Saleprice3", s.Saleprice3.HasValue ? s.Saleprice3.Value.ToString() : "0");
				if (s.Saleprice4 != null)
					slParams.Add("Saleprice4", s.Saleprice4.HasValue ? s.Saleprice4.Value.ToString() : "0");
				if (s.Saleprice5 != null)
					slParams.Add("Saleprice5", s.Saleprice5.HasValue ? s.Saleprice5.Value.ToString() : "0");
				if (s.Saleprice6 != null)
					slParams.Add("Saleprice6", s.Saleprice6.HasValue ? s.Saleprice6.Value.ToString() : "0");
				if (s.Saleprice7 != null)
					slParams.Add("Saleprice7", s.Saleprice7.HasValue ? s.Saleprice7.Value.ToString() : "0");
				if (s.SaletimeS0 != 0)
					slParams.Add("SaletimeS0", s.SaletimeS0.ToString());
				if (s.SaletimeS1 != 0)
					slParams.Add("SaletimeS1", s.SaletimeS1.ToString());
				if (s.SaletimeS2 != 0)
					slParams.Add("SaletimeS2", s.SaletimeS2.ToString());
				if (s.SaletimeS3 != 0)
					slParams.Add("SaletimeS3", s.SaletimeS3.ToString());
				if (s.SaletimeS4 != 0)
					slParams.Add("SaletimeS4", s.SaletimeS4.ToString());
				if (s.SaletimeS5 != 0)
					slParams.Add("SaletimeS5", s.SaletimeS5.ToString());
				if (s.SaletimeS6 != 0)
					slParams.Add("SaletimeS6", s.SaletimeS6.ToString());
				if (s.SaletimeS7 != 0)
					slParams.Add("SaletimeS7", s.SaletimeS7.ToString());
				if (s.SaletimeE0 != 0)
					slParams.Add("SaletimeE0", s.SaletimeE0.ToString());
				if (s.SaletimeE1 != 0)
					slParams.Add("SaletimeE1", s.SaletimeE1.ToString());
				if (s.SaletimeE2 != 0)
					slParams.Add("SaletimeE2", s.SaletimeE2.ToString());
				if (s.SaletimeE3 != 0)
					slParams.Add("SaletimeE3", s.SaletimeE3.ToString());
				if (s.SaletimeE4 != 0)
					slParams.Add("SaletimeE4", s.SaletimeE4.ToString());
				if (s.SaletimeE5 != 0)
					slParams.Add("SaletimeE5", s.SaletimeE5.ToString());
				if (s.SaletimeE6 != 0)
					slParams.Add("SaletimeE6", s.SaletimeE6.ToString());
				if (s.SaletimeE7 != 0)
					slParams.Add("SaletimeE7", s.SaletimeE7.ToString());
				if (s.Gridcharge != null)
					slParams.Add("GridCharge", s.Gridcharge.HasValue ? s.Gridcharge.Value.ToString() : "0");
				if (s.Timechaf1 != 0)
					slParams.Add("TimeChaF1", s.Timechaf1.ToString());
				if (s.Timechae1 != 0)
					slParams.Add("TimeChaE1", s.Timechae1.ToString());
				if (s.Timechaf2 != 0)
					slParams.Add("TimeChaF2", s.Timechaf2.ToString());
				if (s.Timechae2 != 0)
					slParams.Add("TimeChaE2", s.Timechae2.ToString());
				if (s.Bathighcap != null)
					slParams.Add("BatHighCap", s.Bathighcap.HasValue ? s.Bathighcap.Value.ToString() : "0");
				if (s.CtrDis != null)
					slParams.Add("CtrDis", s.CtrDis.HasValue ? s.CtrDis.Value.ToString() : "0");
				if (s.Timedisf1 != 0)
					slParams.Add("TimeDisF1", s.Timedisf1.ToString());
				if (s.Timedise1 != 0)
					slParams.Add("TimeDisE1", s.Timedise1.ToString());
				if (s.Timedisf2 != 0)
					slParams.Add("TimeDisF2", s.Timedisf2.ToString());
				if (s.Timedise2 != 0)
					slParams.Add("TimeDisE2", s.Timedise2.ToString());
				if (s.Batusecap != null)
					slParams.Add("BatUseCap", s.Batusecap.HasValue ? s.Batusecap.Value.ToString() : "0");
				if (s.Generator != null)
					slParams.Add("Generator", s.Generator.HasValue ? s.Generator.Value.ToString() : "0");
				if (s.GeneratorMode != null)
					slParams.Add("GeneratorMode", s.GeneratorMode.HasValue ? s.GeneratorMode.Value.ToString() : "0");
				if (s.GCSOCStart != null)
					slParams.Add("GCSOCStart", s.GCSOCStart.HasValue ? s.GCSOCStart.Value.ToString() : "0");
				if (s.GCSOCEnd != null)
					slParams.Add("GCSOCEnd", s.GCSOCEnd.HasValue ? s.GCSOCEnd.Value.ToString() : "0");
				if (s.GCTimeStart != null)
					slParams.Add("GCTimeStart", s.GCTimeStart.HasValue ? s.GCTimeStart.Value.ToString() : "0");
				if (s.GCTimeEnd != null)
					slParams.Add("GCTimeEnd", s.GCTimeEnd.HasValue ? s.GCTimeEnd.Value.ToString() : "0");
				if (s.GCChargePower != null)
					slParams.Add("GCChargePower", s.GCChargePower.HasValue ? s.GCChargePower.Value.ToString() : "0");
				if (s.GCRatedPower != null)
					slParams.Add("GCRatedPower", s.GCRatedPower.HasValue ? s.GCRatedPower.Value.ToString() : "0");
				if (s.GCOutputMode != null)
					slParams.Add("GCOutputMode", s.GCOutputMode.HasValue ? s.GCOutputMode.Value.ToString() : "0");
				if (s.Workmode != null)
					slParams.Add("Workmode", s.Workmode.HasValue ? s.Workmode.Value.ToString() : "0");
				if (s.AllowAutoUpdate != null)
					slParams.Add("AllowAutoUpdate", s.AllowAutoUpdate.HasValue ? s.AllowAutoUpdate.Value.ToString() : "0");
				if (s.Settime != null)
					slParams.Add("Settime", string.IsNullOrEmpty(s.Settime) ? string.Empty : s.Settime);
				if (s.Acdc != null)
					slParams.Add("ACDC", string.IsNullOrEmpty(s.Acdc) ? string.Empty : s.Acdc);
				if (s.SetFeed != 0)
					slParams.Add("SetFeed", s.SetFeed.ToString());
				if (s.SetPhase != 0)
					slParams.Add("SetPhase", s.SetPhase.ToString());
				if (s.Setmode != 0)
					slParams.Add("Setmode", s.Setmode.ToString());
				if (s.PowerSource != null)
					slParams.Add("PowerSource", string.IsNullOrEmpty(s.PowerSource) ? string.Empty : s.PowerSource);
				if (s.BackUpBox != null)
					slParams.Add("BackUpBox", s.BackUpBox.HasValue ? s.BackUpBox.Value.ToString() : "0");
				if (s.L1SocLimit != null)
					slParams.Add("L1SocLimit", s.L1SocLimit.HasValue ? s.L1SocLimit.Value.ToString() : "0");
				if (s.L2SocLimit != null)
					slParams.Add("L2SocLimit", s.L2SocLimit.HasValue ? s.L2SocLimit.Value.ToString() : "0");
				if (s.L3SocLimit != null)
					slParams.Add("L3SocLimit", s.L3SocLimit.HasValue ? s.L3SocLimit.Value.ToString() : "0");
				if (s.L1Priority != null)
					slParams.Add("L1Priority", s.L1Priority.HasValue ? s.L1Priority.Value.ToString() : "0");
				if (s.L2Priority != null)
					slParams.Add("L2Priority", s.L2Priority.HasValue ? s.L2Priority.ToString() : "0");
				if (s.L3Priority != null)
					slParams.Add("L3Priority", s.L3Priority.HasValue ? s.L3Priority.Value.ToString() : "0");
				if (s.EmsLanguage != null)
					slParams.Add("EmsLanguage", string.IsNullOrEmpty(s.EmsLanguage) ? string.Empty : s.EmsLanguage);
				if (s.SysTimezone != null)
					slParams.Add("SysTimezone", string.IsNullOrEmpty(s.SysTimezone) ? string.Empty : s.SysTimezone);
				if (s.GridType != null)
					slParams.Add("GridType", s.GridType.HasValue ? s.GridType.Value.ToString() : "0");
				if (s.FirmwareVersion != null)
					slParams.Add("FirmwareVersion", string.IsNullOrEmpty(s.FirmwareVersion) ? string.Empty : s.FirmwareVersion);
				if (s.OFSEpvTotal != null)
					slParams.Add("OFS_EpvTotal", s.OFSEpvTotal.HasValue ? s.OFSEpvTotal.Value.ToString() : "0");
				if (s.OFSEinput != null)
					slParams.Add("OFS_Einput", s.OFSEinput.HasValue ? s.OFSEinput.Value.ToString() : "0");
				if (s.OFSEoutput != null)
					slParams.Add("OFS_Eoutput", s.OFSEoutput.HasValue ? s.OFSEoutput.Value.ToString() : "0");
				if (s.OFSEcharge != null)
					slParams.Add("OFS_Echarge", s.OFSEcharge.HasValue ? s.OFSEcharge.Value.ToString() : "0");
				if (s.OFSEGridCharge != null)
					slParams.Add("OFS_EGridCharge", s.OFSEGridCharge.HasValue ? s.OFSEGridCharge.Value.ToString() : "0");
				if (s.OFSEdischarge != null)
					slParams.Add("OFS_Edischarge", s.OFSEdischarge.HasValue ? s.OFSEdischarge.Value.ToString() : "0");
				if (s.OnGridCap != null)
					slParams.Add("OnGridCap", s.OnGridCap.HasValue ? s.OnGridCap.Value.ToString() : "0");
				if (s.StorageCap != null)
					slParams.Add("StorageCap", s.StorageCap.HasValue ? s.StorageCap.Value.ToString() : "0");
				if (s.BatReady != null)
					slParams.Add("BatReady", string.IsNullOrEmpty(s.BatReady) ? string.Empty : s.BatReady);
				if (s.MeterDCNegate != null)
					slParams.Add("MeterDCNegate", string.IsNullOrEmpty(s.MeterDCNegate) ? string.Empty : s.MeterDCNegate);
				if (s.MeterACNegate != null)
					slParams.Add("MerterACNegate", string.IsNullOrEmpty(s.MeterACNegate) ? string.Empty : s.MeterACNegate);
				if (s.Safe != null)
					slParams.Add("Safe", s.Safe.HasValue ? s.Safe.Value.ToString() : "0");
				if (s.PowerFact != null)
					slParams.Add("PowerFact", s.PowerFact.HasValue ? s.PowerFact.Value.ToString() : "0");
				if (s.Volt5MinAvg != null)
					slParams.Add("Volt5MinAvg", s.Volt5MinAvg.HasValue ? s.Volt5MinAvg.Value.ToString() : "0");
				if (s.Volt10MinAvg != null)
					slParams.Add("Volt10MinAvg", s.Volt10MinAvg.HasValue ? s.Volt10MinAvg.Value.ToString() : "0");
				if (s.TempThreshold != null)
					slParams.Add("TempThreshold", s.TempThreshold.HasValue ? s.TempThreshold.Value.ToString() : "0");
				if (s.OutCurProtect != null)
					slParams.Add("OutCurProtect", s.OutCurProtect.ToString());
				if (s.DCI != null)
					slParams.Add("DCI", s.DCI.HasValue ? s.DCI.Value.ToString() : "0");
				if (s.RCD != null)
					slParams.Add("RCD", s.RCD.HasValue ? s.RCD.Value.ToString() : "0");
				if (s.PvISO != null)
					slParams.Add("PvISO", s.PvISO.HasValue ? s.PvISO.Value.ToString() : "0");
				if (s.ChargeBoostCur != null)
					slParams.Add("ChargeBoostCur", s.ChargeBoostCur.HasValue ? s.ChargeBoostCur.Value.ToString() : "0");
				if (s.Channel1 != null)
					slParams.Add("Channel1", string.IsNullOrEmpty(s.Channel1) ? string.Empty : s.Channel1);
				if (s.ControlMode1 != null)
					slParams.Add("ControlMode1", string.IsNullOrEmpty(s.ControlMode1) ? string.Empty : s.ControlMode1);
				if (s.StartTime1A != null)
					slParams.Add("StartTime1A", string.IsNullOrEmpty(s.StartTime1A) ? string.Empty : s.StartTime1A);
				if (s.EndTime1A != null)
					slParams.Add("EndTime1A", string.IsNullOrEmpty(s.EndTime1A) ? string.Empty : s.EndTime1A);
				if (s.StartTime1B != null)
					slParams.Add("StartTime1B", string.IsNullOrEmpty(s.StartTime1B) ? string.Empty : s.StartTime1B);
				if (s.EndTime1B != null)
					slParams.Add("EndTime1B", string.IsNullOrEmpty(s.EndTime1B) ? string.Empty : s.EndTime1B);
				if (s.Date1 != null)
					slParams.Add("Date1", string.IsNullOrEmpty(s.Date1) ? string.Empty : s.Date1);
				if (s.ChargeSOC1 != null)
					slParams.Add("ChargeSOC1", string.IsNullOrEmpty(s.ChargeSOC1) ? string.Empty : s.ChargeSOC1);
				if (s.UPS1 != null)
					slParams.Add("UPS1", string.IsNullOrEmpty(s.UPS1) ? string.Empty : s.UPS1);
				if (s.SwitchOn1 != null)
					slParams.Add("SwitchOn1", s.SwitchOn1.HasValue ? s.SwitchOn1.Value.ToString() : "0");
				if (s.SwitchOff1 != null)
					slParams.Add("SwitchOff1", s.SwitchOff1.HasValue ? s.SwitchOff1.Value.ToString() : "0");
				if (s.Delay1 != null)
					slParams.Add("Delay1", string.IsNullOrEmpty(s.Delay1) ? string.Empty : s.Delay1);
				if (s.Duration1 != null)
					slParams.Add("Duration1", string.IsNullOrEmpty(s.Duration1) ? string.Empty : s.Duration1);
				if (s.Pause1 != null)
					slParams.Add("Pause1", string.IsNullOrEmpty(s.Pause1) ? string.Empty : s.Pause1);
				if (s.Channel2 != null)
					slParams.Add("Channel2", string.IsNullOrEmpty(s.Channel2) ? string.Empty : s.Channel2);
				if (s.ControlMode2 != null)
					slParams.Add("ControlMode2", string.IsNullOrEmpty(s.ControlMode2) ? string.Empty : s.ControlMode2);
				if (s.StartTime2A != null)
					slParams.Add("StartTime2A", string.IsNullOrEmpty(s.StartTime2A) ? string.Empty : s.StartTime2A);
				if (s.EndTime2A != null)
					slParams.Add("EndTime2A", string.IsNullOrEmpty(s.EndTime2A) ? string.Empty : s.EndTime2A);
				if (s.StartTime2B != null)
					slParams.Add("StartTime2B", string.IsNullOrEmpty(s.StartTime2B) ? string.Empty : s.StartTime2B);
				if (s.EndTime2B != null)
					slParams.Add("EndTime2B", string.IsNullOrEmpty(s.EndTime2B) ? string.Empty : s.EndTime2B);
				if (s.Date2 != null)
					slParams.Add("Date2", string.IsNullOrEmpty(s.Date2) ? string.Empty : s.Date2);
				if (s.ChargeSOC2 != null)
					slParams.Add("ChargeSOC2", string.IsNullOrEmpty(s.ChargeSOC2) ? string.Empty : s.ChargeSOC2);
				if (s.UPS2 != null)
					slParams.Add("UPS2", string.IsNullOrEmpty(s.UPS2) ? string.Empty : s.UPS2);
				if (s.SwitchOn2 != null)
					slParams.Add("SwitchOn2", s.SwitchOn2.HasValue ? s.SwitchOn2.Value.ToString() : "0");
				if (s.SwitchOff2 != null)
					slParams.Add("SwitchOff2", s.SwitchOff2.HasValue ? s.SwitchOff2.Value.ToString() : "0");
				if (s.Delay2 != null)
					slParams.Add("Delay2", string.IsNullOrEmpty(s.Delay2) ? string.Empty : s.Delay2);
				if (s.Duration2 != null)
					slParams.Add("Duration2", string.IsNullOrEmpty(s.Duration2) ? string.Empty : s.Duration2);
				if (s.Pause2 != null)
					slParams.Add("Pause2", string.IsNullOrEmpty(s.Pause2) ? string.Empty : s.Pause2);
				if (s.AllowSyncTime != null)
					slParams.Add("AllowSyncTime", s.AllowSyncTime.HasValue ? s.AllowSyncTime.Value.ToString() : "0");
				if (s.BakBoxSN != null)
					slParams.Add("BakBoxSN", string.IsNullOrEmpty(s.BakBoxSN) ? string.Empty : s.BakBoxSN);
				if (s.BakBoxVer != null)
					slParams.Add("BakBoxVer", string.IsNullOrEmpty(s.BakBoxVer) ? string.Empty : s.BakBoxVer);
				if (s.Batterysn1 != null)
					slParams.Add("Batterysn1", string.IsNullOrEmpty(s.Batterysn1) ? string.Empty : s.Batterysn1);
				if (s.Batterysn10 != null)
					slParams.Add("Batterysn10", string.IsNullOrEmpty(s.Batterysn10) ? string.Empty : s.Batterysn10);
				if (s.Batterysn11 != null)
					slParams.Add("Batterysn11", string.IsNullOrEmpty(s.Batterysn11) ? string.Empty : s.Batterysn11);
				if (s.Batterysn12 != null)
					slParams.Add("Batterysn12", string.IsNullOrEmpty(s.Batterysn12) ? string.Empty : s.Batterysn12);
				if (s.Batterysn13 != null)
					slParams.Add("Batterysn13", string.IsNullOrEmpty(s.Batterysn13) ? string.Empty : s.Batterysn13);
				if (s.Batterysn14 != null)
					slParams.Add("Batterysn14", string.IsNullOrEmpty(s.Batterysn14) ? string.Empty : s.Batterysn14);
				if (s.Batterysn15 != null)
					slParams.Add("Batterysn15", string.IsNullOrEmpty(s.Batterysn15) ? string.Empty : s.Batterysn15);
				if (s.Batterysn16 != null)
					slParams.Add("Batterysn16", string.IsNullOrEmpty(s.Batterysn16) ? string.Empty : s.Batterysn16);
				if (s.Batterysn17 != null)
					slParams.Add("Batterysn17", string.IsNullOrEmpty(s.Batterysn17) ? string.Empty : s.Batterysn17);
				if (s.Batterysn18 != null)
					slParams.Add("Batterysn18", string.IsNullOrEmpty(s.Batterysn18) ? string.Empty : s.Batterysn18);
				if (s.Batterysn2 != null)
					slParams.Add("Batterysn2", string.IsNullOrEmpty(s.Batterysn2) ? string.Empty : s.Batterysn2);
				if (s.Batterysn3 != null)
					slParams.Add("Batterysn3", string.IsNullOrEmpty(s.Batterysn3) ? string.Empty : s.Batterysn3);
				if (s.Batterysn4 != null)
					slParams.Add("Batterysn4", string.IsNullOrEmpty(s.Batterysn4) ? string.Empty : s.Batterysn4);
				if (s.Batterysn5 != null)
					slParams.Add("Batterysn5", string.IsNullOrEmpty(s.Batterysn5) ? string.Empty : s.Batterysn5);
				if (s.Batterysn6 != null)
					slParams.Add("Batterysn6", string.IsNullOrEmpty(s.Batterysn6) ? string.Empty : s.Batterysn6);
				if (s.Batterysn7 != null)
					slParams.Add("Batterysn7", string.IsNullOrEmpty(s.Batterysn7) ? string.Empty : s.Batterysn7);
				if (s.Batterysn8 != null)
					slParams.Add("Batterysn8", string.IsNullOrEmpty(s.Batterysn8) ? string.Empty : s.Batterysn8);
				if (s.Batterysn9 != null)
					slParams.Add("Batterysn9", string.IsNullOrEmpty(s.Batterysn9) ? string.Empty : s.Batterysn9);
				if (s.Bmsversion != null)
					slParams.Add("Bmsversion", string.IsNullOrEmpty(s.Bmsversion) ? string.Empty : s.Bmsversion);
				if (s.BMUModel != null)
					slParams.Add("BMUModel", string.IsNullOrEmpty(s.BMUModel) ? string.Empty : s.BMUModel);
				if (s.CheckTime != null)
					slParams.Add("CheckTime", s.CheckTime.HasValue ? s.CheckTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty);
				if (s.Cobat != null)
					slParams.Add("Cobat", s.Cobat.HasValue ? s.Cobat.Value.ToString() : "0");
				//if (orgSystem.CompanyId != s.CompanyId && s.CompanyId != null)
				//	slParams.Add("CompanyId", s.CompanyId.HasValue ? s.CompanyId.Value.ToString() : string.Empty);
				if (s.CTRate != null)
					slParams.Add("CTRate", s.CTRate.HasValue ? s.CTRate.Value.ToString() : "0");
				if (s.DeleteFlag != null)
					slParams.Add("delete_flag", s.DeleteFlag.HasValue ? s.DeleteFlag.Value.ToString() : "0");
				if (s.Fan != null)
					slParams.Add("Fan", s.Fan.HasValue ? s.Fan.Value.ToString() : "0");
				if (s.Fax != null)
					slParams.Add("Fax", string.IsNullOrEmpty(s.Fax) ? string.Empty : s.Fax);
				if (s.Emsversion != null)
					slParams.Add("Emsversion", string.IsNullOrEmpty(s.Emsversion) ? string.Empty : s.Emsversion);
				if (s.Inputcost != null)
					slParams.Add("Inputcost", s.Inputcost.HasValue ? s.Inputcost.Value.ToString() : "0");
				if (s.InventerSn != null)
					slParams.Add("InventerSn", string.IsNullOrEmpty(s.InventerSn) ? string.Empty : s.InventerSn);
				if (s.Latitude != null)
					slParams.Add("Latitude", string.IsNullOrEmpty(s.Latitude) ? string.Empty : s.Latitude);
				if (s.Longitude != null)
					slParams.Add("Longitude", string.IsNullOrEmpty(s.Longitude) ? string.Empty : s.Longitude);
				if (s.Mbat != null)
					slParams.Add("Mbat", string.IsNullOrEmpty(s.Mbat) ? string.Empty : s.Mbat);
				if (s.GridChargeWE != null)
					slParams.Add("GridChargeWE", s.GridChargeWE.HasValue ? s.GridChargeWE.Value.ToString() : "0");
				if (s.TimeChaFWE1 != null && s.TimeChaFWE1 != 0)
					slParams.Add("TimeChaFWE1", s.TimeChaFWE1.HasValue ? s.TimeChaFWE1.Value.ToString() : "0");
				if (s.TimeChaEWE1 != null && s.TimeChaEWE1 != 0)
					slParams.Add("TimeChaEWE1", s.TimeChaEWE1.HasValue ? s.TimeChaEWE1.Value.ToString() : "0");
				if (s.TimeChaFWE2 != null && s.TimeChaFWE2 != 0)
					slParams.Add("TimeChaFWE2", s.TimeChaFWE2.HasValue ? s.TimeChaFWE2.Value.ToString() : "0");
				if (s.TimeChaEWE2 != null && s.TimeChaEWE2 != 0)
					slParams.Add("TimeChaEWE2", s.TimeChaEWE2.HasValue ? s.TimeChaEWE2.Value.ToString() : "0");
				if (s.TimeDisFWE1 != null && s.TimeDisFWE1 != 0)
					slParams.Add("TimeDisFWE1", s.TimeDisFWE1.HasValue ? s.TimeDisFWE1.Value.ToString() : "0");
				if (s.TimeDisEWE1 != null && s.TimeDisEWE1 != 0)
					slParams.Add("TimeDisEWE1", s.TimeDisEWE1.HasValue ? s.TimeDisEWE1.Value.ToString() : "0");
				if (s.TimeDisFWE2 != null && s.TimeDisFWE2 != 0)
					slParams.Add("TimeDisFWE2", s.TimeDisFWE2.HasValue ? s.TimeDisFWE2.Value.ToString() : "0");
				if (s.TimeDisEWE2 != null && s.TimeDisEWE2 != 0)
					slParams.Add("TimeDisEWE2", s.TimeDisEWE2.HasValue ? s.TimeDisEWE2.Value.ToString() : "0");
				if (s.BatHighCapWE != null)
					slParams.Add("BatHighCapWE", s.BatHighCapWE.HasValue ? s.BatHighCapWE.Value.ToString() : "0");
				if (s.BatUseCapWE != null)
					slParams.Add("BatUseCapWE", s.BatUseCapWE.HasValue ? s.BatUseCapWE.Value.ToString() : "0");
				if (s.CtrDisWE != null)
					slParams.Add("CtrDisWE", s.CtrDisWE.HasValue ? s.CtrDisWE.Value.ToString() : "0");
				if (s.ChargeWorkDays != null)
					slParams.Add("ChargeWorkDays", string.IsNullOrEmpty(s.ChargeWorkDays) ? string.Empty : s.ChargeWorkDays);
				if (s.ChargeWeekend != null)
					slParams.Add("ChargeWeekend", string.IsNullOrEmpty(s.ChargeWeekend) ? string.Empty : s.ChargeWeekend);
				if (s.ChargeModel1 != null)
					slParams.Add("ChargeModel1", s.ChargeModel1.HasValue ? s.ChargeModel1.Value.ToString() : "0");
				if (s.ChargeModel2 != null)
					slParams.Add("ChargeModel2", s.ChargeModel2.HasValue ? s.ChargeModel2.Value.ToString() : "0");
				if (s.MaxGridCharge != null)
					slParams.Add("MaxGridCharge", s.MaxGridCharge.HasValue ? s.MaxGridCharge.Value.ToString() : "0");
				if (s.TOPBMUVer != null)
					slParams.Add("TOPBMUVer", string.IsNullOrEmpty(s.TOPBMUVer) ? string.Empty : s.TOPBMUVer);
				if (s.ISOVer != null)
					slParams.Add("ISOVer", string.IsNullOrEmpty(s.ISOVer) ? string.Empty : s.ISOVer);
				if (s.Popv != null)
					slParams.Add("Popv", s.Popv.HasValue ? s.Popv.Value.ToString() : "0");
			}

			return slParams;
		}

		private string GenerateSnPassword(string sn)
		{
			var result = string.Empty;

			if (!string.IsNullOrWhiteSpace(sn) && sn.Length >= 13)
			{
				var y = sn.Substring(2, 2);   // 年
				var m = sn.Substring(4, 1);   // 月
				var v = sn.Substring(5, 2);   // 版本
				var s = sn.Substring(9, 2);   // 屏幕尺寸
				var no = string.Empty; // 序号
				if (sn.Length == 13)
					no = sn.Substring(11, 2);
				else if (sn.Length >= 15)
					no = sn.Substring(11, 4);
				if (char.IsLetter(m[0]))
				{
					if (m.Equals("A", StringComparison.OrdinalIgnoreCase))
						m = "10";
					else if (m.Equals("B", StringComparison.OrdinalIgnoreCase))
						m = "11";
					else if (m.Equals("C", StringComparison.OrdinalIgnoreCase))
						m = "12";
				}
				var myvsno = m + y + v + s + no;  // 合并
				long myvsnoDigit = 0;
				string clc = string.Empty;
				try
				{
					if (long.TryParse(myvsno, out myvsnoDigit))
						clc = ((long.Parse((int.Parse(m) + int.Parse(no) * 16).ToString() + y + v + s + no) + 2016) * 17).ToString();
					else
						clc = ((long.Parse((int.Parse(y) + int.Parse(no) * 16).ToString() + s + no) + 2016) * 17).ToString();
				}
				//catch (FormatException fexc)
				//{ log.Error("Error in GenerateSnPassword", fexc); }
				catch (Exception exc)
				{
					//log.Error("Error in GenerateSnPassword", exc);
					clc = string.Empty;
				}

				if (!string.IsNullOrWhiteSpace(clc))
					result = clc.Substring(2, 1) + clc.Substring(clc.Length - 3, 3) + clc.Substring(0, 2);
			}

			return result;
		}

		private string GenerateSnPassword2(string sn)
		{
			var result = string.Empty;

			if (!string.IsNullOrWhiteSpace(sn) && sn.Length >= 13)
			{
				var y = sn.Substring(2, 2);   // 年
				var m = sn.Substring(4, 1);   // 月
				var v = sn.Substring(5, 2);   // 版本
																			//var s = sn.Substring(9, 2);   // 屏幕尺寸
				var no = string.Empty;        // 序号
				no = sn.Substring(sn.Length - 4, 4);
				if (char.IsLetter(m[0]))
				{
					if (m.Equals("A", StringComparison.OrdinalIgnoreCase))
						m = "10";
					else if (m.Equals("B", StringComparison.OrdinalIgnoreCase))
						m = "11";
					else if (m.Equals("C", StringComparison.OrdinalIgnoreCase))
						m = "12";
				}
				var noV = 0;
				if (!int.TryParse(no, out noV))
				{ no = sn.Substring(sn.Length - 3, 3); }
				var c1 = string.Empty;
				var c2 = string.Empty;
				try
				{
					c1 = (long.Parse((((long.Parse(y) + long.Parse(m) + long.Parse(no)) * 527).ToString() + y)) * 73).ToString();
					c2 = ((((long.Parse(y) + long.Parse(m) + long.Parse(no)) * 9581) + long.Parse(v) + 2016) * 17).ToString();

					result = c1.Substring(3, 2) + c2.Substring(c2.Length - 4, 4);
				}
				//catch (FormatException fexc)
				//{ log.Error("Error in GenerateSnPassword", fexc); }
				catch (Exception exc)
				{
					//log.Error("Error in GenerateSnPassword", exc);
				}
			}

			return result;
		}

		public List<SysWeatherForecast> GetRecentThreeDaysSysWeatherForecastBySn(string sn)
		{
			List<SysWeatherForecast> result = null;
			var today = DateTime.Now.Date;
			var add3days = today.AddDays(3);
			var q = _weatherForecastRepository.GetAll().Where(x => x.SYS_SN == sn && x.DATETIME >= today && x.DATETIME <= add3days);
			if (q != null && q.Any())
			{
				result = new List<SysWeatherForecast>();
				for (var i = 1; i <= 3; i++)
				{
					var d = today.AddDays(1);

					var wt = q.Where(x => x.DATETIME > today && x.DATETIME < d).OrderByDescending(x => x.CREATEDATE).FirstOrDefault();
					if (wt != null)
						result.Add(new SysWeatherForecast
						{
							clouds = wt.clouds, CompanyId = wt.CompanyId, CREATEDATE = wt.CREATEDATE, DATETIME = wt.DATETIME,
							humidity = wt.humidity, Key = wt.Key, Latitude = wt.Latitude, Longitude = wt.Longitude, pressure = wt.pressure, SYS_SN = wt.SYS_SN, temp_max = wt.temp_max, temp_min = wt.temp_min, weather = wt.weather, wind_deg = wt.wind_deg, wind_speed = wt.wind_speed
						});
					today = today.AddDays(1);
				}
			}

			return result;
		}
		/// <summary>
		/// 根据用户名获取系统列表
		/// </summary>
		/// <param name="pageIndex">指定页</param>
		/// <param name="pageSize">每页最大记录数</param>
		/// <param name="username">用户名</param>
		/// <returns>PaginatedList&lt;VT_SYSTEM&gt;实例</returns>
		public PaginatedList<VT_SYSTEM> GetSystemByUser(int pageIndex, int pageSize, string username, Guid companyId)
		{
			var u = _userRepository.GetSingleByUserName(username, companyId);

			if (u != null)
			{
				if (u.USERTYPE.Equals("sharer", StringComparison.OrdinalIgnoreCase))
				{
					var s = GetSystemBySn(u.Inviter, companyId);
					return new PaginatedList<VT_SYSTEM>(1, int.MaxValue, new List<VT_SYSTEM>() { s });
				}
				else
					return GetSystemByUser(pageIndex, pageSize, u.Key);
			}
			else
			{
				return new PaginatedList<VT_SYSTEM>(1, 10, new List<VT_SYSTEM>().AsQueryable());
			}
		}

		/// <summary>
		/// 根据用户id获取系统列表
		/// </summary>
		/// <param name="pageIndex">指定页</param>
		/// <param name="pageSize">每页最大记录数</param>
		/// <param name="userId">用户id</param>
		/// <returns>PaginatedList&lt;VT_SYSTEM&gt;实例</returns>
		public PaginatedList<VT_SYSTEM> GetSystemByUser(int pageIndex, int pageSize, Guid userId)
		{
			var querySystems = _systemRepository.AllIncluding(x => x.SYS_USER)
				.Where(x => x.UserId == userId && x.DeleteFlag == 0 && (x.Workmode == null || x.Workmode == 0))
				.OrderByDescending(x => x.CreateDatetime)
				.Skip((pageIndex - 1) * pageSize).Take(pageSize)
				.Select(s => new
				{
					//x.AllowAutoUpdate, x.Key, x.ACDC, x.SYS_SN, x.LIC_NO, x.COUNTRY_CODE, x.STATE_CODE, x.CITY_CODE, x.ADDRESS, x.POST_CODE,
					//x.CELL_PHONE, x.EmsStatus, x.Latitude, x.Longitude, x.MONEY_TYPE, x.POINV, x.POPV, x.COBAT, x.MBAT, x.USER_ID, x.USCAPACITY,
					//x.RemarkI, x.SYS_USER, x.CREATE_DATETIME, x.TransFrequency
					s.CreateDatetime, s.Acdc, s.Address, s.AllowAutoUpdate, s.BackUpBox, s.BakBoxSN, s.BakBoxVer, s.Bathighcap, s.Batterysn1, s.Batterysn10, s.Batterysn11, s.Batterysn12, s.Batterysn13, s.Batterysn14, s.Batterysn15, s.Batterysn16, s.Batterysn17, s.Batterysn18, s.Batterysn2, s.Batterysn3, s.Batterysn4, s.Batterysn5, s.Batterysn6, s.Batterysn7,
					s.Batterysn8, s.Batterysn9, s.Batusecap, s.Bmsversion, s.BMUModel, s.CellPhone, s.CheckTime, s.CityCode, s.Cobat, s.CompanyId, s.CountryCode, s.CreateAccount, s.CTRate, s.CtrDis, s.DeleteFlag, s.EmsLanguage, s.EmsStatus, s.Emsversion, s.Fan, s.Fax, s.GCChargePower, s.GCOutputMode, s.GCRatedPower, s.GCSOCEnd, s.GCSOCStart, s.GCTimeEnd, s.GCTimeStart, s.Generator, s.GeneratorMode,
					s.Gridcharge, s.GridType, s.Inputcost, s.InventerSn, s.InvVersion, s.L1Priority, s.L1SocLimit, s.L2Priority, s.L2SocLimit, s.L3Priority, s.L3SocLimit, s.LastupdateAccount, s.LastupdateDatetime, s.LastUploadTime, s.Latitude, s.LicNo, s.Linkman, s.Longitude, s.Mbat, s.Minv, s.Mmeter, s.MoneyType, s.Outputcost, s.Poinv, s.Popv, s.PostCode, s.PowerSource, s.RemarkI, s.RemarkSM, s.Saleprice0,
					s.Saleprice1, s.Saleprice2, s.Saleprice3, s.Saleprice4, s.Saleprice5, s.Saleprice6, s.Saleprice7, s.SaletimeE0, s.SaletimeE1, s.SaletimeE2, s.SaletimeE3, s.SaletimeE4, s.SaletimeE5, s.SaletimeE6, s.SaletimeE7, s.SaletimeS0, s.SaletimeS1, s.SaletimeS2, s.SaletimeS3, s.SaletimeS4, s.SaletimeS5, s.SaletimeS6, s.SaletimeS7, s.SCBSN, s.SCBVer, s.Sellprice, s.Setemail, s.SetFeed, s.Setmode,
					s.SetPhase, s.Settime, s.StateCode, s.SysName, s.SysSn, s.Key, s.SysTimezone, s.Timechae1, s.Timechae2, s.Timechaf1, s.Timechaf2, s.Timedise1, s.Timedise2, s.Timedisf1, s.Timedisf2, s.Uscapacity, s.UserId, s.Workmode, s.ActiveTime, s.TransFrequency, s.AllowSyncTime, s.BatReady, s.Channel1, s.Channel2, s.ChargeBoostCur, s.ControlMode1, s.ControlMode2, s.Date1, s.Date2,
					s.DCI, s.Delay1, s.Delay2, s.Duration1, s.Duration2, s.EndTime1A, s.EndTime1B, s.EndTime2A, s.EndTime2B, s.MeterDCNegate, s.MeterACNegate, s.OnGridCap, s.OutCurProtect, s.Pause1, s.Pause2, s.PowerFact, s.PvISO, s.RCD, s.Safe, s.StartTime1A, s.StartTime1B, s.StartTime2A, s.StartTime2B, s.ChargeModel1, s.ChargeModel2,
					s.ChargeSOC1, s.ChargeSOC2, s.StorageCap, s.SwitchOff1, s.SwitchOff2, s.SwitchOn1, s.SwitchOn2, s.TempThreshold, s.UPS1, s.UPS2, s.Volt10MinAvg, s.Volt5MinAvg, s.MaxGridCharge, s.TOPBMUVer, s.ISOVer
				});

			if (querySystems.Count() > 0)
			{
				var systems = new List<VT_SYSTEM>();
				foreach (var i in querySystems.ToList())
				{
					var s = new VT_SYSTEM
					{
						Acdc = i.Acdc, Address = i.Address, AllowAutoUpdate = i.AllowAutoUpdate, BackUpBox = i.BackUpBox, BakBoxSN = i.BakBoxSN, BakBoxVer = i.BakBoxVer,
						Bathighcap = i.Bathighcap, Batterysn1 = i.Batterysn1, Batterysn10 = i.Batterysn10, Batterysn11 = i.Batterysn11, Batterysn12 = i.Batterysn12,
						Batterysn13 = i.Batterysn13, Batterysn14 = i.Batterysn14, Batterysn15 = i.Batterysn15, Batterysn16 = i.Batterysn16, Batterysn17 = i.Batterysn17,
						Batterysn18 = i.Batterysn18, Batterysn2 = i.Batterysn2, Batterysn3 = i.Batterysn3, Batterysn4 = i.Batterysn4, Batterysn5 = i.Batterysn5,
						Batterysn6 = i.Batterysn6, Batterysn7 = i.Batterysn7, Batterysn8 = i.Batterysn8, Batterysn9 = i.Batterysn9, Batusecap = i.Batusecap, Bmsversion = i.Bmsversion,
						BMUModel = i.BMUModel, CellPhone = i.CellPhone, CheckTime = i.CheckTime, CityCode = i.CityCode, Cobat = i.Cobat, CompanyId = i.CompanyId, CountryCode = i.CountryCode,
						CreateAccount = i.CreateAccount, CreateDatetime = i.CreateDatetime, CTRate = i.CTRate, CtrDis = i.CtrDis, DeleteFlag = i.DeleteFlag, EmsStatus = i.EmsStatus,
						Emsversion = i.Emsversion, UserId = i.UserId, Fan = i.Fan, Fax = i.Fax, GCChargePower = i.GCChargePower, GCOutputMode = i.GCOutputMode, GCRatedPower = i.GCRatedPower,
						GCSOCEnd = i.GCSOCEnd, GCSOCStart = i.GCSOCStart, GCTimeEnd = i.GCTimeEnd, GCTimeStart = i.GCTimeStart, Generator = i.Generator, GeneratorMode = i.GeneratorMode,
						Gridcharge = i.Gridcharge, GridType = i.GridType, Inputcost = i.Inputcost, InventerSn = i.InventerSn, InvVersion = i.InvVersion, LastupdateAccount = i.LastupdateAccount,
						LastupdateDatetime = i.LastupdateDatetime, LastUploadTime = i.LastUploadTime, Latitude = i.Latitude, LicNo = i.LicNo, Linkman = i.Linkman, Longitude = i.Longitude,
						Mbat = i.Mbat, Minv = i.Minv, Mmeter = i.Mmeter, MoneyType = i.MoneyType, Outputcost = i.Outputcost, Poinv = i.Poinv, Popv = i.Popv, PostCode = i.PostCode, PowerSource = i.PowerSource,
						RemarkI = i.RemarkI, RemarkSM = i.RemarkSM, Saleprice0 = i.Saleprice0, Saleprice1 = i.Saleprice1, Saleprice2 = i.Saleprice2, Saleprice3 = i.Saleprice3, Saleprice4 = i.Saleprice4,
						Saleprice5 = i.Saleprice5, Saleprice6 = i.Saleprice6, Saleprice7 = i.Saleprice7, SaletimeE0 = i.SaletimeE0, SaletimeE1 = i.SaletimeE1, SaletimeE2 = i.SaletimeE2, SaletimeE3 = i.SaletimeE3,
						SaletimeE4 = i.SaletimeE4, SaletimeE5 = i.SaletimeE5, SaletimeE6 = i.SaletimeE6, SaletimeE7 = i.SaletimeE7, SaletimeS0 = i.SaletimeS0, SaletimeS1 = i.SaletimeS1, SaletimeS2 = i.SaletimeS2,
						SaletimeS3 = i.SaletimeS3, SaletimeS4 = i.SaletimeS4, SaletimeS5 = i.SaletimeS5, SaletimeS6 = i.SaletimeS6, SaletimeS7 = i.SaletimeS7, SCBSN = i.SCBSN, SCBVer = i.SCBVer,
						Sellprice = i.Sellprice, Setemail = i.Setemail, SetFeed = i.SetFeed, Setmode = i.Setmode, SetPhase = i.SetPhase, Settime = i.Settime, StateCode = i.StateCode, SysName = i.SysName,
						SysSn = i.SysSn, Key = i.Key, Timechae1 = i.Timechae1, Timechae2 = i.Timechae2, Timechaf1 = i.Timechaf1, Timechaf2 = i.Timechaf2, Timedise1 = i.Timedise1, Timedise2 = i.Timedise2,
						Timedisf1 = i.Timedisf1, Timedisf2 = i.Timedisf2, Uscapacity = i.Uscapacity, Workmode = i.Workmode, SysTimezone = i.SysTimezone, EmsLanguage = i.EmsLanguage, L1Priority = i.L1Priority,
						L1SocLimit = i.L1SocLimit, L2Priority = i.L2Priority, L2SocLimit = i.L2SocLimit, L3Priority = i.L3Priority, L3SocLimit = i.L3SocLimit, ActiveTime = i.ActiveTime, TransFrequency = i.TransFrequency,
						AllowSyncTime = i.AllowSyncTime, BatReady = i.BatReady, Channel1 = i.Channel1, Channel2 = i.Channel2, ChargeBoostCur = i.ChargeBoostCur, ControlMode1 = i.ControlMode1, ControlMode2 = i.ControlMode2, Date1 = i.Date1, Date2 = i.Date2, DCI = i.DCI, Delay1 = i.Delay1, Delay2 = i.Delay2, Duration1 = i.Duration1, Duration2 = i.Duration2, ChargeModel1 = i.ChargeModel1, ChargeModel2 = i.ChargeModel2,
						EndTime1A = i.EndTime1A, EndTime1B = i.EndTime1B, EndTime2A = i.EndTime2A, EndTime2B = i.EndTime2B, MeterDCNegate = i.MeterDCNegate, MeterACNegate = i.MeterACNegate, OnGridCap = i.OnGridCap, OutCurProtect = i.OutCurProtect, Pause1 = i.Pause1, Pause2 = i.Pause2, PowerFact = i.PowerFact, PvISO = i.PvISO, RCD = i.RCD, Safe = i.Safe, StartTime1A = i.StartTime1A, StartTime1B = i.StartTime1B, StartTime2A = i.StartTime2A, StartTime2B = i.StartTime2B,
						ChargeSOC1 = i.ChargeSOC1, ChargeSOC2 = i.ChargeSOC2, StorageCap = i.StorageCap, SwitchOff1 = i.SwitchOff1, SwitchOff2 = i.SwitchOff2, SwitchOn1 = i.SwitchOn1, SwitchOn2 = i.SwitchOn2, TempThreshold = i.TempThreshold, UPS1 = i.UPS1, UPS2 = i.UPS2, Volt10MinAvg = i.Volt10MinAvg, Volt5MinAvg = i.Volt5MinAvg, MaxGridCharge = i.MaxGridCharge,
						TOPBMUVer = i.TOPBMUVer, ISOVer = i.ISOVer
					};
					s.ListWeatherForecast = GetRecentThreeDaysSysWeatherForecastBySn(s.SysSn);
					systems.Add(s);
				}

				return new PaginatedList<VT_SYSTEM>(pageIndex, pageSize, systems.AsQueryable());
			}
			else
			{
				return new PaginatedList<VT_SYSTEM>(pageIndex, pageSize);
			}
		}

		/// <summary>
		/// 根据用户获取系统列表
		/// </summary>
		/// <param name="pageIndex">指定页</param>
		/// <param name="pageSize">每页最大记录数</param>
		/// <param name="u">用户实例</param>
		/// <returns>PaginatedList&lt;VT_SYSTEM&gt;实例</returns>
		public PaginatedList<VT_SYSTEM> GetSystemByUser(int pageIndex, int pageSize, SYS_USER u)
		{
			if (u != null)
				return GetSystemByUser(pageIndex, pageSize, u.Key);
			else
				return new PaginatedList<VT_SYSTEM>(1, 10);
		}

		/// <summary>
		/// 根据安装商id获取系统列表
		/// </summary>
		/// <param name="pageIndex">指定页</param>
		/// <param name="pageSize">每页最大记录数</param>
		/// <param name="installerUserId">安装商id</param>
		/// <returns>PaginatedList&lt;VT_SYSTEM&gt;实例</returns>
		public PaginatedList<VT_SYSTEM> GetSystemByInstaller(int pageIndex, int pageSize, Guid installerUserId, Guid companyId)
		{
			var u = _userRepository.GetAll().FirstOrDefault(x => x.Key == installerUserId); //_userRepository.GetSingle(installerUserId);

			if (u != null && u.CompanyId.HasValue)
			{
				return GetSystemByLicense(pageIndex, pageSize, u.LICNO, companyId);
			}
			else
			{
				return new PaginatedList<VT_SYSTEM>(1, 10, new List<VT_SYSTEM>().AsQueryable());
			}
		}
		/// <summary>
		/// 根据安装商名称获取系统列表
		/// </summary>
		/// <param name="pageIndex">指定页</param>
		/// <param name="pageSize">每页最大记录数</param>
		/// <param name="installerUsername">安装商名称</param>
		/// <returns>PaginatedList&lt;VT_SYSTEM&gt;实例</returns>
		public PaginatedList<VT_SYSTEM> GetSystemByInstaller(int pageIndex, int pageSize, string installerUsername, Guid companyId)
		{
			var u = _userRepository.GetSingleByUserName(installerUsername, companyId);

			if (u != null)
			{
				return GetSystemByLicense(pageIndex, pageSize, u.LICNO, companyId);
			}
			else
			{
				return new PaginatedList<VT_SYSTEM>(1, 10);
			}
		}

		/// <summary>
		/// 根据license获取系统列表
		/// </summary>
		/// <param name="pageIndex">指定页</param>
		/// <param name="pageSize">每页最大记录数</param>
		/// <param name="license">license</param>
		/// <returns>PaginatedList&lt;VT_SYSTEM&gt;实例</returns>
		public PaginatedList<VT_SYSTEM> GetSystemByLicense(int pageIndex, int pageSize, string license, Guid companyId)
		{
			// 取消过滤未安装的设备
			//var anonySystems = QueryInstalledSystem().Where(x => x.LIC_NO == license && x.DELETE_FLAG == 0 && (x.WorkMode == 0 || x.WorkMode == null))
			//var anonySystems = from s in _systemRepository.GetAll().Where(x => x.CompanyId == companyId && x.LicNo == license && x.DeleteFlag == 0 && (x.Workmode == 0 || x.Workmode == null))
			//									 join u in _userRepository.GetAll() on s.UserId equals u.Key into s_u
			//									 from su in s_u.DefaultIfEmpty()
			//									 select new
			//									 {
			//										 s.CreateDatetime, s.Acdc, s.Address, s.AllowAutoUpdate, s.BackUpBox, s.BakBoxSN, s.BakBoxVer, s.Bathighcap, s.Batterysn1, s.Batterysn10, s.Batterysn11, s.Batterysn12, s.Batterysn13, s.Batterysn14, s.Batterysn15, s.Batterysn16, s.Batterysn17, s.Batterysn18, s.Batterysn2, s.Batterysn3, s.Batterysn4, s.Batterysn5, s.Batterysn6, s.Batterysn7,
			//										 s.Batterysn8, s.Batterysn9, s.Batusecap, s.Bmsversion, s.BMUModel, s.CellPhone, s.CheckTime, s.CityCode, s.Cobat, s.CompanyId, s.CountryCode, s.CreateAccount, s.CTRate, s.CtrDis, s.DeleteFlag, s.EmsLanguage, s.EmsStatus, s.Emsversion, s.Fan, s.Fax, s.GCChargePower, s.GCOutputMode, s.GCRatedPower, s.GCSOCEnd, s.GCSOCStart, s.GCTimeEnd, s.GCTimeStart, s.Generator, s.GeneratorMode,
			//										 s.Gridcharge, s.GridType, s.Inputcost, s.InventerSn, s.InvVersion, s.L1Priority, s.L1SocLimit, s.L2Priority, s.L2SocLimit, s.L3Priority, s.L3SocLimit, s.LastupdateAccount, s.LastupdateDatetime, s.LastUploadTime, s.Latitude, s.LicNo, s.Linkman, s.Longitude, s.Mbat, s.Minv, s.Mmeter, s.MoneyType, s.Outputcost, s.Poinv, s.Popv, s.PostCode, s.PowerSource, s.RemarkI, s.RemarkSM, s.Saleprice0,
			//										 s.Saleprice1, s.Saleprice2, s.Saleprice3, s.Saleprice4, s.Saleprice5, s.Saleprice6, s.Saleprice7, s.SaletimeE0, s.SaletimeE1, s.SaletimeE2, s.SaletimeE3, s.SaletimeE4, s.SaletimeE5, s.SaletimeE6, s.SaletimeE7, s.SaletimeS0, s.SaletimeS1, s.SaletimeS2, s.SaletimeS3, s.SaletimeS4, s.SaletimeS5, s.SaletimeS6, s.SaletimeS7, s.SCBSN, s.SCBVer, s.Sellprice, s.Setemail, s.SetFeed, s.Setmode,
			//										 s.SetPhase, s.Settime, s.StateCode, s.SysName, s.SysSn, s.Key, s.SysTimezone, s.Timechae1, s.Timechae2, s.Timechaf1, s.Timechaf2, s.Timedise1, s.Timedise2, s.Timedisf1, s.Timedisf2, s.Uscapacity, s.UserId, s.Workmode, s.ActiveTime, s.TransFrequency, s.AllowSyncTime, s.BatReady, s.Channel1, s.Channel2, s.ChargeBoostCur, s.ControlMode1, s.ControlMode2, s.Date1, s.Date2,
			//										 s.DCI, s.Delay1, s.Delay2, s.Duration1, s.Duration2, s.EndTime1A, s.EndTime1B, s.EndTime2A, s.EndTime2B, s.MeterDCNegate, s.MeterACNegate, s.OnGridCap, s.OutCurProtect, s.Pause1, s.Pause2, s.PowerFact, s.PvISO, s.RCD, s.Safe, s.StartTime1A, s.StartTime1B, s.StartTime2A, s.StartTime2B, s.ChargeMode1, s.ChargeMode2,
			//										 s.ChargeSOC1, s.ChargeSOC2, s.StorageCap, s.SwitchOff1, s.SwitchOff2, s.SwitchOn1, s.SwitchOn2, s.TempThreshold, s.UPS1, s.UPS2, s.Volt10MinAvg, s.Volt5MinAvg,
			//										 su.USERNAME
			//									 };
			int t = 0;
			var systems = _systemRepository.GetVtSystemByLicense(companyId, license, pageIndex, pageSize, out t);

			if (systems != null && systems.Any())
			{
				foreach (var s in systems)
				{
					//var s = new VT_SYSTEM
					//{
					//	Acdc = i.Acdc, Address = i.Address, AllowAutoUpdate = i.AllowAutoUpdate, BackUpBox = i.BackUpBox, BakBoxSN = i.BakBoxSN, BakBoxVer = i.BakBoxVer,
					//	Bathighcap = i.Bathighcap, Batterysn1 = i.Batterysn1, Batterysn10 = i.Batterysn10, Batterysn11 = i.Batterysn11, Batterysn12 = i.Batterysn12,
					//	Batterysn13 = i.Batterysn13, Batterysn14 = i.Batterysn14, Batterysn15 = i.Batterysn15, Batterysn16 = i.Batterysn16, Batterysn17 = i.Batterysn17,
					//	Batterysn18 = i.Batterysn18, Batterysn2 = i.Batterysn2, Batterysn3 = i.Batterysn3, Batterysn4 = i.Batterysn4, Batterysn5 = i.Batterysn5,
					//	Batterysn6 = i.Batterysn6, Batterysn7 = i.Batterysn7, Batterysn8 = i.Batterysn8, Batterysn9 = i.Batterysn9, Batusecap = i.Batusecap, Bmsversion = i.Bmsversion,
					//	BMUModel = i.BMUModel, CellPhone = i.CellPhone, CheckTime = i.CheckTime, CityCode = i.CityCode, Cobat = i.Cobat, CompanyId = i.CompanyId, CountryCode = i.CountryCode,
					//	CreateAccount = i.CreateAccount, CreateDatetime = i.CreateDatetime, CTRate = i.CTRate, CtrDis = i.CtrDis, DeleteFlag = i.DeleteFlag, EmsStatus = i.EmsStatus,
					//	Emsversion = i.Emsversion, UserId = i.UserId, Fan = i.Fan, Fax = i.Fax, GCChargePower = i.GCChargePower, GCOutputMode = i.GCOutputMode, GCRatedPower = i.GCRatedPower,
					//	GCSOCEnd = i.GCSOCEnd, GCSOCStart = i.GCSOCStart, GCTimeEnd = i.GCTimeEnd, GCTimeStart = i.GCTimeStart, Generator = i.Generator, GeneratorMode = i.GeneratorMode,
					//	Gridcharge = i.Gridcharge, GridType = i.GridType, Inputcost = i.Inputcost, InventerSn = i.InventerSn, InvVersion = i.InvVersion, LastupdateAccount = i.LastupdateAccount,
					//	LastupdateDatetime = i.LastupdateDatetime, LastUploadTime = i.LastUploadTime, Latitude = i.Latitude, LicNo = i.LicNo, Linkman = i.Linkman, Longitude = i.Longitude,
					//	Mbat = i.Mbat, Minv = i.Minv, Mmeter = i.Mmeter, MoneyType = i.MoneyType, Outputcost = i.Outputcost, Poinv = i.Poinv, Popv = i.Popv, PostCode = i.PostCode, PowerSource = i.PowerSource,
					//	RemarkI = i.RemarkI, RemarkSM = i.RemarkSM, Saleprice0 = i.Saleprice0, Saleprice1 = i.Saleprice1, Saleprice2 = i.Saleprice2, Saleprice3 = i.Saleprice3, Saleprice4 = i.Saleprice4,
					//	Saleprice5 = i.Saleprice5, Saleprice6 = i.Saleprice6, Saleprice7 = i.Saleprice7, SaletimeE0 = i.SaletimeE0, SaletimeE1 = i.SaletimeE1, SaletimeE2 = i.SaletimeE2, SaletimeE3 = i.SaletimeE3,
					//	SaletimeE4 = i.SaletimeE4, SaletimeE5 = i.SaletimeE5, SaletimeE6 = i.SaletimeE6, SaletimeE7 = i.SaletimeE7, SaletimeS0 = i.SaletimeS0, SaletimeS1 = i.SaletimeS1, SaletimeS2 = i.SaletimeS2,
					//	SaletimeS3 = i.SaletimeS3, SaletimeS4 = i.SaletimeS4, SaletimeS5 = i.SaletimeS5, SaletimeS6 = i.SaletimeS6, SaletimeS7 = i.SaletimeS7, SCBSN = i.SCBSN, SCBVer = i.SCBVer,
					//	Sellprice = i.Sellprice, Setemail = i.Setemail, SetFeed = i.SetFeed, Setmode = i.Setmode, SetPhase = i.SetPhase, Settime = i.Settime, StateCode = i.StateCode, SysName = i.SysName,
					//	SysSn = i.SysSn, Key = i.Key, Timechae1 = i.Timechae1, Timechae2 = i.Timechae2, Timechaf1 = i.Timechaf1, Timechaf2 = i.Timechaf2, Timedise1 = i.Timedise1, Timedise2 = i.Timedise2,
					//	Timedisf1 = i.Timedisf1, Timedisf2 = i.Timedisf2, Uscapacity = i.Uscapacity, Workmode = i.Workmode, SysTimezone = i.SysTimezone, EmsLanguage = i.EmsLanguage, L1Priority = i.L1Priority,
					//	L1SocLimit = i.L1SocLimit, L2Priority = i.L2Priority, L2SocLimit = i.L2SocLimit, L3Priority = i.L3Priority, L3SocLimit = i.L3SocLimit, ActiveTime = i.ActiveTime, TransFrequency = i.TransFrequency,
					//	AllowSyncTime = i.AllowSyncTime, BatReady = i.BatReady, Channel1 = i.Channel1, Channel2 = i.Channel2, ChargeBoostCur = i.ChargeBoostCur, ControlMode1 = i.ControlMode1, ControlMode2 = i.ControlMode2, Date1 = i.Date1, Date2 = i.Date2, DCI = i.DCI, Delay1 = i.Delay1, Delay2 = i.Delay2, Duration1 = i.Duration1, Duration2 = i.Duration2, ChargeMode1 = i.ChargeMode1, ChargeMode2 = i.ChargeMode2,
					//	EndTime1A = i.EndTime1A, EndTime1B = i.EndTime1B, EndTime2A = i.EndTime2A, EndTime2B = i.EndTime2B, MeterDCNegate = i.MeterDCNegate, MeterACNegate = i.MeterACNegate, OnGridCap = i.OnGridCap, OutCurProtect = i.OutCurProtect, Pause1 = i.Pause1, Pause2 = i.Pause2, PowerFact = i.PowerFact, PvISO = i.PvISO, RCD = i.RCD, Safe = i.Safe, StartTime1A = i.StartTime1A, StartTime1B = i.StartTime1B, StartTime2A = i.StartTime2A, StartTime2B = i.StartTime2B,
					//	ChargeSOC1 = i.ChargeSOC1, ChargeSOC2 = i.ChargeSOC2, StorageCap = i.StorageCap, SwitchOff1 = i.SwitchOff1, SwitchOff2 = i.SwitchOff2, SwitchOn1 = i.SwitchOn1, SwitchOn2 = i.SwitchOn2, TempThreshold = i.TempThreshold, UPS1 = i.UPS1, UPS2 = i.UPS2, Volt10MinAvg = i.Volt10MinAvg, Volt5MinAvg = i.Volt5MinAvg
					//};
					//if (i.UserId.HasValue)
					//	s.SYS_USER = new SYS_USER { Key = i.UserId.Value, USERNAME = i.USERNAME };
					s.ListWeatherForecast = GetRecentThreeDaysSysWeatherForecastBySn(s.SysSn);
				}
			}

			return systems.ToPaginatedList(pageIndex, pageSize, t);
			//			return new PaginatedList<VT_SYSTEM>(pageIndex, pageSize, systems.AsQueryable());
		}

		public VT_SYSTEM GetSystemBySn(string sn, Guid companyId)
		{
			var result = _systemRepository.GetAll().FirstOrDefault(x => x.DeleteFlag == 0 && x.CompanyId == companyId && x.SysSn == sn);
			if (result != null)
				result.ListWeatherForecast = GetRecentThreeDaysSysWeatherForecastBySn(result.SysSn);

			return result;
		}

		public bool UpdateSystem(VT_SYSTEM dbVtSystem, VT_SYSTEM vtSystem, Dictionary<string, string> dicEditProperties)
		{
			var result = true;
			//vtSystem.LastupdateAccount = LoginUser.UserId.ToString();
			//vtSystem.LastupdateDatetime = DateTime.Now;
			//var dbVtSystem = _systemRepository.GetSystemById(vtSystem.Key);
			var changelog = GetVtSystemEditProperties(dbVtSystem, vtSystem);

			if (dbVtSystem.LicNo != vtSystem.LicNo)
			{ dbVtSystem.Workmode = 0; }
			var workmodeChanged = dbVtSystem.Workmode != vtSystem.Workmode;
			if (workmodeChanged)
				dbVtSystem.ActiveTime = DateTime.Now;
			if (dbVtSystem.OEM_flag == 1)
			{
				dbVtSystem.OEM_flag = dbVtSystem.OEM_flag;
				dbVtSystem.OEM_Plant_id = dbVtSystem.OEM_Plant_id;
			}

			if (dbVtSystem.CountryCode != vtSystem.CountryCode || dbVtSystem.StateCode != vtSystem.StateCode || dbVtSystem.PostCode != vtSystem.PostCode)
			{
				dbVtSystem.Latitude = string.Empty;
				dbVtSystem.Longitude = string.Empty;
			}

			if (!string.IsNullOrWhiteSpace(vtSystem.Date1))
				dbVtSystem.Date1 = Convert.ToInt32(vtSystem.Date1, 2).ToString();
			if (!string.IsNullOrWhiteSpace(vtSystem.Date2))
				dbVtSystem.Date2 = Convert.ToInt32(vtSystem.Date2, 2).ToString();

			if (dicEditProperties != null && dicEditProperties.Count > 0)
			{
				foreach (var itm in dicEditProperties)
				{
					dbVtSystem.GetType().GetProperty(itm.Key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase).SetValue(dbVtSystem, vtSystem.GetType().GetProperty(itm.Key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase).GetValue(vtSystem));
				}
			}

			//try
			//{
			try
			{
				_systemRepository.Edit(dbVtSystem);
				_systemRepository.Save();
				#region 更新电价
				if (vtSystem.Sellprice.HasValue && vtSystem.Sellprice.Value > 0)
				{
					var sellPrices = _sysSellPriceRepository.GetBySn(vtSystem.SysSn);
					if (sellPrices != null && sellPrices.Any())
					{
						var lastSellPrice = sellPrices.OrderByDescending(x => x.SellPriceDate).First();
						if (decimal.Round(lastSellPrice.SellPricePrice.HasValue ? lastSellPrice.SellPricePrice.Value : 0, 6) != decimal.Round(vtSystem.Sellprice.Value, 6))
						{
							if (lastSellPrice.SellPriceDate.Date == DateTime.Now.Date)
							{
								lastSellPrice.SellPricePrice = vtSystem.Sellprice.Value;
								_sysSellPriceRepository.Edit(lastSellPrice);
								//AddUserLog(vtSystem.SysSn, string.Format("update sellprice sn: {0}, sellprice: {1}, update result: {2}", vtSystem.SysSn, vtSystem.Sellprice.Value.ToString(), updateResult.ToString()), LogType.System);
							}
							else
							{
								var newSellPrice = new Sys_SellPrice { SellPriceSn = vtSystem.SysSn, Key = Guid.NewGuid(), SellPriceCreateTime = DateTime.Now, SellPriceDate = DateTime.Now.Date, SellPriceDeleteFlag = 0, SellPricePrice = vtSystem.Sellprice.Value };
								_sysSellPriceRepository.Add(newSellPrice);
								//AddUserLog(vtSystem.SysSn, string.Format("add sellprice sn: {0}, sellprice: {1}, add result: {2}", vtSystem.SysSn, vtSystem.Sellprice.Value.ToString(), addResult.ToString()), LogType.System);
							}
							_sysSellPriceRepository.Save();
						}
					}
					else
					{
						var newSellPrice = new Sys_SellPrice { SellPriceSn = vtSystem.SysSn, Key = Guid.NewGuid(), SellPriceCreateTime = DateTime.Now, SellPriceDate = DateTime.Now.Date, SellPriceDeleteFlag = 0, SellPricePrice = vtSystem.Sellprice.Value };
						_sysSellPriceRepository.Add(newSellPrice);
						_sysSellPriceRepository.Save();
						//AddUserLog(vtSystem.SysSn, string.Format("add sellprice sn: {0}, sellprice: {1}, add result: {2}", vtSystem.SysSn, vtSystem.Sellprice.Value.ToString(), addResult.ToString()), LogType.System);
					}
				}

				var purchasePrices = _sysPurchasePriceRepository.GetBySn(vtSystem.SysSn);
				if (purchasePrices != null && purchasePrices.Any())
				{
					var lastPurchasePrice = purchasePrices.Where(x => x.PurchasePriceDate == DateTime.Now.Date);
					if (lastPurchasePrice.Any())
					{
						var purchasePriceToUpdate = lastPurchasePrice.OrderBy(x => x.PurchasePriceBeginTime).ToList();
						purchasePriceToUpdate[0].PurchasePriceBeginTime = (short)(vtSystem.SaletimeS0 % 24);
						purchasePriceToUpdate[1].PurchasePriceBeginTime = (short)(vtSystem.SaletimeS1 % 24);
						purchasePriceToUpdate[2].PurchasePriceBeginTime = (short)(vtSystem.SaletimeS2 % 24);
						purchasePriceToUpdate[3].PurchasePriceBeginTime = (short)(vtSystem.SaletimeS3 % 24);
						purchasePriceToUpdate[4].PurchasePriceBeginTime = (short)(vtSystem.SaletimeS4 % 24);
						purchasePriceToUpdate[5].PurchasePriceBeginTime = (short)(vtSystem.SaletimeS5 % 24);
						purchasePriceToUpdate[6].PurchasePriceBeginTime = (short)(vtSystem.SaletimeS6 % 24);
						purchasePriceToUpdate[7].PurchasePriceBeginTime = (short)(vtSystem.SaletimeS7 % 24);
						purchasePriceToUpdate[0].PurchasePriceEndTime = (short)(vtSystem.SaletimeE0 % 24);
						purchasePriceToUpdate[1].PurchasePriceEndTime = (short)(vtSystem.SaletimeE1 % 24);
						purchasePriceToUpdate[2].PurchasePriceEndTime = (short)(vtSystem.SaletimeE2 % 24);
						purchasePriceToUpdate[3].PurchasePriceEndTime = (short)(vtSystem.SaletimeE3 % 24);
						purchasePriceToUpdate[4].PurchasePriceEndTime = (short)(vtSystem.SaletimeE4 % 24);
						purchasePriceToUpdate[5].PurchasePriceEndTime = (short)(vtSystem.SaletimeE5 % 24);
						purchasePriceToUpdate[6].PurchasePriceEndTime = (short)(vtSystem.SaletimeE6 % 24);
						purchasePriceToUpdate[7].PurchasePriceEndTime = (short)(vtSystem.SaletimeE7 % 24);
						purchasePriceToUpdate[0].PurchasePrice = vtSystem.Saleprice0;
						purchasePriceToUpdate[1].PurchasePrice = vtSystem.Saleprice1;
						purchasePriceToUpdate[2].PurchasePrice = vtSystem.Saleprice2;
						purchasePriceToUpdate[3].PurchasePrice = vtSystem.Saleprice3;
						purchasePriceToUpdate[4].PurchasePrice = vtSystem.Saleprice4;
						purchasePriceToUpdate[5].PurchasePrice = vtSystem.Saleprice5;
						purchasePriceToUpdate[6].PurchasePrice = vtSystem.Saleprice6;
						purchasePriceToUpdate[7].PurchasePrice = vtSystem.Saleprice7;

						var updateResult = true;
						foreach (var item in purchasePriceToUpdate)
							_sysPurchasePriceRepository.Edit(item);

						string t = string.Format("{0}-{7},{1}-{8},{2}-{9},{3}-{10},{4}-{11},{5}-{12},{6}-{13},{7}-{14}", vtSystem.SaletimeS0, vtSystem.SaletimeS1, vtSystem.SaletimeS2, vtSystem.SaletimeS3, vtSystem.SaletimeS4, vtSystem.SaletimeS5, vtSystem.SaletimeS6, vtSystem.SaletimeS7, vtSystem.SaletimeE0, vtSystem.SaletimeE1, vtSystem.SaletimeE2, vtSystem.SaletimeE3, vtSystem.SaletimeE4, vtSystem.SaletimeE5, vtSystem.SaletimeE6, vtSystem.SaletimeE7);
						string p = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", (vtSystem.Saleprice0.HasValue ? vtSystem.Saleprice0.Value.ToString() : "Null"), (vtSystem.Saleprice1.HasValue ? vtSystem.Saleprice1.Value.ToString() : "Null"), (vtSystem.Saleprice2.HasValue ? vtSystem.Saleprice2.Value.ToString() : "Null"), (vtSystem.Saleprice3.HasValue ? vtSystem.Saleprice3.Value.ToString() : "Null"), (vtSystem.Saleprice4.HasValue ? vtSystem.Saleprice4.Value.ToString() : "Null"), (vtSystem.Saleprice5.HasValue ? vtSystem.Saleprice5.Value.ToString() : "Null"), (vtSystem.Saleprice6.HasValue ? vtSystem.Saleprice6.Value.ToString() : "Null"), (vtSystem.Saleprice7.HasValue ? vtSystem.Saleprice7.Value.ToString() : "Null"));
						//AddUserLog(vtSystem.SysSn, string.Format("update purchase price sn: {0}, times:{1}, prices:{2}, add result: {3}", vtSystem.SysSn, t, p, updateResult.ToString()), LogType.System);
					}
					else
					{
						var newPurchasePrice = new List<Sys_PurchasePrice>() {
																new Sys_PurchasePrice { PurchasePriceCreateTime = DateTime.Now, PurchasePriceDate = DateTime.Now.Date, PurchasePriceDeleteFlag = 0, PurchasePriceSn = vtSystem.SysSn, Key = Guid.NewGuid(), PurchasePrice = vtSystem.Saleprice0, PurchasePriceBeginTime = (short)(vtSystem.SaletimeS0 % 24), PurchasePriceEndTime = (short)(vtSystem.SaletimeE0 % 24) },
																new Sys_PurchasePrice { PurchasePriceCreateTime = DateTime.Now, PurchasePriceDate = DateTime.Now.Date, PurchasePriceDeleteFlag = 0, PurchasePriceSn = vtSystem.SysSn, Key = Guid.NewGuid(), PurchasePrice = vtSystem.Saleprice1, PurchasePriceBeginTime = (short)(vtSystem.SaletimeS1 % 24), PurchasePriceEndTime = (short)(vtSystem.SaletimeE1 % 24) },
																new Sys_PurchasePrice { PurchasePriceCreateTime = DateTime.Now, PurchasePriceDate = DateTime.Now.Date, PurchasePriceDeleteFlag = 0, PurchasePriceSn = vtSystem.SysSn, Key = Guid.NewGuid(), PurchasePrice = vtSystem.Saleprice2, PurchasePriceBeginTime = (short)(vtSystem.SaletimeS2 % 24), PurchasePriceEndTime = (short)(vtSystem.SaletimeE2 % 24) },
																new Sys_PurchasePrice { PurchasePriceCreateTime = DateTime.Now, PurchasePriceDate = DateTime.Now.Date, PurchasePriceDeleteFlag = 0, PurchasePriceSn = vtSystem.SysSn, Key = Guid.NewGuid(), PurchasePrice = vtSystem.Saleprice3, PurchasePriceBeginTime = (short)(vtSystem.SaletimeS3 % 24), PurchasePriceEndTime = (short)(vtSystem.SaletimeE3 % 24) },
																new Sys_PurchasePrice { PurchasePriceCreateTime = DateTime.Now, PurchasePriceDate = DateTime.Now.Date, PurchasePriceDeleteFlag = 0, PurchasePriceSn = vtSystem.SysSn, Key = Guid.NewGuid(), PurchasePrice = vtSystem.Saleprice4, PurchasePriceBeginTime = (short)(vtSystem.SaletimeS4 % 24), PurchasePriceEndTime = (short)(vtSystem.SaletimeE4 % 24) },
																new Sys_PurchasePrice { PurchasePriceCreateTime = DateTime.Now, PurchasePriceDate = DateTime.Now.Date, PurchasePriceDeleteFlag = 0, PurchasePriceSn = vtSystem.SysSn, Key = Guid.NewGuid(), PurchasePrice = vtSystem.Saleprice5, PurchasePriceBeginTime = (short)(vtSystem.SaletimeS5 % 24), PurchasePriceEndTime = (short)(vtSystem.SaletimeE5 % 24) },
																new Sys_PurchasePrice { PurchasePriceCreateTime = DateTime.Now, PurchasePriceDate = DateTime.Now.Date, PurchasePriceDeleteFlag = 0, PurchasePriceSn = vtSystem.SysSn, Key = Guid.NewGuid(), PurchasePrice = vtSystem.Saleprice6, PurchasePriceBeginTime = (short)(vtSystem.SaletimeS6 % 24), PurchasePriceEndTime = (short)(vtSystem.SaletimeE6 % 24) },
																new Sys_PurchasePrice { PurchasePriceCreateTime = DateTime.Now, PurchasePriceDate = DateTime.Now.Date, PurchasePriceDeleteFlag = 0, PurchasePriceSn = vtSystem.SysSn, Key = Guid.NewGuid(), PurchasePrice = vtSystem.Saleprice7, PurchasePriceBeginTime = (short)(vtSystem.SaletimeS7 % 24), PurchasePriceEndTime = (short)(vtSystem.SaletimeE7 % 24) }
														};
						_sysPurchasePriceRepository.AddBulk(newPurchasePrice);
						string t = string.Format("{0}-{7},{1}-{8},{2}-{9},{3}-{10},{4}-{11},{5}-{12},{6}-{13},{7}-{14}", vtSystem.SaletimeS0, vtSystem.SaletimeS1, vtSystem.SaletimeS2, vtSystem.SaletimeS3, vtSystem.SaletimeS4, vtSystem.SaletimeS5, vtSystem.SaletimeS6, vtSystem.SaletimeS7, vtSystem.SaletimeE0, vtSystem.SaletimeE1, vtSystem.SaletimeE2, vtSystem.SaletimeE3, vtSystem.SaletimeE4, vtSystem.SaletimeE5, vtSystem.SaletimeE6, vtSystem.SaletimeE7);
						string p = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", (vtSystem.Saleprice0.HasValue ? vtSystem.Saleprice0.Value.ToString() : "Null"), (vtSystem.Saleprice1.HasValue ? vtSystem.Saleprice1.Value.ToString() : "Null"), (vtSystem.Saleprice2.HasValue ? vtSystem.Saleprice2.Value.ToString() : "Null"), (vtSystem.Saleprice3.HasValue ? vtSystem.Saleprice3.Value.ToString() : "Null"), (vtSystem.Saleprice4.HasValue ? vtSystem.Saleprice4.Value.ToString() : "Null"), (vtSystem.Saleprice5.HasValue ? vtSystem.Saleprice5.Value.ToString() : "Null"), (vtSystem.Saleprice6.HasValue ? vtSystem.Saleprice6.Value.ToString() : "Null"), (vtSystem.Saleprice7.HasValue ? vtSystem.Saleprice7.Value.ToString() : "Null"));
						//AddUserLog(vtSystem.SysSn, string.Format("add purchase price sn: {0}, times:{1}, prices:{2}, add result: {3}", vtSystem.SysSn, t, p, addResult.ToString()), LogType.System);
					}
					//}
				}
				else
				{
					var newPurchasePrice = new List<Sys_PurchasePrice>() {
																new Sys_PurchasePrice { PurchasePriceCreateTime = DateTime.Now, PurchasePriceDate = DateTime.Now.Date, PurchasePriceDeleteFlag = 0, PurchasePriceSn = vtSystem.SysSn, Key = Guid.NewGuid(), PurchasePrice = vtSystem.Saleprice0, PurchasePriceBeginTime = (short)(vtSystem.SaletimeS0 % 24), PurchasePriceEndTime = (short)(vtSystem.SaletimeE0 % 24) },
																new Sys_PurchasePrice { PurchasePriceCreateTime = DateTime.Now, PurchasePriceDate = DateTime.Now.Date, PurchasePriceDeleteFlag = 0, PurchasePriceSn = vtSystem.SysSn, Key = Guid.NewGuid(), PurchasePrice = vtSystem.Saleprice1, PurchasePriceBeginTime = (short)(vtSystem.SaletimeS1 % 24), PurchasePriceEndTime = (short)(vtSystem.SaletimeE1 % 24) },
																new Sys_PurchasePrice { PurchasePriceCreateTime = DateTime.Now, PurchasePriceDate = DateTime.Now.Date, PurchasePriceDeleteFlag = 0, PurchasePriceSn = vtSystem.SysSn, Key = Guid.NewGuid(), PurchasePrice = vtSystem.Saleprice2, PurchasePriceBeginTime = (short)(vtSystem.SaletimeS2 % 24), PurchasePriceEndTime = (short)(vtSystem.SaletimeE2 % 24) },
																new Sys_PurchasePrice { PurchasePriceCreateTime = DateTime.Now, PurchasePriceDate = DateTime.Now.Date, PurchasePriceDeleteFlag = 0, PurchasePriceSn = vtSystem.SysSn, Key = Guid.NewGuid(), PurchasePrice = vtSystem.Saleprice3, PurchasePriceBeginTime = (short)(vtSystem.SaletimeS3 % 24), PurchasePriceEndTime = (short)(vtSystem.SaletimeE3 % 24) },
																new Sys_PurchasePrice { PurchasePriceCreateTime = DateTime.Now, PurchasePriceDate = DateTime.Now.Date, PurchasePriceDeleteFlag = 0, PurchasePriceSn = vtSystem.SysSn, Key = Guid.NewGuid(), PurchasePrice = vtSystem.Saleprice4, PurchasePriceBeginTime = (short)(vtSystem.SaletimeS4 % 24), PurchasePriceEndTime = (short)(vtSystem.SaletimeE4 % 24) },
																new Sys_PurchasePrice { PurchasePriceCreateTime = DateTime.Now, PurchasePriceDate = DateTime.Now.Date, PurchasePriceDeleteFlag = 0, PurchasePriceSn = vtSystem.SysSn, Key = Guid.NewGuid(), PurchasePrice = vtSystem.Saleprice5, PurchasePriceBeginTime = (short)(vtSystem.SaletimeS5 % 24), PurchasePriceEndTime = (short)(vtSystem.SaletimeE5 % 24) },
																new Sys_PurchasePrice { PurchasePriceCreateTime = DateTime.Now, PurchasePriceDate = DateTime.Now.Date, PurchasePriceDeleteFlag = 0, PurchasePriceSn = vtSystem.SysSn, Key = Guid.NewGuid(), PurchasePrice = vtSystem.Saleprice6, PurchasePriceBeginTime = (short)(vtSystem.SaletimeS6 % 24), PurchasePriceEndTime = (short)(vtSystem.SaletimeE6 % 24) },
																new Sys_PurchasePrice { PurchasePriceCreateTime = DateTime.Now, PurchasePriceDate = DateTime.Now.Date, PurchasePriceDeleteFlag = 0, PurchasePriceSn = vtSystem.SysSn, Key = Guid.NewGuid(), PurchasePrice = vtSystem.Saleprice7, PurchasePriceBeginTime = (short)(vtSystem.SaletimeS7 % 24), PurchasePriceEndTime = (short)(vtSystem.SaletimeE7 % 24) }
												};

					_sysPurchasePriceRepository.AddBulk(newPurchasePrice);
					string t = string.Format("{0}-{7},{1}-{8},{2}-{9},{3}-{10},{4}-{11},{5}-{12},{6}-{13},{7}-{14}", vtSystem.SaletimeS0, vtSystem.SaletimeS1, vtSystem.SaletimeS2, vtSystem.SaletimeS3, vtSystem.SaletimeS4, vtSystem.SaletimeS5, vtSystem.SaletimeS6, vtSystem.SaletimeS7, vtSystem.SaletimeE0, vtSystem.SaletimeE1, vtSystem.SaletimeE2, vtSystem.SaletimeE3, vtSystem.SaletimeE4, vtSystem.SaletimeE5, vtSystem.SaletimeE6, vtSystem.SaletimeE7);
					string p = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", (vtSystem.Saleprice0.HasValue ? vtSystem.Saleprice0.Value.ToString() : "Null"), (vtSystem.Saleprice1.HasValue ? vtSystem.Saleprice1.Value.ToString() : "Null"), (vtSystem.Saleprice2.HasValue ? vtSystem.Saleprice2.Value.ToString() : "Null"), (vtSystem.Saleprice3.HasValue ? vtSystem.Saleprice3.Value.ToString() : "Null"), (vtSystem.Saleprice4.HasValue ? vtSystem.Saleprice4.Value.ToString() : "Null"), (vtSystem.Saleprice5.HasValue ? vtSystem.Saleprice5.Value.ToString() : "Null"), (vtSystem.Saleprice6.HasValue ? vtSystem.Saleprice6.Value.ToString() : "Null"), (vtSystem.Saleprice7.HasValue ? vtSystem.Saleprice7.Value.ToString() : "Null"));
					//AddUserLog(vtSystem.SysSn, string.Format("add purchase price sn: {0}, times:{1}, prices:{2}, add result: {3}", vtSystem.SysSn, t, p, addResult.ToString()), LogType.System);
				}
				//}
				#endregion
			}
			catch (DbEntityValidationException dbexc)
			{
				//if (dbexc.EntityValidationErrors != null && dbexc.EntityValidationErrors.Any())
				//{
				//	foreach(var itm in dbexc.EntityValidationErrors)

				//}
				throw dbexc;
			}
			catch (Exception exc)
			{ /*log.Error("SendCommand", exc);*/
				throw exc;
			}

			try
			{
				_alphaRemotingService.SendCommand(vtSystem.SysSn, "SetConfig", new object[] { });
				if (workmodeChanged)
					_alphaRemotingService.SendCommand(vtSystem.SysSn, "ChangeWorkMode", new object[] { });
			}
			catch { }
			//SendInsuranceMail(vtSystem);

			//AddUserLog(vtSystem.SysSn, changelog, LogType.System, LoginUser.Username);
			//}
			//catch (Exception exc)
			//{
			//	result = false;
			//	//log.Error("error occured in update vtSystem", exc);
			//	//if (exc.InnerException != null)
			//	//	log.Error("error occured in update vtSystem", exc.InnerException);
			//}

			return result;
		}

		public PaginatedList<VT_SYSTEM> GetSystemBySharer(int pageIndex, int pageSize, string shareSn, Guid companyId)
		{
			var systems = new List<VT_SYSTEM>();
			systems.Add(GetSystemBySn(shareSn, companyId));

			return systems.ToPaginatedList(pageIndex, pageSize);
		}

		public PaginatedList<VT_SYSTEM> QueryVtSystemForReseller(Guid? companyId, string resellerId, int pageIndex, int pageSize)
		{
			int total = 0;
			var lst = _systemRepository.QueryVtSystemForReseller(companyId, resellerId, pageIndex, pageSize, out total);
			if (lst != null)
			{
				foreach (var itm in lst)
					itm.ListWeatherForecast = GetRecentThreeDaysSysWeatherForecastBySn(itm.SysSn);
			}
			return lst.ToPaginatedList(pageIndex, pageSize, total);
		}
		/// <summary>
		/// 获取指定sn的系统状态
		/// </summary>
		/// <param name="sn">sn</param>
		/// <returns>SystemState</returns>
		//public SystemState GetSystemStatus(string sn)
		//{
		//	if (string.IsNullOrWhiteSpace(sn))
		//		return null;

		//	var result = new SystemState() { Sn = sn, State = "fault" };
		//	var coldata = _coldataRepository.GetAll().Where(x => x.SYS_SN == sn && x.DELETE_FLAG == 0 /*&& x.FACTORY_FLAG == 0*/).OrderByDescending(x => x.UPLOAD_DATETIME)
		//		.Select(x => new { x.CREATE_DATETIME, x.EmsStatus }).FirstOrDefault();
		//	if (coldata != null)
		//	{
		//		result.NetWorkStatus = ((DateTime.Now - coldata.CREATE_DATETIME.Value).TotalMinutes < 30) ? 1 : 0;
		//		if (result.NetWorkStatus == 1)
		//			result.State = coldata.EmsStatus;
		//	}
		//	return result;
		//}

		/// <summary>
		/// 获取指定sn功率数据
		/// </summary>
		/// <param name="sn">sn</param>
		/// <param name="date">日期</param>
		/// <returns>PowerData实例</returns>
		public PowerReportData GetPowerDataBySn(string sn, DateTime date, Guid companyId)
		{
			if (string.IsNullOrWhiteSpace(sn))
				throw new ArgumentException("parameter cannot be null", "sn");

			decimal cobatV = 0, systemGridType = 0;
			var system = _systemRepository.GetSystemBySn(sn, companyId);
			if (system != null)
			{
				cobatV = system.Cobat.HasValue ? system.Cobat.Value : 0;
				systemGridType = system.GridType ?? 1;
			}
			var istoday = false;
			DateTime today = DateTime.Now;
			if (system.LastUploadTime.HasValue && system.LastUploadTimeLocal.HasValue)
			{
				// 当前时间+时差
				//today = DateTime.Now.AddHours((system.LastUploadTimeLocal.Value - system.LastUploadTime.Value).Hours);
				today = DateTime.Now - (system.LastUploadTime.Value - system.LastUploadTimeLocal.Value);
				istoday = date.Date == today.Date;
			}

			var reportData = istoday
				? ReportPowerDataFiveMinuteIntervals(_reportPowerRepository.GetTodayReportPower(sn, date, systemGridType))
				: ReportPowerDataFiveMinuteIntervals(_reportPowerRepository.GetByDate(sn, date, 0));
			Report_Energy energyData = null;

			decimal[] ppvs = new decimal[289], dieselPvs = new decimal[289], cbats = new decimal[289], usePowers = new decimal[289], feedIns = new decimal[289], gridCharge = new decimal[289];
			DateTime[] createTime = new DateTime[289];
			int iMaxIdx = 0;

			if (reportData != null && reportData.Any())
			{
				#region reportData 处理
				int iLastIdx = 0;
				var orderedColdata = reportData.OrderBy(c => c.TimelineIndex);
				var minDevStat = orderedColdata.First();
				var maxDevStat = orderedColdata.Last();

				iMaxIdx = maxDevStat.TimelineIndex / 5;

				if (iMaxIdx > 288)
					iMaxIdx = 288;

				int iLastTime = minDevStat.TimelineIndex;
				int iMinSpan;
				int idx = 0;
				foreach (var x in reportData)
				{
					iMinSpan = x.TimelineIndex - iLastTime;
					iLastTime = x.TimelineIndex;

					if (iMinSpan * 60 > 290 && iMinSpan * 60 <= 480 && idx != 0)
						idx = ++iLastIdx;
					else
						idx = x.TimelineIndex / 5;

					if (idx > iMaxIdx)
						break;

					ppvs[idx] = x.Ppv.HasValue ? x.Ppv.Value : 0;
					feedIns[idx] = x.FeedIn.HasValue ? x.FeedIn.Value : 0;
					dieselPvs[idx] = x.DieselPv.HasValue ? x.DieselPv.Value : 0;
					cbats[idx] = x.Cbat.HasValue ? x.Cbat.Value : 0;
					usePowers[idx] = x.UsePower.HasValue ? x.UsePower.Value : 0;
					gridCharge[idx] = x.GridCharge.HasValue ? x.GridCharge.Value : 0;

					createTime[idx] = x.TheDate.AddMinutes(x.TimelineIndex);
					iLastIdx = idx;
				}

				if (systemGridType <= 0)
					//如果(PFeed-In(k-1) == 0)且( PFeed-In(k+1) == 0),则PFeed-In(k) = 0
					for (int feedInIndex = 1; feedInIndex < feedIns.Length - 1; feedInIndex++)
						if (feedIns[feedInIndex - 1] == 0 && feedIns[feedInIndex + 1] == 0)
							feedIns[feedInIndex] = 0;

				//如果(Pgridcharge (k-1) == 0)且( Pgridcharge (k+1) == 0)，则Pgridcharge (k) = 0
				if (gridCharge[1] <= 0 || gridCharge[0] < 100)
					gridCharge[0] = 0;

				for (int gridChargeIndex = 1; gridChargeIndex < gridCharge.Length - 1; gridChargeIndex++)
				{
					if ((gridCharge[gridChargeIndex - 1] <= 0 && gridCharge[gridChargeIndex + 1] <= 0) || gridCharge[gridChargeIndex] < 100)
						gridCharge[gridChargeIndex] = 0;
				}
				if (gridCharge[gridCharge.Length - 2] <= 0 || gridCharge[gridCharge.Length - 1] < 100)
					gridCharge[gridCharge.Length - 1] = 0;

				#region 大于6填充6个数据
				int markCount = 0;   //SOC丢失处理方案，值为0:不处理或求平均；大于1小于6:掉1-5个数据，取最后上一个SOC值；大于6:填充6个SOC数据；

				for (int i = 1; i < iMaxIdx; i++)
					if (cbats[i] == 0)
						if (cbats[i - 1] != 0 && i + 1 < iMaxIdx && cbats[i + 1] != 0)
						{
							cbats[i] = ((cbats[i - 1] + cbats[i + 1]) * 0.5m);
							markCount = 0;
						}
						else
						{
							if (markCount < 6)
							{
								cbats[i] = cbats[i - 1];
								markCount++;
							}
						}
					else
						markCount = 0;
				#endregion

				#region 消除锯齿,当SOC值小于等于10时，判断前后两个值是否相等，如果是则当前值也等于前面那个值
				for (int i = 1; i < iMaxIdx - 1; i++)
					if (cbats[i] <= 10 && cbats[i - 1] == cbats[i + 1])
						cbats[i] = cbats[i - 1];
				#endregion

				cbats[288] = cbats[287];

				var lstEnergyData = istoday ? _reportEnergyRepository.GetTodayEnergyReport(sn, date, cobatV) : _reportEnergyRepository.GetReportEnergyByPeriod(sn, date, date, 0);
				if (lstEnergyData != null && lstEnergyData.Any())
				{
					energyData = new Report_Energy();
					energyData.Ebat = lstEnergyData.Sum(x => x.Ebat);
					energyData.Echarge = lstEnergyData.Sum(x => x.Echarge);
					energyData.Ediesel = lstEnergyData.Sum(x => x.Ediesel);
					energyData.Eeff = lstEnergyData.Sum(x => x.Eeff);
					energyData.EGrid2Load = lstEnergyData.Sum(x => x.EGrid2Load);
					energyData.EGridCharge = lstEnergyData.Sum(x => x.EGridCharge);
					energyData.Einput = lstEnergyData.Sum(x => x.Einput);
					energyData.Eload = lstEnergyData.Sum(x => x.Eload);
					energyData.Eoutput = lstEnergyData.Sum(x => x.Eoutput);
					//app取错值了,api修正
					energyData.Epv2Load = lstEnergyData.Sum(x => x.Eeff);
					energyData.Epvtotal = lstEnergyData.Sum(x => x.Epvtotal);
					//energyData.EselfConsumption = lstEnergyData.Sum(x => x.EselfConsumption);
					//energyData.EselfSufficiency = lstEnergyData.Sum(x => x.EselfSufficiency);
					energyData.Soc = lstEnergyData.OrderBy(x => x.TheDate).Last().Soc;
				}
				#endregion
			}

			var times = new string[289];
			for (int i = 0; i < 289; i++)
			{
				var h = i / 12;
				var m = (i * 5) % 60;
				times[i] = h + ":" + (m < 10 ? "0" + m : m.ToString());
			}

			if (iMaxIdx > 1 && cbats[iMaxIdx] == 0 && usePowers[iMaxIdx] == 0)
				iMaxIdx--;

			var result = new PowerReportData
			{
				Time = times,
				Ppv = ppvs,
				UsePower = usePowers,
				Cbat = cbats,
				FeedIn = feedIns,
				GridCharge = gridCharge,
				//DieselPv = dieselPvs,
				//PowerSource = systemPowerSource,
				LastIndex = iMaxIdx
			};
			if (energyData != null)
			{
				result.EBat = energyData.Ebat.HasValue ? decimal.Round(energyData.Ebat.Value, 2) : 0;
				result.EPvTotal = energyData.Epvtotal.HasValue ? decimal.Round(energyData.Epvtotal.Value, 2) : 0;
				result.ELoad = energyData.Eload.HasValue ? decimal.Round(energyData.Eload.Value, 2) : 0;
				result.ECharge = energyData.Echarge.HasValue ? decimal.Round(energyData.Echarge.Value, 2) : 0;
				result.EGridCharge = energyData.Einput.HasValue ? decimal.Round(energyData.Einput.Value, 2) : 0;
				result.EFeedIn = energyData.Eoutput.HasValue ? decimal.Round(energyData.Eoutput.Value, 2) : 0;
				//result.EInput = energyData.Einput.HasValue ? energyData.Einput.Value : 0;
				//result.Soc = energyData.Soc.HasValue ? energyData.Soc.Value : 0;
				//result.EDiesel = energyData.Ediesel.HasValue ? energyData.Ediesel.Value : 0;
			}

			return result;
		}

		///// <summary>
		///// 检查coldata
		///// </summary>
		///// <param name="coldatas"></param>
		///// <returns>过滤完之后的IList&lt;VT_COLDATA&gt;</returns>
		//private static IList<VT_COLDATA> CheckVtColdata(IList<VT_COLDATA> coldatas)
		//{
		//	IList<VT_COLDATA> coldata = new List<VT_COLDATA>();

		//	if (coldatas != null && coldatas.Any(x => x.FACTORY_FLAG == 0))
		//	{
		//		var releaseColdata = coldatas.Where(x => x.FACTORY_FLAG == 0).ToList();
		//		var first = releaseColdata.First();
		//		coldata.Add(first);

		//		var prevUploadtime = first.UPLOAD_DATETIME.Value;
		//		DateTime currentUploadtime;
		//		for (var i = 1; i < releaseColdata.Count(); i++)
		//		{
		//			var cd = releaseColdata[i];
		//			currentUploadtime = cd.UPLOAD_DATETIME.Value;
		//			var ts = (currentUploadtime - prevUploadtime).TotalSeconds;
		//			if (ts < 290)
		//			{
		//				coldata.RemoveAt(coldata.Count - 1);
		//			}
		//			coldata.Add(cd);

		//			prevUploadtime = currentUploadtime;
		//		}
		//	}

		//	return coldata;
		//}

		/// <summary>
		/// 根据用户名获取功率数据
		/// </summary>
		/// <param name="username">用户名</param>
		/// <param name="date">日期</param>
		/// <returns>PowerData实例</returns>
		public PowerReportData GetPowerDataByUser(string username, DateTime date, Guid companyId)
		{
			if (string.IsNullOrWhiteSpace(username))
				throw new ArgumentException("parameter cannot be null", "username");

			var systems = GetSystemByUser(1, int.MaxValue, username, companyId);

			var result = new PowerReportData();

			if (systems != null && systems.Count > 0)
			{
				decimal cobatSum = 0;
				for (var i = 0; i < systems.Count; i++)
				{
					decimal cobat = (systems[i].Cobat.HasValue ? systems[i].Cobat.Value : 0);
					cobatSum += cobat;
					if (i == 0)
					{
						result = GetPowerDataBySn(systems[i].SysSn, date, companyId);
						if (systems.Count > 1)
							for (var j = 0; j < result.Cbat.Length; j++)
							{
								result.Cbat[j] = result.Cbat[j] * cobat;
							}
					}
					else
					{
						var sticItem = GetPowerDataBySn(systems[i].SysSn, date, companyId);

						result.EBat += sticItem.EBat;
						result.ECharge += sticItem.ECharge;
						result.EFeedIn += sticItem.EFeedIn;
						result.EGridCharge += sticItem.EGridCharge;
						result.ELoad += sticItem.ELoad;
						result.EPvTotal += sticItem.EPvTotal;
						for (var j = 0; j < result.Cbat.Length; j++)
						{
							result.Cbat[j] += sticItem.Cbat[j] * cobat;
							result.FeedIn[j] += sticItem.FeedIn[j];
							result.GridCharge[j] += sticItem.GridCharge[j];
							result.Ppv[j] += sticItem.Ppv[j];
							result.UsePower[j] += sticItem.UsePower[j];
						}
						result.Time = (sticItem.Time.Length > result.Time.Length ? sticItem.Time : result.Time);
					}
				}
				if (systems.Count > 1 && cobatSum > 0)
				{
					for (var i = 0; i < result.Cbat.Length; i++)
					{
						result.Cbat[i] = result.Cbat[i] / cobatSum;
					}
					result.EBat = result.EBat / cobatSum;
				}
			}
			return result;
		}

		/// <summary>
		/// 获取指定sn能量数据
		/// </summary>
		/// <param name="sn">sn</param>
		/// <param name="start">开始日期</param>
		/// <param name="end">结束日期</param>
		/// <returns>EnergyDate实例</returns>
		public EnergyReportData GetEnergeDataBySn(string sn, DateTime? start, DateTime? end, Guid companyId, string statisticsby)
		{
			if (string.IsNullOrWhiteSpace(sn))
				throw new ArgumentException("parameter cannot be null", "sn");

			var system = _systemRepository.GetSystemBySn(sn, companyId);
			EnergyReportData result = new EnergyReportData();
			if (system != null)
			{
				decimal cobatV = system.Cobat.HasValue ? system.Cobat.Value : 0;
				var startDate = start.HasValue ? start.Value : (system.CreateDatetime.HasValue ? new DateTime(system.CreateDatetime.Value.Year, 1, 1) : new DateTime(DateTime.Now.Year, 1, 1));
				var endDate = end.HasValue ? end.Value.AddDays(1).AddMinutes(-1) : DateTime.Now.AddDays(1).AddMinutes(-1);
				DateTime localNow = DateTime.Now;
				List<Report_Energy> energyData = null;
				if (CheckTodayIsWithinRange(system.LastUploadTime, system.LastUploadTimeLocal, startDate, endDate, out localNow))
				{
					energyData = _reportEnergyRepository.GetReportEnergyByPeriod(sn, startDate, localNow.Date.AddSeconds(-1), 0);
					var todayEnergyData = _reportEnergyRepository.GetTodayEnergyReport(sn, localNow.Date, cobatV);
					if (todayEnergyData != null && todayEnergyData.Any())
					{
						if (energyData != null)
							energyData.AddRange(todayEnergyData);
						else
							energyData = todayEnergyData;
					}
				}
				else
					energyData = _reportEnergyRepository.GetReportEnergyByPeriod(sn, startDate, endDate, 0);

				if (energyData != null && energyData.Any())
				{
					switch (statisticsby)
					{
						case "year":
							{
								var startYear = startDate.Year;
								var endYear = endDate.Year;
								if (startYear <= endYear)
								{
									result.Init(endYear - startYear + 1);
									for (var y = startYear; y <= endYear; y++)
									{
										var ed = energyData.Where(x => x.TheDate.Year == y);
										decimal? eBatT = 0, echargeT = 0, epv = 0, eoutput = 0, einput = 0, egridcharge = 0, egrid2load = 0;
										eBatT = ed.Sum(x => x.Ebat) ?? 0;
										echargeT = ed.Sum(x => x.Echarge) ?? 0;
										epv = ed.Sum(x => x.Epvtotal) ?? 0;
										eoutput = ed.Sum(x => x.Eoutput) ?? 0;
										einput = ed.Sum(x => x.Einput) ?? 0;
										egridcharge = ed.Sum(x => x.EGridCharge) ?? 0;
										egrid2load = ed.Sum(x => x.EGrid2Load) ?? 0;
										//soc = ed.OrderBy(x => x.TheDate).Last().Soc;
										//eDiesel = ed.Sum(x => x.Ediesel);
										result.Ebat[y - startYear] = eBatT ?? 0;
										result.Echarge[y - startYear] = echargeT ?? 0;
										result.Eeff[y - startYear] = ed.Sum(x => x.Eeff) ?? 0;//(epv - eoutput) > 0 ? (epv - eoutput).Value : 0;
																																					//app取错值了,api修正
																																					//自给自足能量
										result.Epv2load[y - startYear] = (epv - eoutput) ?? 0;//ed.Sum(x => x.Epv2Load) ?? 0;//(epv - eoutput - echargeT) > 0 ? (epv - eoutput - echargeT).Value : 0;
										result.Eout[y - startYear] = eoutput ?? 0;
										result.EpvT[y - startYear] = epv ?? 0;
										result.Eload[y - startYear] = ed.Sum(x => x.Eload) ?? 0;//(epv + einput - eoutput) > 0 ? (epv + einput - eoutput).Value : 0;
										result.EGridCharge[y - startYear] = egridcharge ?? 0;
										result.EGrid2Load[y - startYear] = egrid2load ?? 0;
										result.Einput[y - startYear] = einput ?? 0;

										result.Timeline[y - startYear] = y.ToString();
									}
									if (result.EpvT.Sum() > 0)
									{
										result.EselfConsumption = decimal.Round(((result.EpvT.Sum() - result.Eout.Sum()) / result.EpvT.Sum()), 2);
									}
									if (result.EpvT.Sum() + result.Einput.Sum() - result.Eout.Sum() > 0)
										result.EselfSufficiency = decimal.Round(((result.EpvT.Sum() - result.Eout.Sum()) / (result.EpvT.Sum() + result.Einput.Sum() - result.Eout.Sum())), 2);

									if (result.EselfConsumption < 0)
										result.EselfConsumption = 0;
									else if (result.EselfConsumption > 1)
										result.EselfConsumption = 1;

									if (result.EselfSufficiency < 0)
										result.EselfSufficiency = 0;
									else if (result.EselfSufficiency > 1)
										result.EselfSufficiency = 1;
								}
							}
							break;
						case "month":
							{
								if (startDate <= endDate)
								{
									var totalMonth = (endDate.Year - startDate.Year) * 12 + endDate.Month - startDate.Month + 1;
									result.Init(totalMonth);
									var startM = new DateTime(startDate.Year, startDate.Month, 1);
									var endM = new DateTime(endDate.Year, endDate.Month, endDate.Day);
									var index = 0;
									while (startM <= endM)
									{
										var ed = energyData.Where(x => x.TheDate > startM && x.TheDate < startM.AddMonths(1));
										decimal? eBatT = 0, echargeT = 0, epv = 0, eoutput = 0, einput = 0, egridcharge = 0, egrid2load = 0;
										eBatT = ed.Sum(x => x.Ebat) ?? 0;
										echargeT = ed.Sum(x => x.Echarge) ?? 0;
										epv = ed.Sum(x => x.Epvtotal) ?? 0;
										eoutput = ed.Sum(x => x.Eoutput) ?? 0;
										einput = ed.Sum(x => x.Einput) ?? 0;
										egridcharge = ed.Sum(x => x.EGridCharge) ?? 0;
										egrid2load = ed.Sum(x => x.EGrid2Load) ?? 0;
										//soc = ed.OrderBy(x => x.TheDate).Last().Soc;
										//eDiesel = ed.Sum(x => x.Ediesel);
										result.Ebat[index] = eBatT ?? 0;
										result.Echarge[index] = echargeT ?? 0;
										result.Eeff[index] = ed.Sum(x => x.Eeff) ?? 0;//(epv - eoutput) > 0 ? (epv - eoutput).Value : 0;
																																	//app取错值了,api修正
																																	//自给自足能量
										result.Epv2load[index] = (epv - eoutput) ?? 0;//ed.Sum(x => x.Epv2Load) ?? 0;//(epv - eoutput - echargeT) > 0 ? (epv - eoutput - echargeT).Value : 0;
										result.Eout[index] = eoutput ?? 0;
										result.EpvT[index] = epv ?? 0;
										result.Eload[index] = ed.Sum(x => x.Eload) ?? 0;//(epv + einput - eoutput) > 0 ? (epv + einput - eoutput).Value : 0;
										result.EGridCharge[index] = egridcharge ?? 0;
										result.EGrid2Load[index] = egrid2load ?? 0;
										result.Einput[index] = einput ?? 0;

										result.Timeline[index] = startM.ToString("yyyy-MM");

										startM = startM.AddMonths(1);
										index++;
									}
									if (result.EpvT.Sum() > 0)
									{
										result.EselfConsumption = decimal.Round(((result.EpvT.Sum() - result.Eout.Sum()) / result.EpvT.Sum()), 2);
									}
									if (result.EpvT.Sum() + result.Einput.Sum() - result.Eout.Sum() > 0)
										result.EselfSufficiency = decimal.Round(((result.EpvT.Sum() - result.Eout.Sum()) / (result.EpvT.Sum() + result.Einput.Sum() - result.Eout.Sum())), 2);

									if (result.EselfConsumption < 0)
										result.EselfConsumption = 0;
									else if (result.EselfConsumption > 1)
										result.EselfConsumption = 1;

									if (result.EselfSufficiency < 0)
										result.EselfSufficiency = 0;
									else if (result.EselfSufficiency > 1)
										result.EselfSufficiency = 1;
								}
							}
							break;
						case "day":
							{
								var totalDays = (int)(endDate - startDate).TotalDays + 1;
								result.Init(totalDays);
								var startD = startDate;
								var index = 0;

								while (startD <= endDate)
								{
									var ed = energyData.Where(x => x.TheDate == startD);
									decimal? eBatT = 0, echargeT = 0, epv = 0, eoutput = 0, einput = 0, egridcharge = 0, egrid2load = 0;
									eBatT = ed.Sum(x => x.Ebat) ?? 0;
									echargeT = ed.Sum(x => x.Echarge) ?? 0;
									epv = ed.Sum(x => x.Epvtotal) ?? 0;
									eoutput = ed.Sum(x => x.Eoutput) ?? 0;
									einput = ed.Sum(x => x.Einput) ?? 0;
									egridcharge = ed.Sum(x => x.EGridCharge) ?? 0;
									egrid2load = ed.Sum(x => x.EGrid2Load) ?? 0;
									result.Ebat[index] = eBatT ?? 0;
									result.Echarge[index] = echargeT ?? 0;
									result.Eeff[index] = ed.Sum(x => x.Eeff) ?? 0;//(epv - eoutput) > 0 ? (epv - eoutput).Value : 0;
																																				//app取错值了,api修正
																																				//自给自足能量
									result.Epv2load[index] = (epv - eoutput) ?? 0;//energyData.Sum(x => x.Epv2Load) ?? 0;//(epv - eoutput - echargeT) > 0 ? (epv - eoutput - echargeT).Value : 0;
									result.Eout[index] = eoutput ?? 0;
									result.EpvT[index] = epv ?? 0;
									result.Eload[index] = ed.Sum(x => x.Eload) ?? 0;//(epv + einput - eoutput) > 0 ? (epv + einput - eoutput).Value : 0;
									result.EGridCharge[index] = egridcharge ?? 0;
									result.EGrid2Load[index] = egrid2load ?? 0;
									result.Einput[index] = einput ?? 0;

									result.Timeline[index] = startD.ToString("yyyy-MM-dd");

									startD = startD.AddDays(1);
									index++;
								}
								if (result.EpvT.Sum() > 0)
								{
									result.EselfConsumption = decimal.Round(((result.EpvT.Sum() - result.Eout.Sum()) / result.EpvT.Sum()), 2);
								}
								if (result.EpvT.Sum() + result.Einput.Sum() - result.Eout.Sum() > 0)
									result.EselfSufficiency = decimal.Round(((result.EpvT.Sum() - result.Eout.Sum()) / (result.EpvT.Sum() + result.Einput.Sum() - result.Eout.Sum())), 2);

								if (result.EselfConsumption < 0)
									result.EselfConsumption = 0;
								else if (result.EselfConsumption > 1)
									result.EselfConsumption = 1;

								if (result.EselfSufficiency < 0)
									result.EselfSufficiency = 0;
								else if (result.EselfSufficiency > 1)
									result.EselfSufficiency = 1;
							}
							break;
					}
				}
				else
				{
					switch (statisticsby)
					{
						case "year":
							{
								var startYear = startDate.Year;
								var endYear = endDate.Year;
								if (startYear <= endYear)
								{
									result.Init(endYear - startYear + 1);
									for (var y = startYear; y <= endYear; y++)
									{
										//result.Ebat[y - startYear] = 0;
										//result.Echarge[y - startYear] = 0;
										//result.Eeff[y - startYear] = 0;
										//result.Epv2load[y - startYear] = 0;
										//result.Eout[y - startYear] = 0;
										//result.EpvT[y - startYear] = 0;
										//result.Eload[y - startYear] = 0;
										//result.EGridCharge[y - startYear] = 0;
										//result.EGrid2Load[y - startYear] = 0;
										//result.Einput[y - startYear] = 0;

										result.Timeline[y - startYear] = y.ToString();
									}
									result.EselfConsumption = 0;
									result.EselfSufficiency = 0;
								}
							}
							break;
						case "month":
							{
								if (startDate <= endDate)
								{
									var totalMonth = (endDate.Year - startDate.Year) * 12 + endDate.Month - startDate.Month + 1;
									result.Init(totalMonth);
									var startM = new DateTime(startDate.Year, startDate.Month, 1);
									var endM = new DateTime(endDate.Year, endDate.Month, endDate.Day);
									var index = 0;
									while (startM <= endM)
									{
										//result.Ebat[index] = 0;
										//result.Echarge[index] = 0;
										//result.Eeff[index] = 0;
										//result.Epv2load[index] = 0;
										//result.Eout[index] = 0;
										//result.EpvT[index] = 0;
										//result.Eload[index] = 0;
										//result.EGridCharge[index] = 0;
										//result.EGrid2Load[index] = 0;
										//result.Einput[index] = 0;

										result.Timeline[index] = startM.ToString("yyyy-MM");

										startM = startM.AddMonths(1);
										index++;
									}
									result.EselfConsumption = 0;
									result.EselfSufficiency = 0;
								}
							}
							break;
						case "day":
							{
								var totalDays = (int)(endDate - startDate).TotalDays + 1;
								result.Init(totalDays);
								var startD = startDate;
								var index = 0;

								while (startD <= endDate)
								{
									//result.Ebat[index] = 0;
									//result.Echarge[index] = 0;
									//result.Eeff[index] = 0;
									//result.Epv2load[index] = 0;
									//result.Eout[index] = 0;
									//result.EpvT[index] = 0;
									//result.Eload[index] = 0;
									//result.EGridCharge[index] = 0;
									//result.EGrid2Load[index] = 0;
									//result.Einput[index] = 0;

									result.Timeline[index] = startD.ToString("yyyy-MM-dd");

									startD = startD.AddDays(1);
									index++;
								}
								result.EselfConsumption = 0;
								result.EselfSufficiency = 0;
							}
							break;
					}
				}
			}

			return result;
		}

		/// <summary>
		/// 根据用户名获取能量数据
		/// </summary>
		/// <param name="username">用户名</param>
		/// <param name="start">开始日期</param>
		/// <param name="end">结束日期</param>
		/// <returns>EnergyDate实例</returns>
		public EnergyReportData GetEnergeDataByUser(string username, DateTime? start, DateTime? end, Guid companyId, string statisticsby)
		{
			if (string.IsNullOrWhiteSpace(username))
				throw new ArgumentException("parameter cannot be null", "username");

			var systems = GetSystemByUser(1, int.MaxValue, username, companyId);

			EnergyReportData result = new EnergyReportData();
			if (systems != null && systems.Count > 0)
			{
				IList<EnergyReportData> eds = new List<EnergyReportData>();
				foreach (var item in systems)
				{
					var ed = GetEnergeDataBySn(item.SysSn, start, end, companyId, statisticsby);
					if (ed != null)
						eds.Add(ed);
				}

				result = eds[0];
				if (eds.Count > 1)
				{
					for (var i = 1; i < eds.Count; i++)
					{
						for (var j = 0; j < eds[i].Timeline.Length; j++)
						{
							result.Ebat[j] += eds[i].Ebat[j];
							result.Echarge[j] += eds[i].Echarge[j];
							result.Eeff[j] += eds[i].Eeff[j];
							result.EGrid2Load[j] += eds[i].EGrid2Load[j];
							result.EGridCharge[j] += eds[i].EGridCharge[j];
							result.Einput[j] += eds[i].Einput[j];
							result.Eload[j] += eds[i].Eload[j];
							result.Eout[j] += eds[i].Eout[j];
							result.Epv2load[j] += eds[i].Epv2load[j];
							result.EpvT[j] += eds[i].EpvT[j];
							//result.EselfConsumption += eds[i].EselfConsumption;
							//result.EselfSufficiency += item.EselfSufficiency;
						}
					}

					if (result.EpvT.Sum() > 0)
						result.EselfConsumption = decimal.Round((result.EpvT.Sum() - result.Eout.Sum()) / result.EpvT.Sum(), 2);
					if (result.Eload.Sum() > 0)
						result.EselfSufficiency = decimal.Round((result.EpvT.Sum() - result.Eout.Sum()) / (result.Eload.Sum()), 2);

					if (result.EselfConsumption < 0)
						result.EselfConsumption = 0;
					else if (result.EselfConsumption > 1)
						result.EselfConsumption = 1;

					if (result.EselfSufficiency < 0)
						result.EselfSufficiency = 0;
					else if (result.EselfSufficiency > 1)
						result.EselfSufficiency = 1;
				}
			}

			return result;
		}

		public ProfitReportData GetProfitDataBySn(string sn, DateTime? start, DateTime? end, Guid companyId, string statisticsby)
		{
			if (string.IsNullOrWhiteSpace(sn))
				throw new ArgumentException("parameter cannot be null", "sn");

			var system = _systemRepository.GetSystemBySn(sn, companyId);
			var result = new ProfitReportData();

			if (system != null)
			{
				//result.InputCost = system.Inputcost ?? 0;
				var startDate = start.HasValue ? start.Value : (system.CreateDatetime.HasValue ? new DateTime(system.CreateDatetime.Value.Year, 1, 1) : new DateTime(DateTime.Now.Year, 1, 1));
				var endDate = end.HasValue ? end.Value.AddDays(1).AddMinutes(-1) : DateTime.Now.AddDays(1).AddMinutes(-1);
				DateTime localNow = DateTime.Now;
				List<Report_Income> incomeData = null;
				if (CheckTodayIsWithinRange(system.LastUploadTime, system.LastUploadTimeLocal, startDate, endDate, out localNow))
				{
					incomeData = _reportIncomeRepository.GetReportIncomeByPeriod(sn, startDate, localNow.Date.AddSeconds(-1), 0);
					var todayIncomeData = _reportIncomeRepository.GetTodayIncomeReport(sn, localNow.Date);
					if (todayIncomeData != null && todayIncomeData.Any())
					{
						if (incomeData != null)
							incomeData.AddRange(todayIncomeData);
						else
							incomeData = todayIncomeData;
					}
				}
				else
					incomeData = _reportIncomeRepository.GetReportIncomeByPeriod(sn, startDate, endDate, 0);

				if (incomeData != null && incomeData.Any())
				{
					switch (statisticsby)
					{
						case "year":
							{
								var startYear = startDate.Year;
								var endYear = endDate.Year;
								if (startYear <= endYear)
								{
									result.Init(endYear - startYear + 1);
									for (var y = startYear; y <= endYear; y++)
									{
										var ind = incomeData.Where(x => x.TheDate.Year == y);
										result.BuyIncome[y - startYear] = ind.Sum(x => x.BuyIncome) ?? 0;
										result.ChargeIncome[y - startYear] = ind.Sum(x => x.ChargeIncome) ?? 0;
										result.DemandCharge[y - startYear] = ind.Sum(x => x.DemandCharge) ?? 0;
										result.SellIncome[y - startYear] = ind.Sum(x => x.SellIncome) ?? 0;

										result.Timeline[y - startYear] = y.ToString();
									}
								}
							}
							break;
						case "month":
							{
								if (startDate <= endDate)
								{
									var totalMonth = (endDate.Year - startDate.Year) * 12 + endDate.Month - startDate.Month + 1;
									result.Init(totalMonth);
									var startM = new DateTime(startDate.Year, startDate.Month, 1);
									var endM = new DateTime(endDate.Year, endDate.Month, endDate.Day);
									var index = 0;
									while (startM <= endM)
									{
										var ind = incomeData.Where(x => x.TheDate > startM && x.TheDate < startM.AddMonths(1));
										result.BuyIncome[index] = ind.Sum(x => x.BuyIncome) ?? 0;
										result.ChargeIncome[index] = ind.Sum(x => x.ChargeIncome) ?? 0;
										result.DemandCharge[index] = ind.Sum(x => x.DemandCharge) ?? 0;
										result.SellIncome[index] = ind.Sum(x => x.SellIncome) ?? 0;

										result.Timeline[index] = startM.ToString("yyyy-MM");

										startM = startM.AddMonths(1);
										index++;
									}
								}
							}
							break;
						case "day":
							{
								var totalDays = (int)(endDate - startDate).TotalDays + 1;
								result.Init(totalDays);
								var startD = startDate;
								var index = 0;

								while (startD <= endDate)
								{
									result.BuyIncome[index] = incomeData.Sum(x => x.BuyIncome) ?? 0;
									result.ChargeIncome[index] = incomeData.Sum(x => x.ChargeIncome) ?? 0;
									result.DemandCharge[index] = incomeData.Sum(x => x.DemandCharge) ?? 0;
									result.SellIncome[index] = incomeData.Sum(x => x.SellIncome) ?? 0;

									result.Timeline[index] = startD.ToString("yyyy-MM-dd");

									startD = startD.AddDays(1);
									index++;
								}
							}
							break;
					}
				}
				result.TotalIncome = _systemRepository.CalculateTotalEarnings(sn);
				result.MoneyType = system.MoneyType;
				result.InputCost = system.Inputcost ?? 0;
			}

			return result;
		}

		public ProfitReportData GetProfitDataByUser(string username, DateTime? start, DateTime? end, Guid companyId, string statisticsby)
		{
			if (string.IsNullOrWhiteSpace(username))
				throw new ArgumentException("parameter cannot be null", "username");

			var systems = GetSystemByUser(1, int.MaxValue, username, companyId);

			ProfitReportData result = new ProfitReportData();
			if (systems != null && systems.Count > 0)
			{
				IList<ProfitReportData> prds = new List<ProfitReportData>();
				foreach (var item in systems)
				{
					var ed = GetProfitDataBySn(item.SysSn, start, end, companyId, statisticsby);
					if (ed != null)
						prds.Add(ed);
				}

				result = prds[0];
				if (prds.Count > 1)
				{
					result.Timeline = prds[0].Timeline;
					result.InputCost = prds.Sum(x => x.InputCost);
					result.TotalIncome = prds.Sum(x => x.TotalIncome);
					for (var i = 1; i < prds.Count; i++)
					{
						for (var j = 0; j < prds[i].Timeline.Length; j++)
						{
							result.BuyIncome[j] += prds[i].BuyIncome[j];
							result.ChargeIncome[j] += prds[i].ChargeIncome[j];
							result.DemandCharge[j] += prds[i].DemandCharge[j];
							result.SellIncome[j] += prds[i].SellIncome[j];
						}
					}
				}
			}

			return result;
		}
		/// <summary>
		/// 根据用户名获取系统列表
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="pageIndex">指定页</param>
		/// <param name="pageSize">每页最大记录数</param>
		/// <param name="username">用户名</param>
		/// <returns>OperationResult&lt;PaginatedList&lt;VT_SYSTEM&gt;&gt;实例</returns>
		public OperationResult<PaginatedList<VT_SYSTEM>> GetSystemByUser(string api_account, long timeStamp, string sign, string token, int pageIndex, int pageSize)
		{
			if (string.IsNullOrWhiteSpace(token))
				return new OperationResult<PaginatedList<VT_SYSTEM>>(OperationCode.Error_Param_Empty);

			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult<PaginatedList<VT_SYSTEM>>(OperationCode.Error_TimeStamp);

			if (!TokenHelper.CheckToken(token))
				return new OperationResult<PaginatedList<VT_SYSTEM>>(OperationCode.Error_TokenExpiration);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult<PaginatedList<VT_SYSTEM>>(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForGetSystemByUser(api_account, timeStamp, sign, pageIndex, pageSize, token, apiAccount.Api_SecretKey))
				return new OperationResult<PaginatedList<VT_SYSTEM>>(OperationCode.Error_Sign);

			var theToken = TokenHelper.GetToken(token);
			var theCompanyId = new Guid(apiAccount.CompanyId);
			var user = _userRepository.GetSingleByKey(theToken.UserId);

			if (user == null)
				return new OperationResult<PaginatedList<VT_SYSTEM>>(OperationCode.Error_UserNotExist);

			PaginatedList<VT_SYSTEM> systems = null;
			if (theToken.UserTypes.Contains("customer"))
			{
				systems = GetSystemByUser(pageIndex, pageSize, user.USERNAME, theCompanyId);
			}
			else if (theToken.UserTypes.Contains("installer"))
			{
				systems = GetSystemByInstaller(pageIndex, pageSize, user.Key, theCompanyId);
			}
			else if (theToken.UserTypes.Contains("sharer"))
			{
				systems = GetSystemBySharer(pageIndex, pageSize, user.Inviter, theCompanyId);
			}
			else if (theToken.UserTypes.Contains("reseller"))
			{
				systems = QueryVtSystemForReseller(theCompanyId, user.Key.ToString(), pageIndex, pageSize);
			}
			var plSystems = new OperationResult<PaginatedList<VT_SYSTEM>>(OperationCode.Success, systems);
			//plSystems.Entity = systems;

			return plSystems;
		}

		/// <summary>
		/// 获取系统状态
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="sns">sn号(多个用逗号分隔)</param>
		/// <returns>OperationResult&lt;IEnumerable&lt;SystemState&gt;&gt;实例</returns>
		public OperationResult<IEnumerable<SystemState>> GetSystemStatus(string api_account, long timeStamp, string sign, string sns, string token)
		{
			if (string.IsNullOrWhiteSpace(token))
				return new OperationResult<IEnumerable<SystemState>>(OperationCode.Error_Param_Empty);

			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult<IEnumerable<SystemState>>(OperationCode.Error_TimeStamp);

			if (!TokenHelper.CheckToken(token))
				return new OperationResult<IEnumerable<SystemState>>(OperationCode.Error_TokenExpiration);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult<IEnumerable<SystemState>>(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForGetSystemStatus(api_account, timeStamp, sign, token, sns, apiAccount.Api_SecretKey))
				return new OperationResult<IEnumerable<SystemState>>(OperationCode.Error_Sign);

			if (string.IsNullOrWhiteSpace(sns))
				return new OperationResult<IEnumerable<SystemState>>(OperationCode.Error_SNNotExist);

			var result = new OperationResult<IEnumerable<SystemState>>(OperationCode.Success);
			var arrSn = sns.Split(',');
			var systemStates = new List<SystemState>();
			var systems = _systemRepository.GetSystemBySns(arrSn);
			if (systems != null && systems.Any())
			{
				foreach (var s in systems)
				{
					var state = new SystemState() { Sn = s.SysSn, State = "fault", Minv = s.Minv };
					if (s.LastUploadTime.HasValue)
						state.NetWorkStatus = ((DateTime.Now - s.LastUploadTime.Value).TotalMinutes < 30) ? 1 : 0;
					if (state.NetWorkStatus == 1)
						state.State = s.EmsStatus;

					systemStates.Add(state);
				}
			}
			result.Entity = systemStates;

			return result;
		}

		/// <summary>
		/// 获取指定sn功率数据
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="sn">sn</param>
		/// <param name="username">用户名</param>
		/// <param name="date">日期</param>
		/// <returns>OperationResult&lt;PowerData&gt;实例</returns>
		public OperationResult<PowerReportData> GetPowerData(string api_account, long timeStamp, string sign, string sn, string username, string date, string token)
		{
			if (string.IsNullOrWhiteSpace(token))
				return new OperationResult<PowerReportData>(OperationCode.Error_Param_Empty);

			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult<PowerReportData>(OperationCode.Error_TimeStamp);

			if (!TokenHelper.CheckToken(token))
				return new OperationResult<PowerReportData>(OperationCode.Error_TokenExpiration);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult<PowerReportData>(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForGetPowerData(api_account, timeStamp, sign, sn, username, date, apiAccount.Api_SecretKey, token))
				return new OperationResult<PowerReportData>(OperationCode.Error_Sign);

			var theCompanyId = new Guid(apiAccount.CompanyId);
			DateTime dt = DateTime.Now;
			if (!DateTime.TryParse(date, out dt))
			{
				return new OperationResult<PowerReportData>(OperationCode.Error_Param);
			}

			if (!string.IsNullOrWhiteSpace(sn))
			{
				var pd = GetPowerDataBySn(sn, dt, theCompanyId);
				var result = new OperationResult<PowerReportData>(OperationCode.Success);
				result.Entity = pd;
				return result;
			}
			else if (!string.IsNullOrWhiteSpace(username))
			{
				var pd = GetPowerDataByUser(username, dt, theCompanyId);
				var result = new OperationResult<PowerReportData>(OperationCode.Success);
				result.Entity = pd;
				return result;
			}
			else
				return new OperationResult<PowerReportData>(OperationCode.Error_Param);
		}

		public OperationResult<PaginatedList<PowerData>> GetLastPowerData(string api_account, long timeStamp, string sign, string sn, string token)
		{
			if (string.IsNullOrWhiteSpace(token))
				return new OperationResult<PaginatedList<PowerData>>(OperationCode.Error_Param_Empty);

			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult<PaginatedList<PowerData>>(OperationCode.Error_TimeStamp);

			if (!TokenHelper.CheckToken(token))
				return new OperationResult<PaginatedList<PowerData>>(OperationCode.Error_TokenExpiration);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult<PaginatedList<PowerData>>(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForLastPowerData(api_account, timeStamp, sign, sn, token, apiAccount.Api_SecretKey))
				return new OperationResult<PaginatedList<PowerData>>(OperationCode.Error_Sign);

			var theToken = TokenHelper.GetToken(token);
			var theCompanyId = new Guid(apiAccount.CompanyId);
			var user = _userRepository.GetSingleByKey(theToken.UserId);

			if (user == null)
				return new OperationResult<PaginatedList<PowerData>>(OperationCode.Error_UserNotExist);

			var lstSn = new List<string>();

			if (string.IsNullOrWhiteSpace(sn))
			{
				PaginatedList<VT_SYSTEM> systems = null;
				if (theToken.UserTypes.Contains("customer"))
					systems = GetSystemByUser(1, int.MaxValue, user.USERNAME, theCompanyId);
				else if (theToken.UserTypes.Contains("installer"))
					systems = GetSystemByInstaller(1, int.MaxValue, user.Key, theCompanyId);
				else if (theToken.UserTypes.Contains("sharer"))
					systems = GetSystemBySharer(1, int.MaxValue, user.Inviter, theCompanyId);
				else if (theToken.UserTypes.Contains("reseller"))
					systems = QueryVtSystemForReseller(theCompanyId, user.Key.ToString(), 1, int.MaxValue);

				if (systems != null && systems.Any())
					lstSn = systems.Select(x => x.SysSn).ToList();
			}
			else
			{
				lstSn.Add(sn);
			}
			var lstData = _powerDataRepository.GetAll().Where(x => lstSn.Contains(x.Sn))
													.OrderByDescending(x => x.UploadTime)
													.GroupBy(x => x.Sn)
													.Select(x => x.FirstOrDefault())
													.ToList();

			return new OperationResult<PaginatedList<PowerData>>(OperationCode.Success, new PaginatedList<PowerData>(1, lstData.Count, lstData));
		}

		/// <summary>
		/// 获取指定sn的能量数据
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="sn">sn</param>
		/// <param name="username">用户名</param>
		/// <param name="start">开始日期</param>
		/// <param name="end">结束日期</param>
		/// <returns>OperationResult&lt;EnergyDate&gt;</returns>
		public OperationResult<EnergyReportData> GetEnergeData(string api_account, long timeStamp, string sign, string sn, string username, string start, string end, string statisticsby, string token)
		{
			if (string.IsNullOrWhiteSpace(token))
				return new OperationResult<EnergyReportData>(OperationCode.Error_Param_Empty);

			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult<EnergyReportData>(OperationCode.Error_TimeStamp);

			if (!TokenHelper.CheckToken(token))
				return new OperationResult<EnergyReportData>(OperationCode.Error_TokenExpiration);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult<EnergyReportData>(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForGetEnergeData(api_account, timeStamp, sign, sn, username, start, end, statisticsby, apiAccount.Api_SecretKey, token))
				return new OperationResult<EnergyReportData>(OperationCode.Error_Sign);

			var theCompanyId = new Guid(apiAccount.CompanyId);
			DateTime? dt_start = null;
			DateTime dt_end = DateTime.Now;
			DateTime dtTemp = DateTime.Now;
			if (!string.IsNullOrWhiteSpace(start))
			{
				if (!DateTime.TryParse(start, out dtTemp))
					return new OperationResult<EnergyReportData>(OperationCode.Error_Param);
				else
					dt_start = dtTemp;
			}
			if (string.IsNullOrWhiteSpace(end))
			{
				dt_end = DateTime.Now;
			}
			else if (!DateTime.TryParse(end, out dt_end))
			{
				return new OperationResult<EnergyReportData>(OperationCode.Error_Param);
			}

			if (!string.IsNullOrWhiteSpace(sn))
			{
				var ed = GetEnergeDataBySn(sn, dt_start, dt_end, theCompanyId, statisticsby);
				var result = new OperationResult<EnergyReportData>(OperationCode.Success);
				result.Entity = ed;
				return result;
			}
			else if (!string.IsNullOrWhiteSpace(username))
			{
				var ed = GetEnergeDataByUser(username, dt_start, dt_end, theCompanyId, statisticsby);
				var result = new OperationResult<EnergyReportData>(OperationCode.Success);
				result.Entity = ed;
				return result;
			}
			else
				return new OperationResult<EnergyReportData>(OperationCode.Error_Param);
		}

		public OperationResult<ProfitReportData> GetProfitData(string api_account, long timeStamp, string sign, string sn, string username, string start, string end, string statisticsby, string token)
		{
			if (string.IsNullOrWhiteSpace(token))
				return new OperationResult<ProfitReportData>(OperationCode.Error_Param_Empty);

			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult<ProfitReportData>(OperationCode.Error_TimeStamp);

			if (!TokenHelper.CheckToken(token))
				return new OperationResult<ProfitReportData>(OperationCode.Error_TokenExpiration);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult<ProfitReportData>(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForGetProfitData(api_account, timeStamp, sign, sn, username, start, end, statisticsby, apiAccount.Api_SecretKey, token))
				return new OperationResult<ProfitReportData>(OperationCode.Error_Sign);

			var theCompanyId = new Guid(apiAccount.CompanyId);
			DateTime? dt_start = null;
			DateTime dt_end = DateTime.Now;
			DateTime dtTemp = DateTime.Now;
			if (!string.IsNullOrWhiteSpace(start))
			{
				if (!DateTime.TryParse(start, out dtTemp))
					return new OperationResult<ProfitReportData>(OperationCode.Error_Param);
				else
					dt_start = dtTemp;
			}
			if (string.IsNullOrWhiteSpace(end))
			{
				dt_end = DateTime.Now;
			}
			else if (!DateTime.TryParse(end, out dt_end))
			{
				return new OperationResult<ProfitReportData>(OperationCode.Error_Param);
			}

			if (!string.IsNullOrWhiteSpace(sn))
			{
				var ed = GetProfitDataBySn(sn, dt_start, dt_end, theCompanyId, statisticsby);
				var result = new OperationResult<ProfitReportData>(OperationCode.Success);
				result.Entity = ed;
				return result;
			}
			else if (!string.IsNullOrWhiteSpace(username))
			{
				var ed = GetProfitDataByUser(username, dt_start, dt_end, theCompanyId, statisticsby);
				var result = new OperationResult<ProfitReportData>(OperationCode.Success);
				result.Entity = ed;
				return result;
			}
			else
				return new OperationResult<ProfitReportData>(OperationCode.Error_Param);
		}

		/// <summary>
		/// 创建新设备
		/// </summary>
		/// <param name="userId">用户id</param>
		/// <param name="sn">sn</param>
		/// <param name="address">地址</param>
		/// <param name="email">邮箱地址</param>
		/// <param name="country">国家</param>
		/// <param name="state">省/州</param>
		/// <param name="city">城市</param>
		/// <param name="language">语言</param>
		/// <param name="timeZone">时区</param>
		/// <param name="contactUser">联系人</param>
		/// <param name="zipCode">邮编</param>
		/// <param name="cellPhone">手机号</param>
		/// <param name="allowAutoUpdates">是否允许自动更新</param>
		/// <returns>VT_SYSTEM实例</returns>
		public VT_SYSTEM CreateNewSystem(Guid companyId, Guid userId, string sn, string address, string email, string country, string state,
			string city, string language, string timeZone, string contactUser, string zipCode, string cellPhone, int allowAutoUpdates)
		{
			//var system = new VT_SYSTEM
			//{ Key = Guid.NewGuid(), UserId = userId, syss = sn, ADDRESS = address, COUNTRY_CODE = country, STATE_CODE = state, CITY_CODE = city, LINKMAN = contactUser, POST_CODE = zipCode, CELL_PHONE = cellPhone, AllowAutoUpdate = allowAutoUpdates, DELETE_FLAG = 0, CompanyId = companyId };

			//_systemRepository.Add(system);
			//_systemRepository.Save();

			//return system;
			throw new NotImplementedException();
		}

		public OperationResult<PaginatedList<Report_Energy>> GetEnergyReportData(string api_account, long timeStamp, string sign, string sn, string theDate, string token)
		{
			if (string.IsNullOrWhiteSpace(token))
				return new OperationResult<PaginatedList<Report_Energy>>(OperationCode.Error_Param_Empty);

			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult<PaginatedList<Report_Energy>>(OperationCode.Error_TimeStamp);

			if (!TokenHelper.CheckToken(token))
				return new OperationResult<PaginatedList<Report_Energy>>(OperationCode.Error_TokenExpiration);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult<PaginatedList<Report_Energy>>(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForGetEnergySummary(api_account, timeStamp, sign, sn, theDate, token, apiAccount.Api_SecretKey))
				return new OperationResult<PaginatedList<Report_Energy>>(OperationCode.Error_Sign);

			var theToken = TokenHelper.GetToken(token);
			var theCompanyId = new Guid(apiAccount.CompanyId);
			var user = _userRepository.GetSingleByKey(theToken.UserId);

			if (user == null)
				return new OperationResult<PaginatedList<Report_Energy>>(OperationCode.Error_UserNotExist);

			var lstSn = new List<string>();

			if (string.IsNullOrWhiteSpace(sn))
			{
				PaginatedList<VT_SYSTEM> systems = null;
				if (theToken.UserTypes.Contains("customer"))
					systems = GetSystemByUser(1, int.MaxValue, user.USERNAME, theCompanyId);
				else if (theToken.UserTypes.Contains("installer"))
					systems = GetSystemByInstaller(1, int.MaxValue, user.Key, theCompanyId);
				else if (theToken.UserTypes.Contains("sharer"))
					systems = GetSystemBySharer(1, int.MaxValue, user.Inviter, theCompanyId);
				else if (theToken.UserTypes.Contains("reseller"))
					systems = QueryVtSystemForReseller(theCompanyId, user.Key.ToString(), 1, int.MaxValue);

				if (systems != null && systems.Any())
					lstSn = systems.Select(x => x.SysSn).ToList();
			}
			else
			{
				lstSn.Add(sn);
			}
			var lstReport = _reportEnergyRepository.GetEnergyReportBySns(lstSn, DateTime.Parse(theDate), 0);

			if (lstReport != null && lstReport.Any())
			{
				foreach (var itm in lstReport)
				{
					var income = _reportIncomeRepository.GetReportIncomeByDate(itm.SysSn, DateTime.Parse(theDate), 0);
					if (income != null && income.Any())
						itm.TotalIncome = income.Sum(x => x.ToalIncome);
				}
				return new OperationResult<PaginatedList<Report_Energy>>(OperationCode.Success, new PaginatedList<Report_Energy>(1, lstReport.Count, lstReport));
			}
			else
				return new OperationResult<PaginatedList<Report_Energy>>(OperationCode.Error_NoData);
		}

		public OperationResult<PaginatedList<VT_COLDATA>> GetRunningNewData(string api_account, long timeStamp, string sign, string sn, string token)
		{
			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult<PaginatedList<VT_COLDATA>>(OperationCode.Error_TimeStamp);

			if (!TokenHelper.CheckToken(token))
				return new OperationResult<PaginatedList<VT_COLDATA>>(OperationCode.Error_TokenExpiration);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult<PaginatedList<VT_COLDATA>>(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForRunningData(api_account, timeStamp, sign, sn, token, apiAccount.Api_SecretKey))
				return new OperationResult<PaginatedList<VT_COLDATA>>(OperationCode.Error_Sign);

			PaginatedList<VT_COLDATA> coldataList = new PaginatedList<VT_COLDATA>(1, int.MaxValue);

			var theToken = TokenHelper.GetToken(token);
			var theCompanyId = new Guid(apiAccount.CompanyId);
			var user = _userRepository.GetSingleByKey(theToken.UserId);

			if (user == null)
				return new OperationResult<PaginatedList<VT_COLDATA>>(OperationCode.Error_UserNotExist);

			var lstSn = new List<string>();
			if (string.IsNullOrWhiteSpace(sn))
			{
				PaginatedList<VT_SYSTEM> systems = null;
				if (theToken.UserTypes.Contains("customer"))
					systems = GetSystemByUser(1, int.MaxValue, user.USERNAME, theCompanyId);
				else if (theToken.UserTypes.Contains("installer"))
					systems = GetSystemByInstaller(1, int.MaxValue, user.Key, theCompanyId);
				else if (theToken.UserTypes.Contains("sharer"))
					systems = GetSystemBySharer(1, int.MaxValue, user.Inviter, theCompanyId);
				else if (theToken.UserTypes.Contains("reseller"))
					systems = QueryVtSystemForReseller(theCompanyId, user.Key.ToString(), 1, int.MaxValue);

				//coldataList = GetRunningDataByUsername(api_Account, timeStamp, sign, systems, pIndex, pSize, out totalCount);
				if (systems != null && systems.Any())
					lstSn = systems.Select(x => x.SysSn).ToList();
			}
			else
			{
				lstSn.Add(sn);
			}
			var data = _coldataRepository.GetAll().Where(x => lstSn.Contains(x.SysSn) && x.CompanyId == theCompanyId && x.DeleteFlag == 0 && x.FactoryFlag != 1)
																		.OrderByDescending(x => x.UploadDatetime)
																		.GroupBy(x => x.SysSn)
																		.Select(x => x.FirstOrDefault())
																		;

			return new OperationResult<PaginatedList<VT_COLDATA>>(OperationCode.Success, new PaginatedList<VT_COLDATA>(1, data.Count(), data.ToList()));
		}

		public OperationResult<PaginatedList<VT_COLDATA>> GetHistoryRunningData(string api_account, long timeStamp, string sign, string sn, string token, string starttime, string endtime)
		{
			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult<PaginatedList<VT_COLDATA>>(OperationCode.Error_TimeStamp);

			if (!TokenHelper.CheckToken(token))
				return new OperationResult<PaginatedList<VT_COLDATA>>(OperationCode.Error_TokenExpiration);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult<PaginatedList<VT_COLDATA>>(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForRunningData(api_account, timeStamp, sign, sn, token, apiAccount.Api_SecretKey))
				return new OperationResult<PaginatedList<VT_COLDATA>>(OperationCode.Error_Sign);

			PaginatedList<VT_COLDATA> coldataList = new PaginatedList<VT_COLDATA>(1, int.MaxValue);

			var theToken = TokenHelper.GetToken(token);
			var theCompanyId = new Guid(apiAccount.CompanyId);
			var user = _userRepository.GetSingleByKey(theToken.UserId);

			if (user == null)
				return new OperationResult<PaginatedList<VT_COLDATA>>(OperationCode.Error_UserNotExist);

			DateTime dtStart = DateTime.Now;
			DateTime dtEnd = DateTime.Now;
			if (!DateTime.TryParse(starttime, out dtStart))
				return new OperationResult<PaginatedList<VT_COLDATA>>(OperationCode.Error_Param);
			if (!DateTime.TryParse(endtime, out dtEnd))
				return new OperationResult<PaginatedList<VT_COLDATA>>(OperationCode.Error_Param);
			if (dtEnd < dtStart || (dtEnd - dtStart).TotalHours > 24)
				return new OperationResult<PaginatedList<VT_COLDATA>>(OperationCode.Error_Param);

			var lstSn = new List<string>();
			if (string.IsNullOrWhiteSpace(sn))
			{
				PaginatedList<VT_SYSTEM> systems = null;
				if (theToken.UserTypes.Contains("customer"))
					systems = GetSystemByUser(1, int.MaxValue, user.USERNAME, theCompanyId);
				else if (theToken.UserTypes.Contains("installer"))
					systems = GetSystemByInstaller(1, int.MaxValue, user.Key, theCompanyId);
				else if (theToken.UserTypes.Contains("sharer"))
					systems = GetSystemBySharer(1, int.MaxValue, user.Inviter, theCompanyId);
				else if (theToken.UserTypes.Contains("reseller"))
					systems = QueryVtSystemForReseller(theCompanyId, user.Key.ToString(), 1, int.MaxValue);

				//coldataList = GetRunningDataByUsername(api_Account, timeStamp, sign, systems, pIndex, pSize, out totalCount);
				if (systems != null && systems.Any())
					lstSn = systems.Select(x => x.SysSn).ToList();
			}
			else
			{
				lstSn.Add(sn);
			}
			var data = _coldataRepository.GetAll().Where(x => x.UploadDatetime >= dtStart && x.UploadDatetime <= dtEnd && lstSn.Contains(x.SysSn) && x.CompanyId == theCompanyId && x.DeleteFlag == 0 && x.FactoryFlag != 1)
																		.OrderByDescending(x => x.UploadDatetime)
																		.GroupBy(x => x.SysSn)
																		.Select(x => x.FirstOrDefault())
																		.ToList();

			return new OperationResult<PaginatedList<VT_COLDATA>>(OperationCode.Success, new PaginatedList<VT_COLDATA>(1, data.Count(), data));
		}

		public OperationResult<Sys_RemoteDispatch> AddRemoteDispatch(string api_account, long timeStamp, string sign, string token, string sn, int activePower, int reactivePower, decimal soc, int status, int controlMode)
		{
			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult<Sys_RemoteDispatch>(OperationCode.Error_TimeStamp);

			if (!TokenHelper.CheckToken(token))
				return new OperationResult<Sys_RemoteDispatch>(OperationCode.Error_TokenExpiration);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult<Sys_RemoteDispatch>(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForRemoteDispatch(api_account, timeStamp, sign, token, sn, activePower, reactivePower, soc, status, controlMode, apiAccount.Api_SecretKey))
				return new OperationResult<Sys_RemoteDispatch>(OperationCode.Error_Sign);

			var theToken = TokenHelper.GetToken(token);
			var theCompanyId = new Guid(apiAccount.CompanyId);
			var user = _userRepository.GetSingleByKey(theToken.UserId);

			if (user == null)
				return new OperationResult<Sys_RemoteDispatch>(OperationCode.Error_UserNotExist);

			var lstSn = new List<string>();
			if (string.IsNullOrWhiteSpace(sn))
			{
				PaginatedList<VT_SYSTEM> systems = null;
				if (theToken.UserTypes.Contains("customer"))
					systems = GetSystemByUser(1, int.MaxValue, user.USERNAME, theCompanyId);
				else if (theToken.UserTypes.Contains("installer"))
					systems = GetSystemByInstaller(1, int.MaxValue, user.Key, theCompanyId);
				else if (theToken.UserTypes.Contains("sharer"))
					systems = GetSystemBySharer(1, int.MaxValue, user.Inviter, theCompanyId);
				else if (theToken.UserTypes.Contains("reseller"))
					systems = QueryVtSystemForReseller(theCompanyId, user.Key.ToString(), 1, int.MaxValue);

				//coldataList = GetRunningDataByUsername(api_Account, timeStamp, sign, systems, pIndex, pSize, out totalCount);
				if (systems != null && systems.Any())
					lstSn = systems.Select(x => x.SysSn).ToList();
			}
			else
			{
				lstSn.Add(sn);

				Sys_RemoteDispatch rd = new Sys_RemoteDispatch
				{
					Key = Guid.NewGuid(), SN = sn, UserName = user.USERNAME, ActivePower = activePower, ReactivePower = reactivePower, SOC = soc, Status = status,
					ControlMode = controlMode, DELETE_FLAG = 0, CreateTime = DateTime.Now
				};

				_remotedispatchRepository.Add(rd);
				_remotedispatchRepository.Save();
			}

			if (lstSn != null && lstSn.Any())
			{
				try
				{
					foreach (var itm in lstSn)
					{
						Sys_RemoteDispatch rd = new Sys_RemoteDispatch
						{
							Key = Guid.NewGuid(), SN = itm, UserName = user.USERNAME, ActivePower = activePower, ReactivePower = reactivePower, SOC = soc, Status = status,
							ControlMode = controlMode, DELETE_FLAG = 0, CreateTime = DateTime.Now
						};
						_remotedispatchRepository.Add(rd);
					}
					_remotedispatchRepository.Save();
				}
				catch
				{
					return new OperationResult<Sys_RemoteDispatch>(OperationCode.Error_Unknown);
				}
			}

			return new OperationResult<Sys_RemoteDispatch>(OperationCode.Success);
		}

		public OperationResult<VT_SYSTEM> GetSystemDetail(string api_account, long timeStamp, string sign, string token, string sn)
		{
			if (string.IsNullOrWhiteSpace(token))
				return new OperationResult<VT_SYSTEM>(OperationCode.Error_Param_Empty);

			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult<VT_SYSTEM>(OperationCode.Error_TimeStamp);

			if (!TokenHelper.CheckToken(token))
				return new OperationResult<VT_SYSTEM>(OperationCode.Error_TokenExpiration);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult<VT_SYSTEM>(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForGetSystemDetail(api_account, timeStamp, sign, sn, token, apiAccount.Api_SecretKey))
				return new OperationResult<VT_SYSTEM>(OperationCode.Error_Sign);

			var theToken = TokenHelper.GetToken(token);
			var theCompanyId = new Guid(apiAccount.CompanyId);

			var system = GetSystemBySn(sn, theCompanyId);

			return new OperationResult<VT_SYSTEM>(OperationCode.Success, system);
		}

		public OperationResult UpdateVtSystem(string api_account, long timeStamp, string sign, string token, VT_SYSTEM s)
		{
			if (string.IsNullOrWhiteSpace(token))
				return new OperationResult(OperationCode.Error_Param_Empty);

			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult(OperationCode.Error_TimeStamp);

			if (!TokenHelper.CheckToken(token))
				return new OperationResult(OperationCode.Error_TokenExpiration);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult(OperationCode.Error_ApiAccountNotExist);

			var dbVtSystem = _systemRepository.GetSystemBySn(s.SysSn);//_systemRepository.GetSystemById(s.Key);
			if (dbVtSystem == null)
				return new OperationResult(OperationCode.Error_SystemNotExist);

			var editProperties = GetVtSystemEditProperties(dbVtSystem, s);
			if (!checkSignForUpdateSystem(api_account, timeStamp, sign, token, apiAccount.Api_SecretKey, s, editProperties))
				return new OperationResult(OperationCode.Error_Sign);

			if (!UpdateSystem(dbVtSystem, s, editProperties))
				return new OperationResult(OperationCode.Error_UpdateSystemFailed);

			return new OperationResult(OperationCode.Success);
		}

		public OperationResult BindNewSystem(string api_account, long timeStamp, string sign, string token, string sn, string licNo, string userName, string checkcode)
		{
			if (string.IsNullOrWhiteSpace(token))
				return new OperationResult(OperationCode.Error_Param_Empty);

			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult(OperationCode.Error_TimeStamp);

			if (!TokenHelper.CheckToken(token))
				return new OperationResult(OperationCode.Error_TokenExpiration);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForBingNewSystem(api_account, timeStamp, sign, token, apiAccount.Api_SecretKey, sn, licNo, userName, checkcode))
				return new OperationResult(OperationCode.Error_Sign);

			var theToken = TokenHelper.GetToken(token);
			var loginuser = _userRepository.GetSingleByKey(theToken.UserId);
			var customer = _userRepository.GetSingleByUserName(userName);

			if (loginuser == null || customer == null)
				return new OperationResult<PaginatedList<Report_Energy>>(OperationCode.Error_UserNotExist);

			if (string.IsNullOrWhiteSpace(sn))
				return new OperationResult(OperationCode.Error_Param_Empty);

			var snObj = _snRepository.GetSingleBySn(sn, loginuser.CompanyId.Value);
			if (snObj == null)
				return new OperationResult(OperationCode.Error_SNNotExist);

			if (string.IsNullOrWhiteSpace(snObj.BindPwd) && !(checkcode.Equals(GenerateSnPassword(sn), StringComparison.OrdinalIgnoreCase) || checkcode.Equals(GenerateSnPassword2(sn), StringComparison.OrdinalIgnoreCase)))
				return new OperationResult(OperationCode.Error_WrongCheckcode);

			if (loginuser.LICNO != licNo)
				return new OperationResult(OperationCode.Error_LicenseInconsistent);

			var s = _systemRepository.GetSystemBySn(sn);
			if (s == null)
				return new OperationResult(OperationCode.Error_SystemNotExist);

			if (s.UserId.HasValue)
				return new OperationResult(OperationCode.Error_AccountBound);

			try
			{
				s.UserId = customer.Key;
				s.Workmode = 0;
				_systemRepository.Edit(s);
				_systemRepository.Save();

				_alphaRemotingService.SendCommand(sn.Trim().ToUpper(), "SetConfig", new object[] { });
				_alphaRemotingService.SendCommand(sn.Trim().ToUpper(), "ChangeWorkMode", new object[] { });

				return new OperationResult(OperationCode.Success);
			}
			catch (Exception exc)
			{
				return new OperationResult(OperationCode.Error_Unknown);
			}
		}

		public OperationResult InstallNewSystem(string api_account, long timeStamp, string sign, string token, string sn, string licNo, string checkcode, DateTime installationDate, string customerName, string contactNumber, string contactAddress)
		{
			if (string.IsNullOrWhiteSpace(token))
				return new OperationResult(OperationCode.Error_Param_Empty);

			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult(OperationCode.Error_TimeStamp);

			if (!TokenHelper.CheckToken(token))
				return new OperationResult(OperationCode.Error_TokenExpiration);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForInstallNewSystem(api_account, timeStamp, sign, token, apiAccount.Api_SecretKey, sn, licNo, installationDate, checkcode, customerName, contactNumber, contactAddress))
				return new OperationResult(OperationCode.Error_Sign);

			var theToken = TokenHelper.GetToken(token);
			var loginuser = _userRepository.GetSingleByKey(theToken.UserId);
			if (loginuser == null)
				return new OperationResult(OperationCode.Error_UserNotExist);

			var snObj = _snRepository.GetSingleBySn(sn, loginuser.CompanyId.Value);
			if (snObj == null)
				return new OperationResult(OperationCode.Error_SNNotExist);

			if (string.IsNullOrWhiteSpace(snObj.BindPwd) && !(checkcode.Equals(GenerateSnPassword(sn), StringComparison.OrdinalIgnoreCase) || checkcode.Equals(GenerateSnPassword2(sn), StringComparison.OrdinalIgnoreCase)))
				return new OperationResult(OperationCode.Error_WrongCheckcode);

			if (_sysInstallRepository.GetAll().Any(x => x.SYS_SN == sn && x.DELETE_FLAG == 0))
				return new OperationResult(OperationCode.Error_InstallExist);

			try
			{
				VT_SYSINSTALL install = new VT_SYSINSTALL() { Key = Guid.NewGuid(), SYS_SN = sn, ADDRESS = contactAddress, CELL_PHONE = contactNumber, CREATE_DATETIME = DateTime.Now, DELETE_FLAG = 0, CUST_NAME = customerName, CREATE_ACCOUNT = loginuser.USERNAME, INSTALL_USERNAME = loginuser.USERNAME, INSTALL_USERID = loginuser.Key.ToString(), INSTALL_TIME = installationDate, CompanyId = loginuser.CompanyId };
				_sysInstallRepository.Add(install);
				_sysInstallRepository.Save();
				var system = _systemRepository.GetSystemBySn(sn);
				var countryName = string.Empty;
				if (!string.IsNullOrWhiteSpace(loginuser.COUNTRYCODE))
				{
					int code = 0;
					if (int.TryParse(loginuser.COUNTRYCODE, out code))
						countryName = _baseCountryRepository.GetAll().FirstOrDefault(x => x.Key == code).AREA_NAME;
				}

				if (system != null)
				{
					var workmodeChanged = false;
					system.LicNo = licNo;
					system.LastupdateAccount = loginuser.USERNAME;
					system.LastupdateDatetime = DateTime.Now;
					if (system.Workmode != 0)
					{
						workmodeChanged = true;
						system.Workmode = 0;
						system.ActiveTime = DateTime.Now;
					}
					if (string.IsNullOrWhiteSpace(system.CountryCode))
						system.CountryCode = countryName;
					if (string.IsNullOrWhiteSpace(system.PostCode))
						system.PostCode = loginuser.POSTCODE;

					_systemRepository.Edit(system);
					_systemRepository.Save();
					if (!snObj.DESCRIPTION.Equals("Growatt"))
					{
						_alphaRemotingService.SendCommand(sn, "SetConfig", new object[] { });
						if (workmodeChanged)
							_alphaRemotingService.SendCommand(sn, "ChangeWorkMode", new object[] { });
					}
				}
				else
				{
					VT_SYSTEM v = new VT_SYSTEM();
					v.Key = Guid.NewGuid();
					v.SysSn = sn;
					v.PostCode = loginuser.POSTCODE;
					v.AllowAutoUpdate = snObj.DESCRIPTION != "Growatt" ? 1 : 0;
					v.Workmode = 0;
					v.CreateAccount = loginuser.USERNAME;
					v.CreateDatetime = DateTime.Now;
					v.CompanyId = loginuser.CompanyId;
					v.CountryCode = countryName;
					v.PostCode = loginuser.POSTCODE;
					v.LicNo = licNo;
					v.DeleteFlag = 0;

					_systemRepository.Add(v);
					_systemRepository.Save();
				}
			}
			catch (Exception exc)
			{ }

			return new OperationResult(OperationCode.Success);
		}

		public OperationResult<FirmwareVersionData> GetFirmwareUpdate(string api_account, long timeStamp, string sign, string token, string sn)
		{
			if (string.IsNullOrWhiteSpace(token))
				return new OperationResult<FirmwareVersionData>(OperationCode.Error_Param_Empty);

			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult<FirmwareVersionData>(OperationCode.Error_TimeStamp);

			if (!TokenHelper.CheckToken(token))
				return new OperationResult<FirmwareVersionData>(OperationCode.Error_TokenExpiration);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult<FirmwareVersionData>(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForGetFirmwareUpdate(api_account, timeStamp, sign, token, apiAccount.Api_SecretKey, sn))
				return new OperationResult<FirmwareVersionData>(OperationCode.Error_Sign);

			var theToken = TokenHelper.GetToken(token);
			var loginuser = _userRepository.GetSingleByKey(theToken.UserId);
			if (loginuser == null)
				return new OperationResult<FirmwareVersionData>(OperationCode.Error_UserNotExist);

			var s = _systemRepository.GetSystemBySn(sn);
			if (s == null)
				return new OperationResult<FirmwareVersionData>(OperationCode.Error_SystemNotExist);

			var result = new FirmwareVersionData { BMSVersion = s.Bmsversion, EMSVersion = s.Emsversion, InvVersion = s.InvVersion };
			var lbmsv = _appversionRepository.GetAll().Where(x => x.APP_TYPE == "BMS" && x.DELETE_FLAG == 0 && x.CompanyId == loginuser.CompanyId).OrderByDescending(x => x.CREATE_DATETIME).FirstOrDefault();
			var lemsv = _appversionRepository.GetAll().Where(x => x.APP_TYPE == "EMS" && x.DELETE_FLAG == 0 && x.CompanyId == loginuser.CompanyId).OrderByDescending(x => x.CREATE_DATETIME).FirstOrDefault();
			var linvv = _appversionRepository.GetAll().Where(x => x.APP_TYPE == "INV" && x.DELETE_FLAG == 0 && x.CompanyId == loginuser.CompanyId).OrderByDescending(x => x.CREATE_DATETIME).FirstOrDefault();
			result.LatestBMSVersion = lbmsv != null ? lbmsv.APP_VERSION : string.Empty;
			result.LatestEMSVersion = lemsv != null ? lemsv.APP_VERSION : string.Empty;
			result.LatestInvVersion = linvv != null ? linvv.APP_VERSION : string.Empty;

			return new OperationResult<FirmwareVersionData>(OperationCode.Success, result);
		}

		public OperationResult UpdateSystemFirmware(string api_account, long timeStamp, string sign, string token, string sn, string category)
		{
			if (string.IsNullOrWhiteSpace(token))
				return new OperationResult(OperationCode.Error_Param_Empty);

			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult(OperationCode.Error_TimeStamp);

			if (!TokenHelper.CheckToken(token))
				return new OperationResult(OperationCode.Error_TokenExpiration);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForUpdateSystemFirmware(api_account, timeStamp, sign, token, sn, category, apiAccount.Api_SecretKey))
				return new OperationResult(OperationCode.Error_Sign);

			var theToken = TokenHelper.GetToken(token);
			var loginuser = _userRepository.GetSingleByKey(theToken.UserId);
			if (loginuser == null)
				return new OperationResult(OperationCode.Error_UserNotExist);

			var s = _systemRepository.GetSystemBySn(sn);
			if (s == null)
				return new OperationResult(OperationCode.Error_SystemNotExist);

			var snObj = _snRepository.GetSingleBySn(sn, s.CompanyId.Value);
			if (snObj.IsUpgrade == 1)
				return new OperationResult(OperationCode.Error_UpdateSystemFailed);

			var arrVersionType = new List<string>();
			if (!string.IsNullOrWhiteSpace(category))
				arrVersionType = category.Split('-').ToList();
			else
				arrVersionType.AddRange(new List<string> { "BMS", "EMS", "INV" });

			var lbmsv = _appversionRepository.GetAll().Where(x => x.APP_TYPE == "BMS" && x.DELETE_FLAG == 0 && x.CompanyId == loginuser.CompanyId).OrderByDescending(x => x.CREATE_DATETIME).FirstOrDefault();
			var lemsv = _appversionRepository.GetAll().Where(x => x.APP_TYPE == "EMS" && x.DELETE_FLAG == 0 && x.CompanyId == loginuser.CompanyId).OrderByDescending(x => x.CREATE_DATETIME).FirstOrDefault();
			var linvv = _appversionRepository.GetAll().Where(x => x.APP_TYPE == "INV" && x.DELETE_FLAG == 0 && x.CompanyId == loginuser.CompanyId).OrderByDescending(x => x.CREATE_DATETIME).FirstOrDefault();

			var alphacloudurl = System.Configuration.ConfigurationManager.AppSettings["ALphaEssWebSiteUrl"];
			if (string.IsNullOrWhiteSpace(alphacloudurl))
				alphacloudurl = @"http://www.alphaess.com";
			else if (alphacloudurl.EndsWith("/"))
				alphacloudurl = alphacloudurl.Substring(0, alphacloudurl.Length - 1);

			var flag = true;
			foreach (var itm in arrVersionType)
			{
				if (!string.IsNullOrWhiteSpace(itm))
				{
					var args = new ArrayList();
					args.Add(itm);
					var description = string.Empty;

					if (itm.Equals("BMS") && lbmsv != null && lbmsv.APP_VERSION != s.Bmsversion && !string.IsNullOrWhiteSpace(lbmsv.UPDATE_URL))
					{
						args.Add(alphacloudurl + (lbmsv.UPDATE_URL.StartsWith("~") ? lbmsv.UPDATE_URL.Substring(1) : lbmsv.UPDATE_URL));
						description = string.IsNullOrWhiteSpace(lbmsv.HashMd5) ? string.Empty : lbmsv.HashMd5 + "|" + (string.IsNullOrWhiteSpace(lbmsv.APP_VERSION) ? string.Empty : lbmsv.APP_VERSION);//Description 带上版本信息
					}
					else if (itm.Equals("EMS") && lemsv != null && lemsv.APP_VERSION != s.Emsversion && !string.IsNullOrWhiteSpace(lemsv.UPDATE_URL))
					{
						args.Add(alphacloudurl + (lemsv.UPDATE_URL.StartsWith("~") ? lemsv.UPDATE_URL.Substring(1) : lemsv.UPDATE_URL));
						description = string.IsNullOrWhiteSpace(lemsv.HashMd5) ? string.Empty : lemsv.HashMd5 + "|" + (string.IsNullOrWhiteSpace(lemsv.APP_VERSION) ? string.Empty : lemsv.APP_VERSION);//Description 带上版本信息
					}
					else if (itm.Equals("INV") && linvv != null && linvv.APP_VERSION != s.InvVersion && !string.IsNullOrWhiteSpace(linvv.UPDATE_URL))
					{
						args.Add(alphacloudurl + (linvv.UPDATE_URL.StartsWith("~") ? linvv.UPDATE_URL.Substring(1) : linvv.UPDATE_URL));
						args.Add(alphacloudurl + (linvv.UPDATE_URL.StartsWith("~") ? linvv.UPDATE_URL.Substring(1) : linvv.UPDATE_URL));
						description = string.IsNullOrWhiteSpace(linvv.HashMd5) ? string.Empty : linvv.HashMd5 + "|" + (string.IsNullOrWhiteSpace(linvv.APP_VERSION) ? string.Empty : linvv.APP_VERSION);//Description 带上版本信息
					}

					flag = flag && _alphaRemotingService.SendCommand(sn, "Update", args.ToArray());
				}
			}

			if (flag)
				return new OperationResult(OperationCode.Success);
			else
				return new OperationResult(OperationCode.Error_UpdateSystemFirmwareFailed);
		}

		public OperationResult<SystemSummaryStatisticsData> GetSystemSummaryStatisticsData(string api_account, long timeStamp, string sign, string token)
		{
			if (string.IsNullOrWhiteSpace(token))
				return new OperationResult<SystemSummaryStatisticsData>(OperationCode.Error_Param_Empty);

			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult<SystemSummaryStatisticsData>(OperationCode.Error_TimeStamp);

			if (!TokenHelper.CheckToken(token))
				return new OperationResult<SystemSummaryStatisticsData>(OperationCode.Error_TokenExpiration);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult<SystemSummaryStatisticsData>(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForGetSystemSummaryStatisticsData(api_account, timeStamp, sign, token, apiAccount.Api_SecretKey))
				return new OperationResult<SystemSummaryStatisticsData>(OperationCode.Error_Sign);

			var theToken = TokenHelper.GetToken(token);
			var loginuser = _userRepository.GetSingleByKey(theToken.UserId);
			if (loginuser == null)
				return new OperationResult<SystemSummaryStatisticsData>(OperationCode.Error_UserNotExist);

			var result = new SystemSummaryStatisticsData();
			var statusCount = _systemRepository.GetVtSystemStatusCountByLicense(loginuser.CompanyId, loginuser.LICNO);
			if (statusCount != null)
			{
				result.NormalCount = statusCount.NormalCount;
				result.ProtectionCount = statusCount.ProtectionCount;
				result.FaultCount = statusCount.FaultCount;
				result.OfflineCount = statusCount.OfflineCount;
			}
			if (theToken.UserTypes.Contains("installer"))
			{
				var summary = _systemRepository.GetSummaryInfoByUser(loginuser.CompanyId, loginuser.Key);
				if (summary != null)
				{
					result.Cobat = summary.Cobat;
					result.Ebat = summary.Ebat;
					result.EchargeT = summary.EchargeT;
					result.Einput = summary.Einput;
					result.Eload = summary.Eload;
					result.Eoutput = summary.Eoutput;
					result.Epv2Load = summary.Epv2Load;
					result.Epvtotal = summary.Epvtotal;
					result.EselfConsumption = summary.EselfConsumption;
					result.EselfSufficiency = summary.EselfSufficiency;
					result.Poinv = summary.Poinv;
				}
			}

			return new OperationResult<SystemSummaryStatisticsData>(OperationCode.Success, result);
		}
	}
}
