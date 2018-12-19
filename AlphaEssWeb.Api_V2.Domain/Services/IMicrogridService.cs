using AlphaEss.Api_V2.Infrastructure;
using System;

namespace AlphaEssWeb.Api_V2.Domain.Services
{
	public interface IMicrogridService
	{
		/// <summary>
		/// 根据用户获取微网信息
		/// </summary>
		/// <param name="userId">用户编号</param>
		/// <returns>MicrogridInfo实例</returns>
		MicrogridInfo GetMicrogridInfoByUserId(Guid userId);
		/// <summary>
		/// 获取指定微网id的调度策略
		/// </summary>
		/// <param name="microgridId">微网编号</param>
		/// <returns>SchedulingStrategy实例</returns>
		SchedulingStrategy GetSchedulingStrategyByMicrogridId(Guid microgridId);
		/// <summary>
		/// 更新调度策略
		/// </summary>
		/// <returns>成功:true/失败:false</returns>
		bool UpdateSchedulingStrategy(Guid microgridId, decimal pGridMax, decimal pOutputMax, decimal dieselStartSOC, decimal dieselPOutputMax,
			decimal dieselStopSOC, decimal dieselStopPower, decimal soc1, decimal soc2, decimal soc3, decimal soc4, decimal soc5, decimal soc6, decimal soc7,
			decimal power1, decimal power2, decimal power3, decimal power4, decimal power5, int chargingStart1, int chargingEnd1, int chargingStart2,
			int chargingEnd2, int dischargeStart1, int dischargeEnd1, int dischargeStart2, int dischargeEnd2, decimal chargingSOCPoint1, decimal chargingSOCPoint2,
			decimal dischargeSOCPoint1, decimal dischargeSOCPoint2);
		/// <summary>
		/// 获取知道微网汇总信息
		/// </summary>
		/// <param name="microgridId">微网编号</param>
		/// <returns>MicrogridSummary实例</returns>
		MicrogridSummary GetMicrogridSummary(Guid microgridId);
		/// <summary>
		/// 获取指定用户的微网信息
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="username">用户名</param>
		/// <returns>OperationResult&lt;MicrogridInfo&gt;实例</returns>
		OperationResult<MicrogridInfo> GetMicrogridInfoByUserId(string api_account, long timeStamp, string sign, string username);
		/// <summary>
		/// 获取指定微网调度策略
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="microgridId">微网编号</param>
		/// <returns>OperationResult&lt;SchedulingStrategy&gt;</returns>
		OperationResult<SchedulingStrategy> GetSchedulingStrategyByMicrogridId(string api_account, long timeStamp, string sign, Guid microgridId);
		/// <summary>
		/// 修改调度策略
		/// </summary>
		/// <returns>OperationResult实例</returns>
		OperationResult UpdateSchedulingStrategy(string api_account, long timeStamp, string sign, Guid microgridId, decimal pGridMax, decimal pOutputMax, decimal dieselStartSOC, decimal dieselPOutputMax,
					decimal dieselStopSOC, decimal dieselStopPower, decimal soc1, decimal soc2, decimal soc3, decimal soc4, decimal soc5, decimal soc6, decimal soc7,
					decimal power1, decimal power2, decimal power3, decimal power4, decimal power5, int chargingStart1, int chargingEnd1, int chargingStart2,
					int chargingEnd2, int dischargeStart1, int dischargeEnd1, int dischargeStart2, int dischargeEnd2, decimal chargingSOCPoint1, decimal chargingSOCPoint2,
					decimal dischargeSOCPoint1, decimal dischargeSOCPoint2);
		/// <summary>
		/// 获取微网汇总信息
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="microgridId">微网编号</param>
		/// <returns>OperationResult&lt;MicrogridSummary&gt;</returns>
		OperationResult<MicrogridSummary> GetMicrogridSummary(string api_account, long timeStamp, string sign, Guid microgridId);
		/// <summary>
		/// 更新微网状态
		/// </summary>
		/// <param name="microgridId">微网id</param>
		/// <param name="command">指令</param>
		/// <returns>成功:true/失败:false</returns>
		bool ChangeMicrogridStateCommand(Guid microgridId, int command);
		/// <summary>
		/// 更新微网状态
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="microgridId">微网编号</param>
		/// <param name="command">指令</param>
		/// <returns>OperationResult实例</returns>
		OperationResult ChangeMicrogridStateCommand(string api_account, long timeStamp, string sign, Guid microgridId, int command);
		/// <summary>
		/// 发送电力调度信息
		/// </summary>
		/// <param name="microgridId">微网编号</param>
		/// <param name="cmdIndex">命令识别码</param>
		/// <param name="controlPower">控制功率</param>
		/// <returns>成功:true/失败:false</returns>
		bool SendPowerDispatchingCommand(Guid microgridId, string cmdIndex, decimal controlPower);
		/// <summary>
		/// 发送电力调度信息
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="microgridId">微网编号</param>
		/// <param name="cmdIndex">命令识别码</param>
		/// <param name="controlPower">控制功率</param>
		/// <returns>OperationResult实例</returns>
		OperationResult SendPowerDispatchingCommand(string api_account, long timeStamp, string sign, Guid microgridId, string cmdIndex, decimal controlPower);
	}
}
