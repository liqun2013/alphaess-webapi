﻿using AlphaEss.Api_V2.Infrastructure;
using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace AlphaEssWeb.Api_V2.Domain.Services
{
	public class ReportInfoService : IReportInfoService
	{
		private readonly IEntityRepository<SYS_USER, Guid> _userRepository;
		private readonly IEntityRepository<Report_Energy, long> _report_Energy;
		private readonly IEntityRepository<VT_SYSTEM, Guid> _systemRepository;
		private readonly IEntityRepository<SYS_USERSERVICEAREA, Guid> _sysUserservicearea;
		private readonly IEntityRepository<Sys_ResellerLicense, long> _sys_ResellerLicense;
		private readonly IEntityRepository<Sys_ServicePartnerSn, long> _sys_ServicePartnerSn;
		private readonly IEntityRepository<SYS_SN, Guid> _snRepository;
		private readonly IEntityRepository<SYS_ROLE, Guid> _sys_ROLE;
		private readonly IEntityRepository<SYS_ROLEUSER, Guid> _sys_ROLEUSER;
		private readonly IParameterValidateService _parameterValidateService;
		private readonly ICryptoService _cryptoService;
        private readonly IEntityRepository<SYS_LOG, Guid> _sysLogService;

        public ReportInfoService(IEntityRepository<SYS_USER, Guid> userRepository, IEntityRepository<Report_Energy, long> report_Energy, IEntityRepository<VT_SYSTEM, Guid> systemRepository, IEntityRepository<SYS_USERSERVICEAREA, Guid> sysUserservicearea, IEntityRepository<Sys_ResellerLicense, long> sys_ResellerLicense, IEntityRepository<Sys_ServicePartnerSn, long> sys_ServicePartnerSn, IEntityRepository<SYS_SN, Guid> snRepository, IEntityRepository<SYS_ROLE, Guid> sys_ROLE, IEntityRepository<SYS_ROLEUSER, Guid> sys_ROLEUSER, IParameterValidateService parameterValidateService, ICryptoService cryptoService, IEntityRepository<SYS_LOG, Guid> sysLogService)
		{
			_userRepository = userRepository;
			_report_Energy = report_Energy;
			_systemRepository = systemRepository;
			_sysUserservicearea = sysUserservicearea;
			_sys_ResellerLicense = sys_ResellerLicense;
			_sys_ServicePartnerSn = sys_ServicePartnerSn;
			_snRepository = snRepository;
			_sys_ROLE = sys_ROLE;
			_sys_ROLEUSER = sys_ROLEUSER;
			_parameterValidateService = parameterValidateService;
			_cryptoService = cryptoService;
            _sysLogService = sysLogService;
        }

		/// <summary>
		/// 公司id配置在webconfig里
		/// </summary>
		public Guid CompanyId
		{
			get
			{
				var companyId = System.Configuration.ConfigurationManager.AppSettings["companyid"];
				if (companyId != null && !string.IsNullOrWhiteSpace(companyId))
				{
					return new Guid(System.Configuration.ConfigurationManager.AppSettings["companyid"]);
				}
				else
					throw new Exception("companyid is empty");
			}
		}

		#region 获取系统能量信息
		/// <summary>
		/// 获取系统能量信息
		/// </summary>
		/// <param name="sn"></param>
		/// <param name="username">用户名</param>
		/// <param name="theDate">日期</param>
		/// <param name="token">token</param>
		/// <returns></returns>
		public OperationResult<PaginatedList<Report_Energy>> GetEnergySummary(string api_Account, long timeStamp, string sign, string sn, string theDate, string token, string ipAddress)
		{
			Guid key = Guid.Empty;
			Guid.TryParse(token, out key);
			var userCache = TokenService.GetCache(key);

			#region 判断
			if (userCache == null || userCache.ExpirationTime < DateTime.Now)
			{
				return new OperationResult<PaginatedList<Report_Energy>>(OperationCode.Error_LoginFailed);
			}

			if (userCache.ipAddress != ipAddress)
			{
                var ul = new SYS_LOG { Key = Guid.NewGuid(), CREATE_DATETIME = DateTime.Now, THREAD = OperationCode.Error_UserChangesDevice.ToString(), LEVEL = LogLevel.ERROR.ToString(), LOGGER = "Services.ReportInfoService", MESSAGE = "用户更换设备", EXCEPTION = "ipAddress_1:" + userCache.ipAddress + ",ipAddress:" + ipAddress, CREATE_ACCOUNT = "", LOG_CONTENT = "" };
                _sysLogService.Add(ul);
                _sysLogService.Save();

                return new OperationResult<PaginatedList<Report_Energy>>(OperationCode.Error_UserChangesDevice);
			}

			var query = from a1 in _sys_ROLE.GetAll()
									join r1 in _sys_ROLEUSER.GetAll() on a1.Key equals r1.ROLEID
									where r1.USERID == userCache.UserId
									select new
									{
										userType = a1.ROLENAME
									};

			List<string> user_Types = new List<string>();
			foreach (var q in query)
			{
				user_Types.Add(q.userType);
			}

            if (!checkSignForEnergySummary(api_Account, timeStamp, sign, sn, theDate, token))
			{
                string secretKey = _cryptoService.GetSecretKey(api_Account);
                var ul = new SYS_LOG { Key = Guid.NewGuid(), CREATE_DATETIME = DateTime.Now, THREAD = OperationCode.Error_Sign.ToString(), LEVEL = LogLevel.ERROR.ToString(), LOGGER = "Services.ReportInfoService", MESSAGE = "签名错误", EXCEPTION = "api_Account:" + api_Account + ",timeStamp:" + timeStamp + ",sign:" + sign + ",token:" + token + ", sn:" + sn + ",ipAddress:" + ipAddress+ ",theDate:"+ theDate + ",SecretKey:" + secretKey, CREATE_ACCOUNT = "", LOG_CONTENT = "" };
                _sysLogService.Add(ul);
                _sysLogService.Save();

                return new OperationResult<PaginatedList<Report_Energy>>(OperationCode.Error_Sign);
			}

			if (!_parameterValidateService.CheckTimestamp(timeStamp))
			{
                var ul = new SYS_LOG { Key = Guid.NewGuid(), CREATE_DATETIME = DateTime.Now, THREAD = OperationCode.Error_TimeStamp.ToString(), LEVEL = LogLevel.ERROR.ToString(), LOGGER = "Services.ReportInfoService", MESSAGE = "时间戳错误", EXCEPTION = "api_Account:" + api_Account + ",timeStamp:" + timeStamp + ",sign:" + sign + ",token:" + token + ", sn:" + sn + ",ipAddress:" + ipAddress + ",theDate" + theDate, CREATE_ACCOUNT = "", LOG_CONTENT = "" };
                _sysLogService.Add(ul);
                _sysLogService.Save();

                return new OperationResult<PaginatedList<Report_Energy>>(OperationCode.Error_TimeStamp);
			}

			if (!_parameterValidateService.ApiAccountExist(api_Account))
			{
                var ul = new SYS_LOG { Key = Guid.NewGuid(), CREATE_DATETIME = DateTime.Now, THREAD = OperationCode.Error_ApiAccountNotExist.ToString(), LEVEL = LogLevel.ERROR.ToString(), LOGGER = "Services.ReportInfoService", MESSAGE = "api账户不存在", EXCEPTION = "api_Account:" + api_Account + ",timeStamp:" + timeStamp + ",sign:" + sign + ",token:" + token + ", sn:" + sn + ",ipAddress:" + ipAddress + ",theDate" + theDate, CREATE_ACCOUNT = "", LOG_CONTENT = "" };
                _sysLogService.Add(ul);
                _sysLogService.Save();

                return new OperationResult<PaginatedList<Report_Energy>>(OperationCode.Error_ApiAccountNotExist);
			}

            DateTime dt = DateTime.Now;
            if (!DateTime.TryParse(theDate, out dt))
            {
                var ul = new SYS_LOG { Key = Guid.NewGuid(), CREATE_DATETIME = DateTime.Now, THREAD = OperationCode.Error_ApiAccountNotExist.ToString(), LEVEL = LogLevel.ERROR.ToString(), LOGGER = "Services.ReportInfoService", MESSAGE = "时间参数转换错误", EXCEPTION = "api_Account:" + api_Account + ",timeStamp:" + timeStamp + ",sign:" + sign + ",token:" + token + ", sn:" + sn + ",ipAddress:" + ipAddress + ",theDate" + dt, CREATE_ACCOUNT = "", LOG_CONTENT = "" };
                _sysLogService.Add(ul);
                _sysLogService.Save();

                return new OperationResult<PaginatedList<Report_Energy>>(OperationCode.Error_Param);
            }
            #endregion

            int pIndex = 1;
			int pSize = int.MaxValue;
			int totalCount = 0;
			PaginatedList<Report_Energy> energyList = new PaginatedList<Report_Energy>(pIndex, pSize, null);

			#region 获取用户系统列表
			IList<VT_SYSTEM> systems = new List<VT_SYSTEM>();
			if (user_Types.Contains("customer"))
			{
				systems = GetSystemByUser(pIndex, pSize, userCache.UserId, this.CompanyId, out totalCount);
			}
			else if (user_Types.Contains("installer"))
			{
				SYS_USER user = _userRepository.GetAll().Where(x => x.Key == userCache.UserId).FirstOrDefault();
				if (user != null)
				{
					systems = GetSystemByInstaller(pIndex, pSize, user.LICNO, this.CompanyId, out totalCount);
				}
			}
			else if (user_Types.Contains("servicer"))
			{
				systems = GetSystemByServicer(pIndex, pSize, userCache.UserId, this.CompanyId, out totalCount);
			}
			else if (user_Types.Contains("admin"))
			{
				systems = GetSystemByManager(pIndex, pSize, this.CompanyId, out totalCount);
			}
			else if (user_Types.Contains("systemmanager"))
			{
				systems = systems = GetSystemByManager(pIndex, pSize, this.CompanyId, out totalCount);
			}
			else if (user_Types.Contains("reseller"))
			{
				systems = GetSystemByReseller(pIndex, pSize, userCache.UserId, this.CompanyId, out totalCount);
			}
			else if (user_Types.Contains("servicepartner"))
			{
				systems = GetSystemByServicepartner(pIndex, pSize, userCache.UserId, this.CompanyId, out totalCount);
			}
			#endregion

			if (!string.IsNullOrWhiteSpace(sn))
			{
				if (_snRepository.GetAll().Where(x => x.SN_NO == sn.Trim() && x.CompanyId == this.CompanyId && x.DELETE_FLAG == 0).FirstOrDefault() == null)
				{
                    var ul = new SYS_LOG { Key = Guid.NewGuid(), CREATE_DATETIME = DateTime.Now, THREAD = OperationCode.Error_SNNotExist.ToString(), LEVEL = LogLevel.ERROR.ToString(), LOGGER = "Services.ReportInfoService", MESSAGE = "sn不存在", EXCEPTION = "api_Account:" + api_Account + ",timeStamp:" + timeStamp + ",sign:" + sign + ",token:" + token + ", sn:" + sn + ",ipAddress:" + ipAddress + ",theDate" + dt, CREATE_ACCOUNT = "", LOG_CONTENT = "" };
                    _sysLogService.Add(ul);
                    _sysLogService.Save();

                    return new OperationResult<PaginatedList<Report_Energy>>(OperationCode.Error_SNNotExist);
				}

				if (systems.Where(x => x.SYS_SN == sn).Count() == 0)
				{
                    SYS_USER user = _userRepository.GetAll().Where(x => x.Key == userCache.UserId).FirstOrDefault();
                    string username = user == null ? "" : user.USERNAME;

                    var ul = new SYS_LOG { Key = Guid.NewGuid(), CREATE_DATETIME = DateTime.Now, THREAD = OperationCode.Error_NoPermissionsToQuery.ToString(), LEVEL = LogLevel.ERROR.ToString(), LOGGER = "Services.ReportInfoService", MESSAGE = "用户无此权限,调取用户:" + username, EXCEPTION = "api_Account:" + api_Account + ",timeStamp:" + timeStamp + ",sign:" + sign + ",token:" + token + ", sn:" + sn + ",ipAddress:" + ipAddress + ",theDate" + dt, CREATE_ACCOUNT = "", LOG_CONTENT = "" };
                    _sysLogService.Add(ul);
                    _sysLogService.Save();

                    return new OperationResult<PaginatedList<Report_Energy>>(OperationCode.Error_NoPermissionsToQuery);
				}

				energyList = GetEnergySummaryBySn(api_Account, timeStamp, sign, sn, dt, pIndex, pSize, out totalCount);
			}
			else
			{
				energyList = GetEnergySummaryByUsername(api_Account, timeStamp, sign, systems, dt, pIndex, pSize, out totalCount);
			}

			energyList.TotalCount = totalCount;
			energyList.TotalPageCount = (int)Math.Ceiling(energyList.TotalCount / (double)pSize);
			var plSystems = new OperationResult<PaginatedList<Report_Energy>>(OperationCode.Success);
			plSystems.Entity = energyList;

			return plSystems;
		}

		private bool checkSignForEnergySummary(string api_Account, long timeStamp, string sign, string sn, string theDate, string token)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_Account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("Sn", sn);	
			slParams.Add("Token", token);
            slParams.Add("TheDate", theDate);
            slParams.Add("secretkey", _cryptoService.GetSecretKey(api_Account));

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		/// <summary>
		/// 获取系统能量信息 根据username
		/// </summary>
		/// <returns></returns>
		private PaginatedList<Report_Energy> GetEnergySummaryByUsername(string api_Account, long timeStamp, string sign, IList<VT_SYSTEM> systems, DateTime theDate, int pIndex, int pSize, out int totalCount)
		{
			IList<string> lst = new List<string>();
			if (systems != null && systems.Count > 0)
			{
				foreach (var item in systems)
				{
					if (!string.IsNullOrEmpty(item.SYS_SN.Trim()))
						lst.Add(item.SYS_SN.Trim());
				}
			}

			IList<Report_Energy> result = new List<Report_Energy>();
			result = _report_Energy.GetAll().Where(x => lst.Contains(x.SysSn) && x.TheDate == theDate && x.WorkMode == 0).ToList();

			if (result != null && result.Count > 0)
			{
				totalCount = result.Count;
				return new PaginatedList<Report_Energy>(pIndex, pSize, result.AsQueryable());
			}
			else
			{
				totalCount = 0;
				return new PaginatedList<Report_Energy>(pIndex, pSize, null);
			}
		}

		/// <summary>
		/// 获取系统能量信息 根据sn
		/// </summary>
		/// <returns></returns>
		private PaginatedList<Report_Energy> GetEnergySummaryBySn(string api_Account, long timeStamp, string sign, string sn, DateTime theDate, int pIndex, int pSize, out int totalCount)
		{
			IList<Report_Energy> result = new List<Report_Energy>();

			result = _report_Energy.GetAll().Where(x => x.SysSn == sn && x.TheDate == theDate && x.WorkMode == 0).ToList();

			if (result != null && result.Count > 0)
			{
				totalCount = result.Count;
				return new PaginatedList<Report_Energy>(pIndex, pSize, result.AsQueryable());
			}
			else
			{
				totalCount = 0;
				return new PaginatedList<Report_Energy>(pIndex, pSize, null);
			}

		}
		#endregion

		#region 获取系统能量信息 方法
		public IList<VT_SYSTEM> GetSystemByServicepartner(int pIndex, int pSize, Guid userId, Guid companyId, out int totalCount)
		{
			IList<VT_SYSTEM> vs = new List<VT_SYSTEM>();
			IList<Sys_ServicePartnerSn> rl = _sys_ServicePartnerSn.GetAll().Where(x => x.UserId == userId).ToList<Sys_ServicePartnerSn>();

			List<string> ss = new List<string>();
			if (rl != null && rl.Count > 0)
			{
				foreach (var item in rl)
				{
					if (!string.IsNullOrEmpty(item.SysSn))
						ss.Add(item.SysSn);
				}
			}

			var result2 = (QueryForManager(companyId)).Where(x => ss.Contains(x.SYS_SN)).ToList();

			if (result2.Count() > 0)
			{
				var systems = new List<VT_SYSTEM>();
				foreach (var item in result2.ToList())
				{
					vs.Add(new VT_SYSTEM
					{
						Key = item.Key,
						SYS_SN = item.SYS_SN,
						SYS_NAME = item.SYS_NAME,
						COBAT = item.COBAT,
						USCAPACITY = item.USCAPACITY,
						MBAT = item.MBAT,
						POINV = item.POINV,
						POPV = item.POPV,
						RemarkSM = item.RemarkSM,
						ACDC = item.ACDC,
						EMSVersion = item.EMSVersion,
						BMSVersion = item.BMSVersion,
						InvVersion = item.InvVersion,
						MINV = item.MINV,
						MMETER = item.MMETER,
						SetPhase = item.SetPhase,
						SetFeed = item.SetFeed,
						EmsStatus = item.EmsStatus,
						LASTUPDATE_DATETIME = item.LASTUPDATE_DATETIME,
						SYS_USER = item.SYS_USER
					});
				}
			}
			totalCount = vs.Count();

			if (totalCount > 0)
			{
				vs = vs.Skip((pIndex - 1) * pSize).Take(pSize).ToList();
				return vs;
			}
			else
			{
				return null;
			}
		}

		public IList<VT_SYSTEM> GetSystemByReseller(int pIndex, int pSize, Guid userId, Guid companyId, out int totalCount)
		{
			IList<VT_SYSTEM> vs = new List<VT_SYSTEM>();

			IList<Sys_ResellerLicense> rl = _sys_ResellerLicense.GetAll().Where(x => x.UserId == userId).ToList<Sys_ResellerLicense>();


			List<string> ss = new List<string>();
			if (rl != null && rl.Count > 0)
			{
				foreach (var item in rl)
				{
					if (!string.IsNullOrEmpty(item.LicenseNo))
						ss.Add(item.LicenseNo);
				}
			}
			IList<VT_SYSTEM> result2 = (QueryForManager(companyId)).Where(x => ss.Contains(x.LIC_NO)).ToList();

			if (result2.Count() > 0)
			{
				var systems = new List<VT_SYSTEM>();
				foreach (var item in result2.ToList())
				{
					vs.Add(new VT_SYSTEM
					{
						Key = item.Key,
						SYS_SN = item.SYS_SN,
						SYS_NAME = item.SYS_NAME,
						COBAT = item.COBAT,
						USCAPACITY = item.USCAPACITY,
						MBAT = item.MBAT,
						POINV = item.POINV,
						POPV = item.POPV,
						RemarkSM = item.RemarkSM,
						ACDC = item.ACDC,
						EMSVersion = item.EMSVersion,
						BMSVersion = item.BMSVersion,
						InvVersion = item.InvVersion,
						MINV = item.MINV,
						MMETER = item.MMETER,
						SetPhase = item.SetPhase,
						SetFeed = item.SetFeed,
						EmsStatus = item.EmsStatus,
						LASTUPDATE_DATETIME = item.LASTUPDATE_DATETIME,
						SYS_USER = item.SYS_USER
					});
				}
			}
			totalCount = vs.Count();

			if (totalCount > 0)
			{
				vs = vs.Skip((pIndex - 1) * pSize).Take(pSize).ToList();
				return vs;
			}
			else
			{
				return null;
			}
		}

		public IList<VT_SYSTEM> GetSystemByManager(int pIndex, int pSize, Guid companyId, out int totalCount)
		{
			IList<VT_SYSTEM> vs = new List<VT_SYSTEM>();
			var result2 = QueryForManager(companyId);
			totalCount = result2.Count();
			result2 = result2.Skip((pIndex - 1) * pSize).Take(pSize).ToList();

			if (result2.Count() > 0)
			{
				var systems = new List<VT_SYSTEM>();
				foreach (var item in result2.ToList())
				{
					vs.Add(new VT_SYSTEM
					{
						Key = item.Key,
						SYS_SN = item.SYS_SN,
						SYS_NAME = item.SYS_NAME,
						COBAT = item.COBAT,
						USCAPACITY = item.USCAPACITY,
						MBAT = item.MBAT,
						POINV = item.POINV,
						POPV = item.POPV,
						RemarkSM = item.RemarkSM,
						ACDC = item.ACDC,
						EMSVersion = item.EMSVersion,
						BMSVersion = item.BMSVersion,
						InvVersion = item.InvVersion,
						MINV = item.MINV,
						MMETER = item.MMETER,
						SetPhase = item.SetPhase,
						SetFeed = item.SetFeed,
						EmsStatus = item.EmsStatus,
						LASTUPDATE_DATETIME = item.LASTUPDATE_DATETIME,
						SYS_USER = item.SYS_USER
					});
				}
			}

			if (totalCount > 0)
			{
				return vs;
			}
			else
			{
				return null;
			}
		}

		public IList<VT_SYSTEM> GetSystemByServicer(int pIndex, int pSize, Guid userId, Guid companyId, out int totalCount)
		{
			IList<SYS_USERSERVICEAREA> userServiceAreaList = null;
			IList<VT_SYSTEM> vs = new List<VT_SYSTEM>();
			int count = 0;

			var query = _sysUserservicearea.GetAll().Where(x => x.USER_ID == userId.ToString());

			if (query.Any())
				userServiceAreaList = query.ToList();

			if (userServiceAreaList != null && userServiceAreaList.Count > 0)
			{
				if (userServiceAreaList.Any(x => x.AllArea != null && x.AllArea.Equals("All", StringComparison.OrdinalIgnoreCase)))
				{
					var result2 = QueryForManager(companyId);
					count = result2.Count();
					result2 = result2.Skip((pIndex - 1) * pSize).Take(pSize).ToList();

					if (result2.Count() > 0)
					{
						var systems = new List<VT_SYSTEM>();
						foreach (var item in result2.ToList())
						{
							vs.Add(new VT_SYSTEM
							{
								Key = item.Key,
								SYS_SN = item.SYS_SN,
								SYS_NAME = item.SYS_NAME,
								COBAT = item.COBAT,
								USCAPACITY = item.USCAPACITY,
								MBAT = item.MBAT,
								POINV = item.POINV,
								POPV = item.POPV,
								RemarkSM = item.RemarkSM,
								ACDC = item.ACDC,
								EMSVersion = item.EMSVersion,
								BMSVersion = item.BMSVersion,
								InvVersion = item.InvVersion,
								MINV = item.MINV,
								MMETER = item.MMETER,
								SetPhase = item.SetPhase,
								SetFeed = item.SetFeed,
								EmsStatus = item.EmsStatus,
								LASTUPDATE_DATETIME = item.LASTUPDATE_DATETIME,
								SYS_USER = item.SYS_USER
							});
						}
					}
				}
				else {
					IList<VT_SYSTEM> result = new List<VT_SYSTEM>();

					foreach (var area in userServiceAreaList)
					{
						Expression<Func<VT_SYSTEM, bool>> countryNameFunc = f => true;
						Expression<Func<VT_SYSTEM, bool>> provinceNameFunc = f => true;
						Expression<Func<VT_SYSTEM, bool>> cityNameFunc = f => true;

						if (!string.IsNullOrWhiteSpace(area.COUNTRY_NAME))
							countryNameFunc = x => x.COUNTRY_CODE == area.COUNTRY_NAME;
						if (!string.IsNullOrWhiteSpace(area.PROVINCE_NAME))
							provinceNameFunc = x => x.STATE_CODE == area.PROVINCE_NAME;
						if (!string.IsNullOrWhiteSpace(area.CITY_NAME))
							cityNameFunc = x => x.CITY_CODE == area.CITY_NAME;

						var lstSystem = _systemRepository.GetAll().Where(x => x.DELETE_FLAG == 0 && (x.WorkMode == null || x.WorkMode == 0) && x.CompanyId == companyId).Where(countryNameFunc).Where(provinceNameFunc).Where(cityNameFunc).ToList<VT_SYSTEM>();

						if (lstSystem != null && lstSystem.Count > 0)
						{
							foreach (var sys in lstSystem)
							{
								result.Add(sys);
							}
						}
					}

					if (result.Count > 0)
					{
						IList<VT_SYSTEM> queryAll = QueryForManager(companyId);

						vs = (from fb in queryAll
									join lst in result on fb.Key.ToString().Trim() equals lst.Key.ToString().Trim()
									select fb).ToList();
						count = vs.Count();
						vs = vs.Skip((pIndex - 1) * pSize).Take(pSize).ToList();
					}
				}
			}

			totalCount = count;
			if (count > 0)
			{
				return vs;
			}
			else
			{
				totalCount = 0;
				return null;
			}
		}

		private IList<VT_SYSTEM> QueryForManager(Guid companyId)
		{
			return _systemRepository.GetAll().Where(x => x.DELETE_FLAG == 0 && (x.WorkMode == null || x.WorkMode == 0) && x.CompanyId == companyId).ToList<VT_SYSTEM>();
		}

		public IList<VT_SYSTEM> GetSystemByInstaller(int pIndex, int pSize, string licno, Guid companyId, out int totalCount)
		{
			var querySystems = _systemRepository.GetAll()
				.Where(x => x.LIC_NO == licno && x.DELETE_FLAG == 0 && (x.WorkMode == null || x.WorkMode == 0) && x.CompanyId == companyId)
				.OrderByDescending(x => x.CREATE_DATETIME)
				.Select(x => new
				{
					x.Key,
					x.SYS_SN,
					x.SYS_NAME,
					x.COBAT,
					x.USCAPACITY,
					x.MBAT,
					x.POINV,
					x.POPV,
					x.RemarkSM,
					x.ACDC,
					x.EMSVersion,
					x.BMSVersion,
					x.InvVersion,
					x.MINV,
					x.MMETER,
					x.SetPhase,
					x.SetFeed,
					x.EmsStatus,
					x.LASTUPDATE_DATETIME,
					x.SYS_USER
				});

			if (querySystems.Count() > 0)
			{
				var systems = new List<VT_SYSTEM>();
				foreach (var item in querySystems.ToList())
				{
					systems.Add(new VT_SYSTEM
					{
						Key = item.Key,
						SYS_SN = item.SYS_SN,
						SYS_NAME = item.SYS_NAME,
						COBAT = item.COBAT,
						USCAPACITY = item.USCAPACITY,
						MBAT = item.MBAT,
						POINV = item.POINV,
						POPV = item.POPV,
						RemarkSM = item.RemarkSM,
						ACDC = item.ACDC,
						EMSVersion = item.EMSVersion,
						BMSVersion = item.BMSVersion,
						InvVersion = item.InvVersion,
						MINV = item.MINV,
						MMETER = item.MMETER,
						SetPhase = item.SetPhase,
						SetFeed = item.SetFeed,
						EmsStatus = item.EmsStatus,
						LASTUPDATE_DATETIME = item.LASTUPDATE_DATETIME,
						SYS_USER = item.SYS_USER
					});
				}

				totalCount = querySystems.Count();
				systems = systems.Skip((pIndex - 1) * pSize).Take(pSize).ToList();
				return systems;
			}
			else
			{
				totalCount = 0;
				return null;
			}
		}

		public IList<VT_SYSTEM> GetSystemByUser(int pIndex, int pSize, Guid userId, Guid companyId, out int totalCount)
		{
			var querySystems = _systemRepository.GetAll()
				.Where(x => x.USER_ID == userId && x.DELETE_FLAG == 0 && (x.WorkMode == null || x.WorkMode == 0) && x.CompanyId == companyId)
				.OrderByDescending(x => x.CREATE_DATETIME)
				.Select(x => new
				{
					x.Key,
					x.SYS_SN,
					x.SYS_NAME,
					x.COBAT,
					x.USCAPACITY,
					x.MBAT,
					x.POINV,
					x.POPV,
					x.RemarkSM,
					x.ACDC,
					x.EMSVersion,
					x.BMSVersion,
					x.InvVersion,
					x.MINV,
					x.MMETER,
					x.SetPhase,
					x.SetFeed,
					x.EmsStatus,
					x.LASTUPDATE_DATETIME,
					x.SYS_USER
				});

			if (querySystems.Count() > 0)
			{
				var systems = new List<VT_SYSTEM>();
				foreach (var item in querySystems.ToList())
				{
					systems.Add(new VT_SYSTEM
					{
						Key = item.Key,
						SYS_SN = item.SYS_SN,
						SYS_NAME = item.SYS_NAME,
						COBAT = item.COBAT,
						USCAPACITY = item.USCAPACITY,
						MBAT = item.MBAT,
						POINV = item.POINV,
						POPV = item.POPV,
						RemarkSM = item.RemarkSM,
						ACDC = item.ACDC,
						EMSVersion = item.EMSVersion,
						BMSVersion = item.BMSVersion,
						InvVersion = item.InvVersion,
						MINV = item.MINV,
						MMETER = item.MMETER,
						SetPhase = item.SetPhase,
						SetFeed = item.SetFeed,
						EmsStatus = item.EmsStatus,
						LASTUPDATE_DATETIME = item.LASTUPDATE_DATETIME,
						SYS_USER = item.SYS_USER
					});
				}

				totalCount = querySystems.Count();
				systems = systems.Skip((pIndex - 1) * pSize).Take(pSize).ToList();

				return systems;
			}
			else
			{
				totalCount = 0;
				return null;
			}
		}
		#endregion

	}
}
