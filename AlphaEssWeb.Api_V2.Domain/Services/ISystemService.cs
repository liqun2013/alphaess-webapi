using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Collections.Generic;

namespace AlphaEssWeb.Api_V2.Domain.Services
{
	public interface ISystemService
	{
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
		VT_SYSTEM CreateNewSystem(Guid companyId, Guid userId, string sn, string address, string email, string country, string state, string city, string language, string timeZone, string contactUser, string zipCode, string cellPhone, int allowAutoUpdates);
		/// <summary>
		/// 根据用户名获取系统列表
		/// </summary>
		/// <param name="pageIndex">指定页</param>
		/// <param name="pageSize">每页最大记录数</param>
		/// <param name="username">用户名</param>
		/// <returns>PaginatedList&lt;VT_SYSTEM&gt;实例</returns>
		PaginatedList<VT_SYSTEM> GetSystemByUser(int pageIndex, int pageSize, string username, Guid companyId);
		/// <summary>
		/// 根据用户id获取系统列表
		/// </summary>
		/// <param name="pageIndex">指定页</param>
		/// <param name="pageSize">每页最大记录数</param>
		/// <param name="userId">用户id</param>
		/// <returns>PaginatedList&lt;VT_SYSTEM&gt;实例</returns>
		PaginatedList<VT_SYSTEM> GetSystemByUser(int pageIndex, int pageSize, Guid userId);
		/// <summary>
		/// 根据安装商id获取系统列表
		/// </summary>
		/// <param name="pageIndex">指定页</param>
		/// <param name="pageSize">每页最大记录数</param>
		/// <param name="installerUserId">安装商id</param>
		/// <returns>PaginatedList&lt;VT_SYSTEM&gt;实例</returns>
		PaginatedList<VT_SYSTEM> GetSystemByInstaller(int pageIndex, int pageSize, Guid installerUserId, Guid companyId);
		/// <summary>
		/// 根据安装商名称获取系统列表
		/// </summary>
		/// <param name="pageIndex">指定页</param>
		/// <param name="pageSize">每页最大记录数</param>
		/// <param name="installerUsername">安装商名称</param>
		/// <returns>PaginatedList&lt;VT_SYSTEM&gt;实例</returns>
		PaginatedList<VT_SYSTEM> GetSystemByInstaller(int pageIndex, int pageSize, string installerUsername, Guid companyId);
		/// <summary>
		/// 根据license获取系统列表
		/// </summary>
		/// <param name="pageIndex">指定页</param>
		/// <param name="pageSize">每页最大记录数</param>
		/// <param name="license">license</param>
		/// <returns>PaginatedList&lt;VT_SYSTEM&gt;实例</returns>
		PaginatedList<VT_SYSTEM> GetSystemByLicense(int pageIndex, int pageSize, string license, Guid companyId);
		/// <summary>
		/// 获取指定sn的系统状态
		/// </summary>
		/// <param name="sn">sn</param>
		/// <returns>SystemState</returns>
		//SystemState GetSystemStatus(string sn);
		/// <summary>
		/// 获取指定sn功率数据
		/// </summary>
		/// <param name="sn">sn</param>
		/// <param name="date">日期</param>
		/// <returns>PowerData实例</returns>
		PowerReportData GetPowerDataBySn(string sn, DateTime date, Guid companyId);
		/// <summary>
		/// 根据用户名获取功率数据
		/// </summary>
		/// <param name="username">用户名</param>
		/// <param name="date">日期</param>
		/// <returns>PowerData实例</returns>
		PowerReportData GetPowerDataByUser(string username, DateTime date, Guid companyId);
		/// <summary>
		/// 获取指定sn能量数据
		/// </summary>
		/// <param name="sn">sn</param>
		/// <param name="start">开始日期</param>
		/// <param name="end">结束日期</param>
		/// <returns>EnergyDate实例</returns>
		EnergyReportData GetEnergeDataBySn(string sn, DateTime? start, DateTime? end, Guid companyId, string statisticsby);
		/// <summary>
		/// 根据用户名获取能量数据
		/// </summary>
		/// <param name="username">用户名</param>
		/// <param name="start">开始日期</param>
		/// <param name="end">结束日期</param>
		/// <returns>EnergyDate实例</returns>
		EnergyReportData GetEnergeDataByUser(string username, DateTime? start, DateTime? end, Guid companyId, string statisticsby);
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
		OperationResult<PaginatedList<VT_SYSTEM>> GetSystemByUser(string api_account, long timeStamp, string sign, string token, int pageIndex, int pageSize);
		/// <summary>
		/// 获取系统状态
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="sns">sn号(多个用逗号分隔)</param>
		/// <returns>OperationResult&lt;IEnumerable&lt;SystemState&gt;&gt;实例</returns>
		OperationResult<IEnumerable<SystemState>> GetSystemStatus(string api_account, long timeStamp, string sign, string sns, string token);
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
		OperationResult<PowerReportData> GetPowerData(string api_account, long timeStamp, string sign, string sn, string username, string date, string token);
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
		OperationResult<EnergyReportData> GetEnergeData(string api_account, long timeStamp, string sign, string sn, string username, string start, string end, string statisticsby, string token);
		VT_SYSTEM GetSystemBySn(string sn, Guid companyId);
		PaginatedList<VT_SYSTEM> QueryVtSystemForReseller(Guid? companyId, string resellerId, int pageIndex, int pageSize);
		OperationResult<PaginatedList<Report_Energy>> GetEnergyReportData(string api_account, long timeStamp, string sign, string sn, string theDate, string token);
		OperationResult<PaginatedList<VT_COLDATA>> GetRunningNewData(string api_account, long timeStamp, string sign, string sn, string token);
		OperationResult<PaginatedList<VT_COLDATA>> GetHistoryRunningData(string api_Account, long timeStamp, string sign, string sn, string token, string starttime, string endtime);
		OperationResult<PaginatedList<PowerData>> GetLastPowerData(string api_account, long timeStamp, string sign, string sn, string token);
		OperationResult<Sys_RemoteDispatch> AddRemoteDispatch(string api_account, long timeStamp, string sign, string token, string sn, int activePower, int reactivePower, decimal soc, int status, int controlMode);
		bool UpdateSystem(VT_SYSTEM dbVtSystem, VT_SYSTEM vtSystem, Dictionary<string, string> dicEditProperties);
		OperationResult UpdateVtSystem(string api_account, long timeStamp, string sign, string token, VT_SYSTEM s);
		OperationResult<VT_SYSTEM> GetSystemDetail(string api_account, long timeStamp, string sign, string token, string sn);
		OperationResult BindNewSystem(string api_account, long timeStamp, string sign, string token, string sn, string licNo, string userName, string checkcode);
		OperationResult<ProfitReportData> GetProfitData(string api_account, long timeStamp, string sign, string sn, string username, string start, string end, string statisticsby, string token);
		List<SysWeatherForecast> GetRecentThreeDaysSysWeatherForecastBySn(string sn);
		OperationResult InstallNewSystem(string api_account, long timeStamp, string sign, string token, string sn, string licNo, string checkcode, DateTime installationDate, string customerName, string contactNumber, string contactAddress);
		OperationResult<FirmwareVersionData> GetFirmwareUpdate(string api_account, long timeStamp, string sign, string token, string sn);
		OperationResult UpdateSystemFirmware(string api_account, long timeStamp, string sign, string token, string sn, string category);
		OperationResult<SystemSummaryStatisticsData> GetSystemSummaryStatisticsData(string api_account, long timeStamp, string sign, string token);
	}
}
