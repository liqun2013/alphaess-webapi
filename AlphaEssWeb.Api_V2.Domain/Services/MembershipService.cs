using AlphaEss.Api_V2.Infrastructure;
using AlphaEssWeb.Api_V2.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace AlphaEssWeb.Api_V2.Domain.Services
{
	public class MembershipService : IMembershipService
	{
		private readonly IEntityRepository<SYS_USER, Guid> _userRepository;
		private readonly IEntityRepository<SYS_ROLE, Guid> _roleRepository;
		private readonly IEntityRepository<SYS_ROLEUSER, Guid> _roleUserRepository;
		private readonly IEntityRepository<SYS_API, Guid> _apiRepository;
		private readonly IEntityRepository<SYS_USERAGREEMENT, Guid> _userAgreementRepository;
		private readonly IEntityRepository<VT_SYSTEM, Guid> _systemRepository;
		private readonly IEntityRepository<SYS_LICENSE, Guid> _licenseRepository;
		private readonly IEntityRepository<SYS_SN, Guid> _snRepository;
		private readonly ICryptoService _cryptoService;
		private readonly IParameterValidateService _parameterValidateService;
		private readonly IEmailService _emailService;

		#region 系统角色名
		public string CustomerRoleName
		{
			get { return "customer"; }
		}
		public string InstallerRoleName
		{
			get { return "installer"; }
		}
		public string ServicerRoleName
		{
			get { return "servicer"; }
		}
		public string SharerRoleName
		{
			get { return "sharer"; }
		}
		public string ManagerRoleName
		{
			get { return "admin"; }
		}
		public string SystemManagerRoleName
		{
			get { return "systemmanager"; }
		}
		public string MicrogridManagerRoleName
		{
			get { return "MicrogridManager"; }
		}
		///// <summary>
		///// 公司id配置在webconfig里
		///// </summary>
		//public Guid CompanyId
		//{
		//	get
		//	{
		//		var companyId = System.Configuration.ConfigurationManager.AppSettings["companyid"];
		//		if (companyId != null && !string.IsNullOrWhiteSpace(companyId))
		//		{
		//			return new Guid(System.Configuration.ConfigurationManager.AppSettings["companyid"]);
		//		}
		//		else
		//			throw new Exception("companyid is empty");
		//	}
		//}
		#endregion

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
			//return string.Equals(_cryptoService.Encrypt(pwd, user.USERNAME), user.USERPWD);
			return string.Equals(_cryptoService.DecryptNew(pwd.Trim(), username), _cryptoService.DecryptNew(user.USERPWD, user.USERNAME));
		}

		/// <summary>
		/// 为注册检查签名
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="sn">sn</param>
		/// <param name="license_no">license</param>
		/// <param name="username">用户名</param>
		/// <param name="password">密码</param>
		/// <param name="email">邮箱地址</param>
		/// <param name="allow_autoupdate">是否允许自动更新</param>
		/// <returns>签名正确:true/签名错误:false</returns>
		private bool checkSignForRegister(string api_account, long timeStamp, string sign, string sn, string license_no,
			string username, string password, string email, int allow_autoupdate, string postcode, string country, string secretKey)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("sn", string.IsNullOrEmpty(sn) ? string.Empty : sn);
			slParams.Add("license_no", string.IsNullOrEmpty(license_no) ? string.Empty : license_no);
			slParams.Add("username", username);
			slParams.Add("password", password.Trim());
			slParams.Add("email", email);
			//slParams.Add("allow_autoupdate", allow_autoupdate.ToString());
			slParams.Add("postcode", postcode);
			slParams.Add("country", country);
			slParams.Add("secretkey", secretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		/// <summary>
		/// 为修改用户信息检查签名
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="username">用户名</param>
		/// <param name="language_code">语言</param>
		/// <param name="country">国家</param>
		/// <param name="state">省/州</param>
		/// <param name="city">城市</param>
		/// <param name="zipcode">邮编</param>
		/// <param name="contact_user">联系人</param>
		/// <param name="email">邮箱地址</param>
		/// <param name="allow_autoupdate">是否允许自动更新</param>
		/// <param name="cellphone">手机号</param>
		/// <returns>签名正确:true/签名错误:false</returns>
		private bool checkSignForUpdateUserInfo(string api_account, long timeStamp, string sign, string username, string language_code,
			string country, string state, string city, string zipcode, string contact_user, string email, int allow_autoupdate, string cellphone, string address, string token, string secretKey)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("language_code", string.IsNullOrEmpty(language_code) ? string.Empty : language_code);
			slParams.Add("country", string.IsNullOrEmpty(country) ? string.Empty : country);
			slParams.Add("state", string.IsNullOrEmpty(state) ? string.Empty : state);
			slParams.Add("city", string.IsNullOrEmpty(city) ? string.Empty : city);
			slParams.Add("zipcode", string.IsNullOrEmpty(zipcode) ? string.Empty : zipcode);
			slParams.Add("contact_user", string.IsNullOrEmpty(contact_user) ? string.Empty : contact_user);
			slParams.Add("username", username);
			slParams.Add("email", email);
			slParams.Add("allow_autoupdate", allow_autoupdate.ToString());
			slParams.Add("secretkey", secretKey);
			slParams.Add("cellphone", string.IsNullOrEmpty(cellphone) ? string.Empty : cellphone);
			slParams.Add("address", string.IsNullOrEmpty(address) ? string.Empty : address);
			slParams.Add("Token", token);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		/// <summary>
		/// 为找回密码检查签名
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="username">用户名</param>
		/// <param name="email">邮箱地址</param>
		/// <returns>签名正确:true/签名错误:false</returns>
		private bool checkSignForRetrievePwd(string api_account, long timeStamp, string sign, string username, string email, string secretKey)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("username", username);
			slParams.Add("email", email);
			slParams.Add("secretkey", secretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		/// <summary>
		/// 为修改密码检查签名
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="username">用户名</param>
		/// <param name="oldpwd">老密码</param>
		/// <param name="newpwd">新密码</param>
		/// <returns>签名正确:true/签名错误:false</returns>
		private bool checkSignForChangePassword(string api_account, long timeStamp, string sign, string username, string oldpwd, string newpwd, string token, string secretKey)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("username", username);
			slParams.Add("oldpwd", oldpwd);
			slParams.Add("newpwd", newpwd);
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
		/// 为登入检查签名
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="username">用户名</param>
		/// <param name="password">密码</param>
		/// <returns>签名正确:true/签名错误:false</returns>
		private bool checkSignForLogin(string api_account, long timeStamp, string sign, string username, string password, string secretKey)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("username", username);
			slParams.Add("password", password);
			slParams.Add("secretkey", secretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		/// <summary>
		/// 为获取用户协议检查签名
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="lanCode">语言</param>
		/// <returns>签名正确:true/签名错误:false</returns>
		private bool checkSignForGetUserAgreementByLanguageCode(string api_account, long timeStamp, string sign, string lanCode, string secretKey)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("language_code", lanCode);
			slParams.Add("secretkey", secretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		/// <summary>
		/// 为获取用户信息检查签名
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="username">用户名</param>
		/// <returns>签名正确:true/签名错误:false</returns>
		private bool checkSignForGetUser(string api_account, long timeStamp, string sign, string username, string token, string secretKey)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("username", username);
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
		/// 将用户添加至角色
		/// </summary>
		/// <param name="user">用户实例</param>
		/// <param name="rolename">角色名</param>
		/// <returns>成功:true/失败:false</returns>
		private bool addUserToRole(SYS_USER user, string rolename)
		{
			var role = _roleRepository.GetSingleByRoleName(rolename);
			if (role != null)
			{
				var roleUser = new SYS_ROLEUSER() { ROLEID = role.Key, USERID = user.Key, Key = Guid.NewGuid() };
				_roleUserRepository.Add(roleUser);
				_roleUserRepository.Save();

				return true;
			}
			else
				return false;
		}

		/// <summary>
		/// 根据用户编号获取角色
		/// </summary>
		/// <param name="userKey">用户编号</param>
		/// <returns>IEnumerable&lt;SYS_ROLE&gt;</returns>
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

		/// <summary>
		/// 根据用户获取用户+角色
		/// </summary>
		/// <param name="user">用户实例</param>
		/// <returns>UserWithRoles实例</returns>
		private UserWithRoles getUserWithRoles(SYS_USER user)
		{
			UserWithRoles result = null;

			if (user != null)
			{
				var roles = getUserRoles(user.Key);
				result = new UserWithRoles { User = user, Roles = roles };
			}

			return result;
		}

		/// <summary>
		/// 检查用户是否已存在
		/// </summary>
		/// <param name="username">用户名</param>
		/// <returns>已存在:true/不存在:false</returns>
		private bool checkUserExist(string username, Guid companyId)
		{
			var result = false;

			if (!string.IsNullOrWhiteSpace(username))
			{
				result = _userRepository.GetSingleByUserName(username, companyId) != null;
			}

			return result;
		}

		/// <summary>
		/// 检查用户是否已存在
		/// </summary>
		/// <param name="username">用户名</param>
		/// <returns>已存在:true/不存在:false</returns>
		private bool checkUserExist(string username)
		{
			var result = false;

			if (!string.IsNullOrWhiteSpace(username))
			{
				result = _userRepository.GetSingleByUserName(username) != null;
			}

			return result;
		}

		/// <summary>
		/// licenseNo是否存在
		/// </summary>
		/// <param name="licenseNo"></param>
		/// <returns></returns>
		private bool isLicenseNoExist(string licenseNo, Guid companyId)
		{
			var result = false;

			if (!string.IsNullOrWhiteSpace(licenseNo))
			{
				result = _licenseRepository.GetSingleByLicenseNo(licenseNo, companyId) != null;
			}

			return result;
		}

		/// <summary>
		/// licenseNo 是否已被占用
		/// </summary>
		/// <param name="licenseNo"></param>
		/// <returns>已被使用：true/未被使用：false</returns>
		private bool isLicenseNoBeUsed(string licenseNo)
		{
			if (string.IsNullOrWhiteSpace(licenseNo))
				throw new ArgumentException("parameter must be a valid string", "licenseNo");

			return _userRepository.GetSingleByLicenseNo(licenseNo) != null;
		}

		/// <summary>
		/// sn是否存在
		/// </summary>
		/// <param name="sn"></param>
		/// <returns>存在：true/不存在：false</returns>
		private bool isSnExist(string sn, Guid companyId)
		{
			if (string.IsNullOrWhiteSpace(sn))
				throw new ArgumentException("parameter must be a valid string", "sn");

			return _snRepository.GetSingleBySn(sn, companyId) != null;
		}

		/// <summary>
		/// sn是否已被注册
		/// </summary>
		/// <param name="sn"></param>
		/// <returns>已被注册：true/未被注册：false</returns>
		private bool isSnBeUsed(string sn, Guid companyId)
		{
			if (string.IsNullOrWhiteSpace(sn))
				throw new ArgumentException("parameter must be a valid string", "sn");

			var u = _systemRepository.GetSystemBySn(sn, companyId);

			if (u == null)
				return false;

			return u.UserId.HasValue;
		}

		/// <summary>
		/// sn是否已被注册
		/// </summary>
		/// <param name="sn"></param>
		/// <returns>已被注册：true/未被注册：false</returns>
		private bool isSnBeUsed(string sn)
		{
			if (string.IsNullOrWhiteSpace(sn))
				throw new ArgumentException("parameter must be a valid string", "sn");

			var u = _systemRepository.GetSystemBySn(sn);

			if (u == null)
				return false;

			return u.UserId.HasValue;
		}

		public MembershipService(IEntityRepository<SYS_USER, Guid> userRepository, IEntityRepository<SYS_ROLE, Guid> roleRepository,
			IEntityRepository<SYS_ROLEUSER, Guid> roleUserRepository, IEntityRepository<SYS_API, Guid> apiRepository, IEntityRepository<SYS_USERAGREEMENT, Guid> userAgreementRepository,
			IEntityRepository<VT_SYSTEM, Guid> systemRepository, IEntityRepository<SYS_LICENSE, Guid> licenseRepository, IEntityRepository<SYS_SN, Guid> snRepository, ICryptoService cryptoService, IParameterValidateService parameterValidateService, IEmailService emailService)
		{
			_userRepository = userRepository;
			_roleRepository = roleRepository;
			_roleUserRepository = roleUserRepository;
			_apiRepository = apiRepository;
			_parameterValidateService = parameterValidateService;
			_systemRepository = systemRepository;
			_licenseRepository = licenseRepository;
			_snRepository = snRepository;
			_cryptoService = cryptoService;
			_userAgreementRepository = userAgreementRepository;
			_emailService = emailService;
		}

		/// <summary>
		/// 创建用户
		/// </summary>
		/// <param name="licenseNo">license</param>
		/// <param name="userName">用户名</param>
		/// <param name="password">密码</param>
		/// <param name="languageCode">语言</param>
		/// <param name="country">国家</param>
		/// <param name="state">省/州</param>
		/// <param name="city">城市</param>
		/// <param name="zipCode">邮编</param>
		/// <param name="contactUser">联系人</param>
		/// <param name="email">邮箱地址</param>
		/// <returns>新加的用户实例(带角色)</returns>
		public UserWithRoles CreateUser(Guid companyId, string licenseNo, string userName, string password, string languageCode,
			string country, string state, string city, string zipCode, string contactUser, string email)
		{
			return CreateUser(companyId, licenseNo, userName, password, languageCode, country, state, city, zipCode, contactUser, email, string.Empty);
		}

		/// <summary>
		/// 创建新用户
		/// </summary>
		/// <param name="licenseNo">license</param>
		/// <param name="userName">用户名</param>
		/// <param name="password">密码</param>
		/// <param name="languageCode">语言</param>
		/// <param name="country">国家</param>
		/// <param name="state">省/州</param>
		/// <param name="city">城市</param>
		/// <param name="zipCode">邮编</param>
		/// <param name="contactUser">联系人</param>
		/// <param name="email">邮箱地址</param>
		/// <param name="role">角色</param>
		/// <returns>新加的用户实例(带角色)</returns>
		public UserWithRoles CreateUser(Guid companyId, string licenseNo, string userName, string password, string languageCode,
			string country, string state, string city, string zipCode, string contactUser, string email, string role)
		{
			return CreateUser(companyId, licenseNo, userName, password, languageCode, country, state, city, zipCode, contactUser, email, new string[] { role });
		}

		/// <summary>
		/// 创建新用户
		/// </summary>
		/// <param name="licenseNo">license</param>
		/// <param name="userName">用户名</param>
		/// <param name="password">密码</param>
		/// <param name="languageCode">语言</param>
		/// <param name="country">国家</param>
		/// <param name="state">省/州</param>
		/// <param name="city">城市</param>
		/// <param name="zipCode">邮编</param>
		/// <param name="contactUser">联系人</param>
		/// <param name="email">邮箱地址</param>
		/// <param name="role">角色</param>
		/// <returns>新加的用户实例(带角色)</returns>
		public UserWithRoles CreateUser(Guid companyId, string licenseNo, string userName, string password, string languageCode,
			string country, string state, string city, string zipCode, string contactUser, string email, string[] role)
		{
			var u = new SYS_USER { LICNO = licenseNo, USERNAME = userName, USERPWD = password, LANGUAGECODE = languageCode, COUNTRYCODE = country, STATECODE = state, CITYCODE = city, POSTCODE = zipCode, LINKMAN = contactUser, EMAIL = email, CREATE_DATETIME = DateTime.Now, Key = Guid.NewGuid(), CompanyId = companyId };
			u.USERTYPE = (role != null && role.Length > 0 ? role[0] : string.Empty);
			_userRepository.Add(u);
			_userRepository.Save();

			if (role != null && role.Length > 0)
			{
				foreach (var r in role)
				{
					addUserToRole(u, r);
				}
			}

			return getUserWithRoles(u);
		}

		/// <summary>
		/// 添加用户到指定角色
		/// </summary>
		/// <param name="username">用户名</param>
		/// <param name="role">要添加到的角色名称</param>
		/// <returns>成功:true/失败:false</returns>
		public bool AddToRole(string username, string role, Guid companyId)
		{
			var result = false;

			var u = _userRepository.GetSingleByUserName(username, companyId);
			if (u != null)
			{
				result = addUserToRole(u, role);
			}

			return result;
		}

		/// <summary>
		/// 添加用户到指定角色
		/// </summary>
		/// <param name="username">用户名</param>
		/// <param name="role">要添加到的角色名称</param>
		/// <returns>成功:true/失败:false</returns>
		public bool AddToRole(string username, string role)
		{
			var result = false;

			var u = _userRepository.GetSingleByUserName(username);
			if (u != null)
			{
				result = addUserToRole(u, role);
			}

			return result;
		}

		/// <summary>
		/// 添加用户到指定角色
		/// </summary>
		/// <param name="userKey">用户编号</param>
		/// <param name="role">要添加到的角色名称</param>
		/// <returns>成功:true/失败:false</returns>
		public bool AddToRole(Guid userKey, string role)
		{
			var result = false;

			var u = _userRepository.GetAll().FirstOrDefault(x => x.Key == userKey); //_userRepository.GetSingle(userKey);
			if (u != null)
			{
				result = addUserToRole(u, role);
			}

			return result;
		}

		/// <summary>
		/// 修改密码
		/// </summary>
		/// <param name="username">用户名</param>
		/// <param name="newPassword">密码</param>
		/// <returns>成功:true/失败:false</returns>
		public bool ChangePassword(string username, string newPassword, Guid companyId)
		{
			var u = _userRepository.GetSingleByUserName(username, companyId);

			if (u != null)
			{
				u.USERPWD = newPassword;
				u.LASTUPDATE_DATETIME = DateTime.Now;
				_userRepository.Edit(u);
				_userRepository.Save();

				return true;
			}

			return false;
		}

		/// <summary>
		/// 修改密码
		/// </summary>
		/// <param name="username">用户名</param>
		/// <param name="newPassword">密码</param>
		/// <returns>成功:true/失败:false</returns>
		public bool ChangePassword(string username, string newPassword)
		{
			var u = _userRepository.GetSingleByUserName(username);

			if (u != null)
			{
				u.USERPWD = newPassword;
				u.LASTUPDATE_DATETIME = DateTime.Now;
				_userRepository.Edit(u);
				_userRepository.Save();

				return true;
			}

			return false;
		}

		/// <summary>
		/// 获取指定角色名称的角色
		/// </summary>
		/// <param name="name">角色名</param>
		/// <returns>角色实例</returns>
		public SYS_ROLE GetRole(string name)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 获取指定编号的角色
		/// </summary>
		/// <param name="key">角色编号</param>
		/// <returns>角色实例</returns>
		public SYS_ROLE GetRole(Guid key)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 获取所有有效角色
		/// </summary>
		/// <returns>角色列表</returns>
		public string[] GetRoles(Guid uid)
		{
			string[] result = null;

			var roleUsers = _roleUserRepository.FindBy(x => x.USERID == uid).ToList();

			if (roleUsers != null && roleUsers.Count > 0)
			{
				var roleKeys = roleUsers.Select(x => x.ROLEID).ToArray();
				result = _roleRepository.FindBy(x => roleKeys.Contains(x.Key)).Select(x => x.ROLENAME).ToArray();
			}

			return result;
		}

		/// <summary>
		/// 获取指定用户名的用户实例
		/// </summary>
		/// <param name="name">用户名</param>
		/// <returns>用户实例(带角色)</returns>
		public UserWithRoles GetUser(string name)
		{
			var u = _userRepository.GetSingleByUserName(name);
			return getUserWithRoles(u);
		}

		/// <summary>
		/// 获取指定用户名称的用户实例
		/// </summary>
		/// <param name="name">用户名</param>
		/// <returns>用户实例(带角色及系统列表)</returns>
		public UserWithRolesAndSystems GetUserWithRolesAndSystems(string name, Guid companyId)
		{
			UserWithRolesAndSystems result = null;

			var u = _userRepository.GetSingleByUserName(name, companyId);
			if (u != null)
			{
				result = new UserWithRolesAndSystems();
				var ur = getUserWithRoles(u);
				var anonySystems = _systemRepository.GetSystemsByUser(u.Key, companyId).Select(x => new { x.AllowAutoUpdate, x.Key, x.Acdc, x.SysSn, x.LicNo, x.CountryCode, x.StateCode, x.CityCode, x.Address, x.PostCode, x.CellPhone, x.EmsStatus, x.Latitude, x.Longitude, x.MoneyType, x.Poinv, x.Popv, x.Cobat, x.Mbat, x.UserId, x.Uscapacity, x.TransFrequency});

				var systems = new List<VT_SYSTEM>();

				if (anonySystems != null && anonySystems.Any())
				{
					foreach (var x in anonySystems)
					{
						systems.Add(new VT_SYSTEM
						{
							AllowAutoUpdate = x.AllowAutoUpdate,
							Key = x.Key,
							Acdc = x.Acdc,
							SysSn = x.SysSn,
							LicNo = x.LicNo,
							CountryCode = x.CountryCode,
							StateCode = x.StateCode,
							CityCode = x.CityCode,
							Address = x.Address,
							PostCode = x.PostCode,
							CellPhone = x.CellPhone,
							EmsStatus = x.EmsStatus,
							Latitude = x.Latitude,
							Longitude = x.Longitude,
							MoneyType = x.MoneyType,
							Poinv = x.Poinv,
							Popv = x.Popv,
							Cobat = x.Cobat,
							Mbat = x.Mbat,
							UserId = x.UserId,
							Uscapacity = x.Uscapacity,
							CompanyId = companyId, TransFrequency= x.TransFrequency
						});
					}
				}
				result.User = u;
				result.Roles = ur.Roles;
				result.Systems = systems;
			}

			return result;
		}

		/// <summary>
		/// 获取指定编号的用户
		/// </summary>
		/// <param name="key">用户编号</param>
		/// <returns>用户实例(带角色)</returns>
		public UserWithRoles GetUser(Guid key)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 获取分页功能的用户列表
		/// </summary>
		/// <param name="pageIndex">指定页</param>
		/// <param name="pageSize">每页最大记录数</param>
		/// <returns></returns>
		public PaginatedList<UserWithRoles> GetUsers(int pageIndex, int pageSize)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 将用户从角色中移除
		/// </summary>
		/// <param name="username">用户名</param>
		/// <param name="role">角色名称</param>
		/// <returns>成功:true/失败:false</returns>
		public bool RemoveFromRole(string username, string role)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 修改用户信息
		/// </summary>
		/// <param name="username">用户名</param>
		/// <param name="language_code">语言</param>
		/// <param name="country">国家</param>
		/// <param name="state">省/州</param>
		/// <param name="city">城市</param>
		/// <param name="zipCode">邮编</param>
		/// <param name="contact_user">联系人</param>
		/// <param name="email">邮箱地址</param>
		/// <param name="allow_autoupdate">是否允许自动更新</param>
		/// <param name="cellPhone">手机</param>
		/// <returns>新加的用户实例(带角色)</returns>
		public UserWithRoles UpdateUserInfo(Guid companyId, string username, string language_code, string country, string state, string city,
			string zipcode, string contact_user, string email, int allow_autoupdate, string cellPhone, string address)
		{
			var user = _userRepository.GetSingleByUserName(username, companyId);
			if (user != null)
			{
				user.LANGUAGECODE = language_code ?? string.Empty;
				user.COUNTRYCODE = country ?? string.Empty;
				user.STATECODE = state ?? string.Empty;
				user.CITYCODE = city ?? string.Empty;
				user.POSTCODE = zipcode ?? string.Empty;
				user.LINKMAN = contact_user ?? string.Empty;
				user.EMAIL = email ?? string.Empty;
				user.LASTUPDATE_DATETIME = DateTime.Now;
				user.CELLPHONE = cellPhone ?? string.Empty;
				user.ADDRESS = address ?? string.Empty;

				var system = _systemRepository.GetSystemsByUser(user.Key, companyId);
				if (system != null && system.Any())
				{
					foreach (var item in system)
					{
						if (item.AllowAutoUpdate == null || !item.AllowAutoUpdate.Equals(allow_autoupdate.ToString()))
						{
							item.AllowAutoUpdate = allow_autoupdate;
							item.LastupdateDatetime = DateTime.Now;
							_systemRepository.Edit(item);
						}
					}
					_systemRepository.Save();
				}

				_userRepository.Edit(user);
				_userRepository.Save();

				return getUserWithRoles(user);
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// 验证用户名密码是否正确
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public ValidUserContext ValidateUser(string username, string password, Guid companyId)
		{
			var vuCtx = new ValidUserContext();

			var user = _userRepository.GetSingleByUserName(username, companyId);
			if (user != null && isUserValid(user, password, username))
			{
				var userRoles = getUserRoles(user.Key);
				vuCtx.User = new UserWithRoles { User = user, Roles = userRoles };

				var identity = new GenericIdentity(user.USERNAME);
				vuCtx.Principal = new GenericPrincipal(identity, userRoles.Select(x => x.ROLENAME).ToArray());
			}

			return vuCtx;
		}

		/// <summary>
		/// 注册普通会员
		/// </summary>
		/// <param name="api_account">接口账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="sn">系统S/N</param>
		/// <param name="username">用户名</param>
		/// <param name="password">密码</param>
		/// <param name="email">邮件地址</param>
		/// <param name="allow_autoupdate">是否允许程序自动升级</param>
		/// <returns>OperationResult&lt;UserWithRoles&gt;实例</returns>
		public OperationResult<UserWithRoles> RegisterCustomer(string api_account, long timeStamp, string sign, string sn, string username,
			string password, string email, int allow_autoupdate, string postcode, string country)
		{
			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult<UserWithRoles>(OperationCode.Error_TimeStamp);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult<UserWithRoles>(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForRegister(api_account, timeStamp, sign, sn, string.Empty, username, password, email, allow_autoupdate, postcode, country, apiAccount.Api_SecretKey))
				return new OperationResult<UserWithRoles>(OperationCode.Error_Sign);

			var theCompanyId = new Guid(apiAccount.CompanyId);
			if (checkUserExist(username, theCompanyId))
				return new OperationResult<UserWithRoles>(OperationCode.Error_UserExist);

			if (!isSnExist(sn, theCompanyId))
				return new OperationResult<UserWithRoles>(OperationCode.Error_SNNotExist);

			if (isSnBeUsed(sn, theCompanyId))
				return new OperationResult<UserWithRoles>(OperationCode.Error_SnBeRegistered);

			var userWithRoles = CreateUser(theCompanyId, string.Empty, username, password, string.Empty, country, string.Empty, string.Empty, postcode, string.Empty, email, CustomerRoleName);
			if (userWithRoles != null)
			{
				var system = _systemRepository.GetSystemBySn(sn, theCompanyId);
				var u = userWithRoles.User;
				if (system != null)
				{
					system.UserId = u.Key;
					system.AllowAutoUpdate = allow_autoupdate;
					system.Workmode = 0;
					system.LastupdateDatetime= DateTime.Now;

					_systemRepository.Edit(system);
					_systemRepository.Save();
				}
				else
				{
					system = new VT_SYSTEM { Key = Guid.NewGuid(), CreateAccount = "webapi", DeleteFlag = 0, CreateDatetime = DateTime.Now, UserId = u.Key, SysSn = sn.ToUpper(), Address= u.ADDRESS, AllowAutoUpdate = allow_autoupdate, Workmode = 0, CompanyId = theCompanyId };
					_systemRepository.Add(system);
					_systemRepository.Save();
				}

				var result = new OperationResult<UserWithRoles>(OperationCode.Success);
				result.Entity = userWithRoles;

				return result;
			}
			else
			{
				return new OperationResult<UserWithRoles>(OperationCode.Error_Unknown);
			}
		}

		/// <summary>
		/// 注册安装商
		/// </summary>
		/// <param name="api_account">接口账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="license_no">license</param>
		/// <param name="username">用户名</param>
		/// <param name="password">密码</param>
		/// <param name="email">邮箱地址</param>
		/// <returns>OperationResult&lt;UserWithRoles&gt;实例</returns>
		public OperationResult<UserWithRoles> RegisterInstaller(string api_account, long timeStamp, string sign, string license_no, string username, string password, string email, string postcode, string country)
		{
			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult<UserWithRoles>(OperationCode.Error_TimeStamp);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult<UserWithRoles>(OperationCode.Error_ApiAccountNotExist);

			var theCompanyId = new Guid(apiAccount.CompanyId);

			if (!checkSignForRegister(api_account, timeStamp, sign, string.Empty, license_no, username, password, email, 0, postcode, country, apiAccount.Api_SecretKey))
				return new OperationResult<UserWithRoles>(OperationCode.Error_Sign);

			if (checkUserExist(username, theCompanyId))
				return new OperationResult<UserWithRoles>(OperationCode.Error_UserExist);

			if (!isLicenseNoExist(license_no, theCompanyId))
				return new OperationResult<UserWithRoles>(OperationCode.Error_LicenseNotExist);

			if (isLicenseNoBeUsed(license_no))
				return new OperationResult<UserWithRoles>(OperationCode.Error_LicenseBeRegistered);

			var userWithRoles = CreateUser(theCompanyId, license_no, username, password, string.Empty, country, string.Empty, string.Empty, string.Empty, string.Empty, email, InstallerRoleName);
			if (userWithRoles != null)
			{
				var result = new OperationResult<UserWithRoles>(OperationCode.Success);
				result.Entity = userWithRoles;

				return result;
			}
			else
			{
				return new OperationResult<UserWithRoles>(OperationCode.Error_Unknown);
			}
		}

		/// <summary>
		/// 注册新用户
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="sn">sn</param>
		/// <param name="license_no">license号</param>
		/// <param name="username">用户名</param>
		/// <param name="password">密码</param>
		/// <param name="email">邮箱地址</param>
		/// <param name="allow_autoupdate">是否允许自动更新</param>
		/// <returns>OperationResult&lt;UserWithRoles&gt;实例</returns>
		public OperationResult<UserWithRoles> Register(string api_account, long timeStamp, string sign, string sn, string license_no, string username, string password, string email, int allow_autoupdate, string postcode, string country)
		{
			if (!string.IsNullOrWhiteSpace(sn))
			{
				return RegisterCustomer(api_account, timeStamp, sign, sn, username, password, email, allow_autoupdate, postcode, country);
			}
			else if (!string.IsNullOrWhiteSpace(license_no))
			{
				return RegisterInstaller(api_account, timeStamp, sign, license_no, username, password, email, postcode, country);
			}
			else
				return new OperationResult<UserWithRoles>(OperationCode.Error_Param);
		}

		/// <summary>
		/// 获取指定语言的用户协议
		/// </summary>
		/// <param name="lanCode">语言编号</param>
		/// <returns>用户协议实例</returns>
		public SYS_USERAGREEMENT GetUserAgreementByLanguageCode(string lanCode, Guid companyId)
		{
			if (!LanguageCodeConstants.CheckLanguageCodeIsExist(lanCode))
			{
				lanCode = LanguageCodeConstants.GetLanguageCodeByName(LanguageNames.English);
			}//lanCode不存在时，去默认英文

			return _userAgreementRepository.GetAll().FirstOrDefault(x => x.Agreement_Language == lanCode && x.CompanyId == companyId);
		}

		//public UserWithRoles UpdateUserInfo(string username, string language_code, string country, string state, string city, string zipcode, string contact_user, string email, int allow_autoupdate)
		//{
		//	throw new NotImplementedException();
		//}

		/// <summary>
		/// 修改用户信息
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="username">用户名</param>
		/// <param name="language_code">语言</param>
		/// <param name="country">国家</param>
		/// <param name="state">省/州</param>
		/// <param name="city">城市</param>
		/// <param name="zipcode">邮编</param>
		/// <param name="contact_user">联系人</param>
		/// <param name="email">邮箱地址</param>
		/// <param name="allow_autoupdate">是否允许自动更新</param>
		/// <param name="cellPhone">手机号</param>
		/// <returns>OperationResult&lt;UserWithRoles&gt;实例</returns>
		public OperationResult<UserWithRoles> UpdateUserinfo(string api_account, long timeStamp, string sign, string username, string language_code, string country, string state, string city, string zipcode, string contact_user, string email, int allow_autoupdate, string cellPhone, string address, string token)
		{
			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult<UserWithRoles>(OperationCode.Error_TimeStamp);

			if (string.IsNullOrWhiteSpace(token))
				return new OperationResult<UserWithRoles>(OperationCode.Error_Param_Empty);

			if (!TokenHelper.CheckToken(token))
				return new OperationResult<UserWithRoles>(OperationCode.Error_TokenExpiration);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult<UserWithRoles>(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForUpdateUserInfo(api_account, timeStamp, sign, username, language_code, country, state, city, zipcode, contact_user, email, allow_autoupdate, cellPhone, address, token, apiAccount.Api_SecretKey))
				return new OperationResult<UserWithRoles>(OperationCode.Error_Sign);

			var theCompanyId = new Guid(apiAccount.CompanyId);
			if (!checkUserExist(username, theCompanyId))
				return new OperationResult<UserWithRoles>(OperationCode.Error_UserNotExist);

			var userWithRoles = UpdateUserInfo(theCompanyId, username, language_code, country, state, city, zipcode, contact_user, email, allow_autoupdate, cellPhone, address);
			if (userWithRoles != null)
			{
				var result = new OperationResult<UserWithRoles>(OperationCode.Success);
				result.Entity = userWithRoles;

				return result;
			}
			else
			{
				return new OperationResult<UserWithRoles>(OperationCode.Error_Unknown);
			}
		}

		/// <summary>
		/// 找回密码
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="username">用户名</param>
		/// <param name="email">邮箱地址</param>
		/// <returns>OperationResult&lt;UserWithRoles&gt;实例</returns>
		public OperationResult<UserWithRoles> RetrievePwd(string api_account, long timeStamp, string sign, string username, string email)
		{
			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult<UserWithRoles>(OperationCode.Error_TimeStamp);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null || string.IsNullOrWhiteSpace(apiAccount.Api_SecretKey))
				return new OperationResult<UserWithRoles>(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForRetrievePwd(api_account, timeStamp, sign, username, email, apiAccount.Api_SecretKey))
				return new OperationResult<UserWithRoles>(OperationCode.Error_Sign);

			var theCompanyId = new Guid(apiAccount.CompanyId);
			var user = _userRepository.GetSingleByUserName(username, theCompanyId);

			if (user == null)
				return new OperationResult<UserWithRoles>(OperationCode.Error_UserNotExist);

			if (!user.EMAIL.Equals(email))
				return new OperationResult<UserWithRoles>(OperationCode.Error_EmailNotMapToTheUser);

			var retrievePwdContent = string.Format("{0}Monitoring/ResetPassword/Default?Hour={1}&UserId={2}", 
				System.Configuration.ConfigurationManager.AppSettings["ALphaEssWebSiteUrl"], 
				System.Web.HttpUtility.UrlEncode(_cryptoService.EncryptNew(((long)(DateTime.UtcNow - new DateTime(2015, 1, 1)).TotalHours).ToString(), apiAccount.Api_SecretKey)), 
				user.Key.ToString());

			_emailService.SendEmail(email, "Retrieve password", body: retrievePwdContent);

			return new OperationResult<UserWithRoles>(OperationCode.Success);
		}

		/// <summary>
		/// 修改密码
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="username">用户名</param>
		/// <param name="oldPassword">旧密码</param>
		/// <param name="newPassword">新密码</param>
		/// <returns>OperationResult实例</returns>
		public OperationResult ChangePassword(string api_account, long timeStamp, string sign, string username, string oldPassword, string newPassword, string token)
		{
			if (string.IsNullOrWhiteSpace(token))
				return new OperationResult(OperationCode.Error_Param_Empty);

			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult(OperationCode.Error_TimeStamp);

			if (!TokenHelper.CheckToken(token))
				return new OperationResult(OperationCode.Error_TokenExpiration);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult<Report_Energy>(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForChangePassword(api_account, timeStamp, sign, username, oldPassword, newPassword, token, apiAccount.Api_SecretKey))
				return new OperationResult(OperationCode.Error_Sign);

			var theCompanyId = new Guid(apiAccount.CompanyId);
			var user = _userRepository.GetSingleByUserName(username);

			if (user == null)
				return new OperationResult(OperationCode.Error_UserNotExist);

			if (!oldPassword.Equals(user.USERPWD))
				return new OperationResult(OperationCode.Error_OldPasswordNotMatch);

			user.USERPWD = newPassword;
			user.LASTUPDATE_DATETIME = DateTime.Now;
			_userRepository.Edit(user);
			_userRepository.Save();

			return new OperationResult(OperationCode.Success);
		}

		/// <summary>
		/// 用户登入
		/// 支持的用户类型: 终端用户,安装商,分享用户,微网用户
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="username">用户名</param>
		/// <param name="password">密码</param>
		/// <returns>OperationResult实例</returns>
		public OperationResult<UserContext> Login(string api_account, long timeStamp, string sign, string username, string password)
		{
			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult<UserContext>(OperationCode.Error_TimeStamp);

			if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
				return new OperationResult<UserContext>(OperationCode.Error_Param_Empty);

			//Guid theCompanyId;
			//if (!_parameterValidateService.ApiAccountExist(api_account, out theCompanyId))
			//	return new OperationResult(OperationCode.Error_ApiAccountNotExist);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null || string.IsNullOrWhiteSpace(apiAccount.Api_SecretKey))
				return new OperationResult<UserContext>(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForLogin(api_account, timeStamp, sign, username, password, apiAccount.Api_SecretKey))
				return new OperationResult<UserContext>(OperationCode.Error_Sign);

			var user = _userRepository.GetSingleByUserName(username, new Guid(apiAccount.CompanyId));

			if (user == null)
				return new OperationResult<UserContext>(OperationCode.Error_UserNotExist);

			if (!isUserValid(user, password, username))
				return new OperationResult<UserContext>(OperationCode.Error_PasswordNotMatch);

			UserContext ucntx = new UserContext();

			var u = GetUser(username);
			if (u != null && u.Roles != null)
			{
				var roles = u.Roles.Select(x => x.ROLENAME);
				if (roles != null && roles.Any())
				{
					ucntx.UserType = string.Join(",", roles);
					ucntx.UserTypes = roles.ToList();
				}
				ucntx.Token = Guid.NewGuid();
				TokenHelper.SetToken(ucntx.Token, u.User.Key, ucntx.UserTypes.ToArray(), DateTime.Now.AddMinutes(TokenHelper.ExpirationTime));
			}

			return new OperationResult<UserContext>(OperationCode.Success, ucntx);
		}

		/// <summary>
		/// 终端用户,安装商登入
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="username">用户名</param>
		/// <param name="password">密码</param>
		/// <returns>OperationResult实例</returns>
		//public OperationResult LoginForCustomerAndInstaller(string api_account, long timeStamp, string sign, string username, string password)
		//{
		//	if (!_parameterValidateService.CheckTimestamp(timeStamp))
		//		return new OperationResult(OperationCode.Error_TimeStamp);

		//	if (!_parameterValidateService.ApiAccountExist(api_account))
		//		return new OperationResult(OperationCode.Error_ApiAccountNotExist);

		//	var user = _userRepository.GetSingleByUserName(username, this.CompanyId);

		//	if (user == null
		//		|| user.USERTYPE.Equals(ManagerRoleName, StringComparison.OrdinalIgnoreCase)
		//		|| user.USERTYPE.Equals(SharerRoleName, StringComparison.OrdinalIgnoreCase)
		//		|| user.USERTYPE.Equals(ServicerRoleName, StringComparison.OrdinalIgnoreCase))
		//		return new OperationResult(OperationCode.Error_UserNotExist);

		//	if (!isUserValid(user, password, username))
		//	{
		//		return new OperationResult(OperationCode.Error_PasswordNotMatch);
		//	}

		//	if (!checkSignForLogin(api_account, timeStamp, sign, username, password))
		//		return new OperationResult(OperationCode.Error_Sign);

		//	return new OperationResult(OperationCode.Success);
		//}

		/// <summary>
		/// 微网用户登入
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="username">用户名</param>
		/// <param name="password">密码</param>
		/// <returns>OperationResult实例</returns>
		//public OperationResult LoginForMicrogridManager(string api_account, long timeStamp, string sign, string username, string password)
		//{
		//	if (!_parameterValidateService.CheckTimestamp(timeStamp))
		//		return new OperationResult(OperationCode.Error_TimeStamp);

		//	if (!_parameterValidateService.ApiAccountExist(api_account))
		//		return new OperationResult(OperationCode.Error_ApiAccountNotExist);

		//	var user = _userRepository.GetSingleByUserName(username, this.CompanyId);

		//	if (user == null || !user.USERTYPE.Equals(MicrogridManagerRoleName, StringComparison.OrdinalIgnoreCase))
		//		return new OperationResult(OperationCode.Error_UserNotExist);

		//	if (!isUserValid(user, password, username))
		//	{
		//		return new OperationResult(OperationCode.Error_PasswordNotMatch);
		//	}

		//	if (!checkSignForLogin(api_account, timeStamp, sign, username, password))
		//		return new OperationResult(OperationCode.Error_Sign);

		//	return new OperationResult(OperationCode.Success);
		//}

		/// <summary>
		/// 获取指定语言的用户协议
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="lanCode">语言</param>
		/// <returns>OperationResult&lt;SYS_USERAGREEMENT&gt;实例</returns>
		public OperationResult<SYS_USERAGREEMENT> GetUserAgreementByLanguageCode(string api_account, long timeStamp, string sign, string lanCode)
		{
			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult<SYS_USERAGREEMENT>(OperationCode.Error_TimeStamp);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult<SYS_USERAGREEMENT>(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForGetUserAgreementByLanguageCode(api_account, timeStamp, sign, lanCode, apiAccount.Api_SecretKey))
				return new OperationResult<SYS_USERAGREEMENT>(OperationCode.Error_Sign);

			var agreement = GetUserAgreementByLanguageCode(lanCode, new Guid(apiAccount.CompanyId));

			if (agreement != null)
			{
				var result = new OperationResult<SYS_USERAGREEMENT>(OperationCode.Success);
				result.Entity = agreement;

				return result;
			}
			else
			{
				return new OperationResult<SYS_USERAGREEMENT>(OperationCode.Error_NoAgreement);
			}
		}

		/// <summary>
		/// 获取所有有效用户
		/// </summary>
		/// <returns>IEnumerable&lt;SYS_USER&gt;</returns>
		public IEnumerable<SYS_USER> GetAllUsers()
		{
			return _userRepository.GetAll().Where(x => x.DELETE_FLAG == 0);
		}

		/// <summary>
		/// 获取指定用户名的用户实例
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="username">用户名</param>
		/// <returns>OperationResult&lt;UserWithRolesAndSystems&gt;实例</returns>
		public OperationResult<UserWithRolesAndSystems> GetUser(string api_account, long timeStamp, string sign, string username, string token)
		{
			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult<UserWithRolesAndSystems>(OperationCode.Error_TimeStamp);

			if (string.IsNullOrWhiteSpace(token))
				return new OperationResult<UserWithRolesAndSystems>(OperationCode.Error_Param_Empty);

			if (!TokenHelper.CheckToken(token))
				return new OperationResult<UserWithRolesAndSystems>(OperationCode.Error_TokenExpiration);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult<UserWithRolesAndSystems>(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForGetUser(api_account, timeStamp, sign, username, token, apiAccount.Api_SecretKey))
				return new OperationResult<UserWithRolesAndSystems>(OperationCode.Error_Sign);

			var u = GetUserWithRolesAndSystems(username, new Guid(apiAccount.CompanyId));
			if (u != null)
			{
				var result = new OperationResult<UserWithRolesAndSystems>(OperationCode.Success);
				result.Entity = u;

				return result;
			}
			else
			{
				return new OperationResult<UserWithRolesAndSystems>(OperationCode.Error_UserNotExist);
			}
		}
	}
}
