using AlphaEss.Api_V2.Infrastructure;
using AlphaEssWeb.Api_V2.Model;
using System;
using System.Collections.Generic;

namespace AlphaEssWeb.Api_V2.Domain.Services
{
	public interface IMembershipService
	{	
		/// <summary>
		/// 验证用户名密码是否正确
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		ValidUserContext ValidateUser(string username, string password, Guid companyId);
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
		UserWithRoles CreateUser(Guid companyId, string licenseNo, string userName, string password, string languageCode,
			string country, string state, string city, string zipCode, string contactUser, string email);
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
		UserWithRoles CreateUser(Guid companyId, string licenseNo, string userName, string password, string languageCode,
			string country, string state, string city, string zipCode, string contactUser, string email, string role);
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
		UserWithRoles CreateUser(Guid companyId, string licenseNo, string userName, string password, string languageCode, string country,
			string state, string city, string zipCode, string contactUser, string email, string[] role);
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
		UserWithRoles UpdateUserInfo(Guid companyId, string username, string language_code, string country, string state, string city, string zipcode, string contact_user, string email, int allow_autoupdate, string cellPhone, string address);
		bool ChangePassword(string username, string newPassword, Guid companyId);
		/// <summary>
		/// 修改密码
		/// </summary>
		/// <param name="username">用户名</param>
		/// <param name="newPassword">密码</param>
		/// <returns>成功:true/失败:false</returns>
		bool ChangePassword(string username, string newPassword);
		/// <summary>
		/// 添加用户到指定角色
		/// </summary>
		/// <param name="userKey">用户编号</param>
		/// <param name="role">要添加到的角色名称</param>
		/// <returns>成功:true/失败:false</returns>
		bool AddToRole(Guid userKey, string role);
		bool AddToRole(string username, string role, Guid companyId);
		/// <summary>
		/// 添加用户到指定角色
		/// </summary>
		/// <param name="username">用户名</param>
		/// <param name="role">要添加到的角色名称</param>
		/// <returns>成功:true/失败:false</returns>
		bool AddToRole(string username, string role);
		/// <summary>
		/// 将用户从角色中移除
		/// </summary>
		/// <param name="username">用户名</param>
		/// <param name="role">角色名称</param>
		/// <returns>成功:true/失败:false</returns>
		bool RemoveFromRole(string username, string role);
		/// <summary>
		/// 获取所有有效角色
		/// </summary>
		/// <returns>角色列表</returns>
		string[] GetRoles(Guid uid);
		/// <summary>
		/// 获取指定编号的角色
		/// </summary>
		/// <param name="key">角色编号</param>
		/// <returns>角色实例</returns>
		SYS_ROLE GetRole(Guid key);
		/// <summary>
		/// 获取指定角色名称的角色
		/// </summary>
		/// <param name="name">角色名</param>
		/// <returns>角色实例</returns>
		SYS_ROLE GetRole(string name);
		/// <summary>
		/// 获取分页功能的用户列表
		/// </summary>
		/// <param name="pageIndex">指定页</param>
		/// <param name="pageSize">每页最大记录数</param>
		/// <returns></returns>
		PaginatedList<UserWithRoles> GetUsers(int pageIndex, int pageSize);
		/// <summary>
		/// 获取指定编号的用户
		/// </summary>
		/// <param name="key">用户编号</param>
		/// <returns>用户实例(带角色)</returns>
		UserWithRoles GetUser(Guid key);
		/// <summary>
		/// 获取指定用户名的用户实例
		/// </summary>
		/// <param name="name">用户名</param>
		/// <returns>用户实例(带角色)</returns>
		UserWithRoles GetUser(string name);
		/// <summary>
		/// 获取指定用户名称的用户实例
		/// </summary>
		/// <param name="name">用户名</param>
		/// <returns>用户实例(带角色及系统列表)</returns>
		UserWithRolesAndSystems GetUserWithRolesAndSystems(string name, Guid companyId);
		SYS_USERAGREEMENT GetUserAgreementByLanguageCode(string lanCode, Guid companyId);
		/// <summary>
		/// 获取指定语言的用户协议
		/// </summary>
		/// <param name="lanCode">语言编号</param>
		/// <returns>用户协议实例</returns>
		//SYS_USERAGREEMENT GetUserAgreementByLanguageCode(string lanCode);
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
		OperationResult<UserWithRoles> Register(string api_account, long timeStamp, string sign, string sn,
			string license_no, string username, string password, string email, int allow_autoupdate, string postcode, string country);
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
		OperationResult<UserWithRoles> UpdateUserinfo(string api_account, long timeStamp, string sign, string username,
			string language_code, string country, string state, string city, string zipcode, string contact_user, string email,
			int allow_autoupdate, string cellPhone, string address, string token);
		/// <summary>
		/// 找回密码
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="username">用户名</param>
		/// <param name="email">邮箱地址</param>
		/// <returns>OperationResult&lt;UserWithRoles&gt;实例</returns>
		OperationResult<UserWithRoles> RetrievePwd(string api_account, long timeStamp, string sign, string username, string email);
	
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
		OperationResult ChangePassword(string api_account, long timeStamp, string sign, string username, string oldPassword, string newPassword, string token);
		/// <summary>
		/// 终端用户,安装商登入
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="username">用户名</param>
		/// <param name="password">密码</param>
		/// <returns>OperationResult实例</returns>
		//OperationResult LoginForCustomerAndInstaller(string api_account, long timeStamp, string sign, string username, string password);
		/// <summary>
		/// 获取指定语言的用户协议
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="lanCode">语言</param>
		/// <returns>OperationResult&lt;SYS_USERAGREEMENT&gt;实例</returns>
		OperationResult<SYS_USERAGREEMENT> GetUserAgreementByLanguageCode(string api_account, long timeStamp, string sign, string lanCode);
		/// <summary>
		/// 获取所有有效用户
		/// </summary>
		/// <returns>IEnumerable&lt;SYS_USER&gt;</returns>
		IEnumerable<SYS_USER> GetAllUsers();
		/// 获取指定用户名的用户实例
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="username">用户名</param>
		/// <returns>OperationResult&lt;UserWithRolesAndSystems&gt;实例</returns>
		OperationResult<UserWithRolesAndSystems> GetUser(string api_account, long timeStamp, string sign, string username, string token);
		
		/// <summary>
		/// 微网用户登入
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="username">用户名</param>
		/// <param name="password">密码</param>
		/// <returns>OperationResult实例</returns>
		//OperationResult LoginForMicrogridManager(string api_account, long timeStamp, string sign, string username, string password);
		OperationResult<UserContext> Login(string api_account, long timeStamp, string sign, string username, string password);
	}
}
