using AlphaEss.Api_V2.Infrastructure;
using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace AlphaEssWeb.Api_V2.Domain.Services
{
	public class AlphaESSV2Service : IAlphaESSV2Service
	{
		private readonly IEntityRepository<SYS_USER, Guid> _userRepository;
		private readonly IEntityRepository<VT_SYSTEM, Guid> _systemRepository;
		private readonly IEntityRepository<SYS_USERSERVICEAREA, Guid> _sysUserservicearea;
		private readonly IEntityRepository<Sys_ResellerLicense, long> _sys_ResellerLicense;
		private readonly IEntityRepository<Sys_ServicePartnerSn, long> _sys_ServicePartnerSn;
		private readonly IEntityRepository<SYS_ROLE, Guid> _sys_ROLE;
		private readonly IEntityRepository<SYS_ROLEUSER, Guid> _sys_ROLEUSER;
		private readonly IEntityRepository<SYS_API, Guid> _sysAPIRepository;
		private readonly IParameterValidateService _parameterValidateService;
		private readonly ICryptoService _cryptoService;
		private readonly IEntityRepository<SYS_LOG, Guid> _sysLogService;

		public AlphaESSV2Service() { }
		public AlphaESSV2Service(IEntityRepository<SYS_USER, Guid> userRepository,
			IEntityRepository<VT_SYSTEM, Guid> systemRepository, IEntityRepository<SYS_USERSERVICEAREA, Guid> sysUserservicearea, IEntityRepository<Sys_ResellerLicense, long> sys_ResellerLicense, IEntityRepository<Sys_ServicePartnerSn, long> sys_ServicePartnerSn, IEntityRepository<SYS_ROLE, Guid> sys_ROLE, IEntityRepository<SYS_ROLEUSER, Guid> sys_ROLEUSER, IEntityRepository<SYS_API, Guid> sysAPIRepository, IParameterValidateService parameterValidateService, ICryptoService cryptoService, IEntityRepository<SYS_LOG, Guid> sysLogService)
		{
			_userRepository = userRepository;
			_systemRepository = systemRepository;
			_sysUserservicearea = sysUserservicearea;
			_sys_ResellerLicense = sys_ResellerLicense;
			_sys_ServicePartnerSn = sys_ServicePartnerSn;
			_sys_ROLE = sys_ROLE;
			_sys_ROLEUSER = sys_ROLEUSER;
			_sysAPIRepository = sysAPIRepository;
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

		/// <summary>
		/// 过期时间 配置在webconfig里
		/// </summary>
		public int ExpiryTime
		{
			get
			{
				var expiryTime = System.Configuration.ConfigurationManager.AppSettings["expiryTime"];
				int time = 90;
				int.TryParse(expiryTime, out time);

				if (int.TryParse(expiryTime, out time))
				{
					return time;
				}
				else
				{
					time = 90;
					return time;
				}
			}
		}

		#region 用户登录
		/// <summary>
		/// 用户登录
		/// </summary>
		/// <param name="username">用户名</param>
		/// <param name="password">密码</param>
		/// <returns></returns>
		public OperationResult<UserLogin> LoginForUser(string api_Account, long timeStamp, string sign, string username, string password, string ipAddress)
		{
			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult<UserLogin>(OperationCode.Error_TimeStamp);

			if (!_parameterValidateService.ApiAccountExist(api_Account))
				return new OperationResult<UserLogin>(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForUser(api_Account, timeStamp, sign, username, password))
				return new OperationResult<UserLogin>(OperationCode.Error_Sign);

			var user = _userRepository.GetSingleByUserName(username, this.CompanyId);

			if (user == null || !(user.USERTYPE.Equals(MicrogridManagerRoleName, StringComparison.OrdinalIgnoreCase) || user.USERTYPE.Equals(SharerRoleName, StringComparison.OrdinalIgnoreCase)
				|| user.USERTYPE.Equals(InstallerRoleName, StringComparison.OrdinalIgnoreCase) || user.USERTYPE.Equals(CustomerRoleName, StringComparison.OrdinalIgnoreCase)))
				return new OperationResult<UserLogin>(OperationCode.Error_UserNotExist);

			if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
			{
				var result6 = new OperationResult<UserLogin>(OperationCode.Error_Param_Empty);
				result6.Entity = null;

				var ul = new SYS_LOG { Key = Guid.NewGuid(), CREATE_DATETIME = DateTime.Now, THREAD = OperationCode.Error_Param_Empty.ToString(), LEVEL = LogLevel.ERROR.ToString(), LOGGER = "Services.AlphaESSV2Service", MESSAGE = "参数为空", EXCEPTION = "api_Account:" + api_Account + ",timeStamp:" + timeStamp + ",sign:" + sign + ", username" + username + ", password:" + password + ",ipAddress:" + ipAddress, CREATE_ACCOUNT = "", LOG_CONTENT = "" };
				_sysLogService.Add(ul);
				_sysLogService.Save();

				return result6;
			}

			if (!isUserValid(user, password, username))
			{
				var result5 = new OperationResult<UserLogin>(OperationCode.Error_PasswordNotMatch);
				result5.Entity = null;

				var ul = new SYS_LOG { Key = Guid.NewGuid(), CREATE_DATETIME = DateTime.Now, THREAD = OperationCode.Error_PasswordNotMatch.ToString(), LEVEL = LogLevel.ERROR.ToString(), LOGGER = "Services.AlphaESSV2Service", MESSAGE = "密码不匹配", EXCEPTION = "api_Account:" + api_Account + ",timeStamp:" + timeStamp + ",sign:" + sign + ", username" + username + ", password:" + password + ",ipAddress:" + ipAddress, CREATE_ACCOUNT = "", LOG_CONTENT = "" };
				_sysLogService.Add(ul);
				_sysLogService.Save();

				return result5;
			}

			UserLogin u = null;
			try
			{
				var query = from a1 in _sys_ROLE.GetAll()
										join r1 in _sys_ROLEUSER.GetAll() on a1.Key equals r1.ROLEID
										where r1.USERID == user.Key
										select new
										{
											userType = a1.ROLENAME
										};

				string userTypes = string.Empty;
				foreach (var q in query)
				{
					userTypes += q.userType + ",";
				}
				if (userTypes.Length > 0)
				{ userTypes = userTypes.Substring(0, userTypes.Length - 1); }

				//过期时间
				DateTime expiryTime = DateTime.Now.AddMinutes(ExpiryTime);

				Guid token = Guid.NewGuid();
				TokenService.SetCache(token, user.Key, expiryTime, ipAddress);

				u = new UserLogin() { userType = userTypes, token = token };
			}
			catch (Exception ex)
			{
				var ul = new SYS_LOG { Key = Guid.NewGuid(), CREATE_DATETIME = DateTime.Now, THREAD = OperationCode.Error_Unknown.ToString(), LEVEL = LogLevel.ERROR.ToString(), LOGGER = "Services.AlphaESSV2Service", MESSAGE = "失败", EXCEPTION = ex.ToString(), CREATE_ACCOUNT = "", LOG_CONTENT = "" };
				_sysLogService.Add(ul);
				_sysLogService.Save();
			}

			var result = new OperationResult<UserLogin>(OperationCode.Success);
			result.Entity = u;

			return result;
		}

		private bool checkSignForUser(string api_account, long timeStamp, string sign, string username, string password)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("username", username);
			slParams.Add("password", password);
			slParams.Add("secretkey", _cryptoService.GetSecretKey(api_account));

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		/// <summary>
		/// 判断用户名密码与指定用户是否匹配
		/// </summary>
		/// <param name="user">指定用户</param>
		/// <param name="pwd">密码</param>
		/// <param name="username">用户名</param>
		/// <returns>匹配:true/不匹配:false</returns>
		private bool isUserValid(SYS_USER user, string pwd, string username)
		{
			if (isPasswordValid(user, pwd, username))
			{
				return user.DELETE_FLAG != 1;
			}

			return false;
		}

		/// <summary>
		/// 判断用户名密码与指定用户是否匹配
		/// </summary>
		/// <param name="user">指定用户</param>
		/// <param name="pwd">密码</param>
		/// <param name="username">用户名</param>
		/// <returns>匹配:true/不匹配:false</returns>
		private bool isPasswordValid(SYS_USER user, string pwd, string username)
		{
			return string.Equals(_cryptoService.DecryptNew(pwd.Trim(), username), _cryptoService.DecryptNew(user.USERPWD, user.USERNAME));
		}
		#endregion

		#region 获取系统列表
		public OperationResult<PaginatedList<VT_SYSTEM>> GetSystemListForUser(string api_Account, long timeStamp, string sign, string token, int? pageIndex, int? pageSize, string ipAddress)
		{
			Guid key = Guid.Empty;
			Guid.TryParse(token, out key);
			var userCache = TokenService.GetCache(key);

			#region 判断
			if (userCache == null || userCache.ExpirationTime < DateTime.Now)
			{
				return new OperationResult<PaginatedList<VT_SYSTEM>>(OperationCode.Error_LoginFailed);
			}

			if (userCache.ipAddress != ipAddress)
			{
				var ul = new SYS_LOG { Key = Guid.NewGuid(), CREATE_DATETIME = DateTime.Now, THREAD = OperationCode.Error_UserChangesDevice.ToString(), LEVEL = LogLevel.ERROR.ToString(), LOGGER = "Services.AlphaESSV2Service", MESSAGE = "用户更换设备", EXCEPTION = "ipAddress_1:" + userCache.ipAddress + ",ipAddress:" + ipAddress, CREATE_ACCOUNT = "", LOG_CONTENT = "" };
				_sysLogService.Add(ul);
				_sysLogService.Save();

				return new OperationResult<PaginatedList<VT_SYSTEM>>(OperationCode.Error_UserChangesDevice);
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

			if (!checkSignForSystemList(api_Account, timeStamp, sign, token, pageIndex, pageSize))
			{
				string secretKey = _cryptoService.GetSecretKey(api_Account);
				var ul = new SYS_LOG { Key = Guid.NewGuid(), CREATE_DATETIME = DateTime.Now, THREAD = OperationCode.Error_Sign.ToString(), LEVEL = LogLevel.ERROR.ToString(), LOGGER = "Services.AlphaESSV2Service", MESSAGE = "签名错误", EXCEPTION = "api_Account:" + api_Account + ",timeStamp:" + timeStamp + ",sign:" + sign + ",token:" + token + ", pageIndex:" + pageIndex + ",pageSize:" + pageSize + ",ipAddress:" + ipAddress + ",SecretKey:" + secretKey, CREATE_ACCOUNT = "", LOG_CONTENT = "" };
				_sysLogService.Add(ul);
				_sysLogService.Save();

				return new OperationResult<PaginatedList<VT_SYSTEM>>(OperationCode.Error_Sign);
			}

			if (!_parameterValidateService.CheckTimestamp(timeStamp))
			{
				var ul = new SYS_LOG { Key = Guid.NewGuid(), CREATE_DATETIME = DateTime.Now, THREAD = OperationCode.Error_TimeStamp.ToString(), LEVEL = LogLevel.ERROR.ToString(), LOGGER = "Services.AlphaESSV2Service", MESSAGE = "时间戳错误", EXCEPTION = "api_Account:" + api_Account + ",timeStamp:" + timeStamp + ",sign:" + sign + ",token:" + token + ", pageIndex:" + pageIndex + ",pageSize:" + pageSize + ",ipAddress:" + ipAddress, CREATE_ACCOUNT = "", LOG_CONTENT = "" };
				_sysLogService.Add(ul);
				_sysLogService.Save();

				return new OperationResult<PaginatedList<VT_SYSTEM>>(OperationCode.Error_TimeStamp);
			}

			if (!_parameterValidateService.ApiAccountExist(api_Account))
			{
				var ul = new SYS_LOG { Key = Guid.NewGuid(), CREATE_DATETIME = DateTime.Now, THREAD = OperationCode.Error_ApiAccountNotExist.ToString(), LEVEL = LogLevel.ERROR.ToString(), LOGGER = "Services.AlphaESSV2Service", MESSAGE = "api账户不存在", EXCEPTION = "api_Account:" + api_Account + ",timeStamp:" + timeStamp + ",sign:" + sign + ",token:" + token + ", pageIndex:" + pageIndex + ",pageSize:" + pageSize + ",ipAddress:" + ipAddress, CREATE_ACCOUNT = "", LOG_CONTENT = "" };
				_sysLogService.Add(ul);
				_sysLogService.Save();

				return new OperationResult<PaginatedList<VT_SYSTEM>>(OperationCode.Error_ApiAccountNotExist);
			}
			#endregion

			int pIndex = pageIndex == null ? 1 : (int)pageIndex;
			int pSize = pageSize == null ? 10 : (int)pageSize;
			int totalCount = 0;

			#region 获取用户系统列表
			IList<VT_SYSTEM> result = new List<VT_SYSTEM>();
			PaginatedList<VT_SYSTEM> systems = new PaginatedList<VT_SYSTEM>(pIndex, pSize, null);

			if (user_Types.Contains("customer"))
			{
				result = GetSystemByUser(pIndex, pSize, userCache.UserId, this.CompanyId, out totalCount);
			}
			else if (user_Types.Contains("installer"))
			{
				SYS_USER user = _userRepository.GetAll().Where(x => x.Key == userCache.UserId).FirstOrDefault();
				if (user != null)
				{
					result = GetSystemByInstaller(pIndex, pSize, user.LICNO, this.CompanyId, out totalCount);
				}
			}
			else if (user_Types.Contains("servicer"))
			{
				result = GetSystemByServicer(pIndex, pSize, userCache.UserId, this.CompanyId, out totalCount);
			}
			else if (user_Types.Contains("admin"))
			{
				result = GetSystemByManager(pIndex, pSize, this.CompanyId, out totalCount);
			}
			else if (user_Types.Contains("systemmanager"))
			{
				result = GetSystemByManager(pIndex, pSize, this.CompanyId, out totalCount);
			}
			else if (user_Types.Contains("reseller"))
			{
				result = GetSystemByReseller(pIndex, pSize, userCache.UserId, this.CompanyId, out totalCount);
			}
			else if (user_Types.Contains("servicepartner"))
			{
				result = GetSystemByServicepartner(pIndex, pSize, userCache.UserId, this.CompanyId, out totalCount);
			}
			#endregion

			if (result != null && totalCount > 0)
			{
				systems = new PaginatedList<VT_SYSTEM>(pIndex, pSize, result.AsQueryable());
				systems.TotalCount = totalCount;
				systems.TotalPageCount = (int)Math.Ceiling(systems.TotalCount / (double)pageSize);
			}
			else
			{
				systems = null;
			}

			var plSystems = new OperationResult<PaginatedList<VT_SYSTEM>>(OperationCode.Success);
			plSystems.Entity = systems;

			return plSystems;
		}

		private bool checkSignForSystemList(string api_Account, long timeStamp, string sign, string token, int? pageIndex, int? pageSize)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_Account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("Token", token);
			slParams.Add("pageindex", pageIndex);
			slParams.Add("pagesize", pageSize);
			slParams.Add("secretkey", _cryptoService.GetSecretKey(api_Account));

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

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
						LastUploadTime = item.LastUploadTime,
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
						SYS_USER = item.SYS_USER,
						LastUploadTime = item.LastUploadTime
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
						LastUploadTime = item.LastUploadTime,
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
								SYS_USER = item.SYS_USER,
								LastUploadTime = item.LastUploadTime
							});
						}
					}
				}
				else
				{
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
					x.SYS_USER,
					x.LastUploadTime
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
						LastUploadTime = item.LastUploadTime,
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
					x.SYS_USER,
					x.LastUploadTime
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
						LastUploadTime = item.LastUploadTime,
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
		#endregion
	}
}
