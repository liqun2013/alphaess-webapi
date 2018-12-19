using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace AlphaEssWeb.Api_V2.Domain.Services
{
	public class MicrogridService : IMicrogridService
	{
		private readonly IEntityRepository<MicrogridInfo, Guid> _microgridInfoRepository;
		private readonly IEntityRepository<SchedulingStrategy, Guid> _schedulingStrategyRepository;
		private readonly IEntityRepository<MicrogridSummary, Guid> _microgridSummaryRepository;
		private readonly IEntityRepository<SYS_USER, Guid> _userRepository;
		private readonly IParameterValidateService _parameterValidateService;
		private readonly IEntityRepository<SYS_API, Guid> _apiRepository;
		private readonly ICryptoService _cryptoService;
		private readonly IAlphaRemotingService _alphaRemotingService;

		public MicrogridService(IEntityRepository<MicrogridInfo, Guid> microgridInfoRepository, IEntityRepository<SchedulingStrategy, Guid> schedulingStrategyRepository, IEntityRepository<MicrogridSummary, Guid> microgridSummaryRepository,
			IEntityRepository<SYS_USER, Guid> userRepository, IEntityRepository<SYS_API, Guid> apiRepository, IParameterValidateService parameterValidateService, ICryptoService cryptoService, IAlphaRemotingService alphaRemotingService)
		{
			_microgridInfoRepository = microgridInfoRepository;
			_schedulingStrategyRepository = schedulingStrategyRepository;
			_microgridSummaryRepository = microgridSummaryRepository;
			_userRepository = userRepository;
			_parameterValidateService = parameterValidateService;
			_cryptoService = cryptoService;
			_alphaRemotingService = alphaRemotingService;
			_apiRepository = apiRepository;
		}

		public string MicrogridManagerRoleName
		{
			get { return "MicrogridManager"; }
		}

		#region check sign

		/// <summary>
		/// 为获取微网信息检查签名
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="username">用户名</param>
		/// <returns>签名正确:true/签名错误:false</returns>
		private bool checkSignForGetMicrogridInfo(string api_account, long timeStamp, string sign, string username, string secretKey)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("username", username);
			slParams.Add("secretkey", secretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		/// <summary>
		/// 为获取调度策略检查签名
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="microgridId">微网id</param>
		/// <returns>签名正确:true/签名错误:false</returns>
		private bool checkSignForGetSchedulingStrategy(string api_account, long timeStamp, string sign, Guid microgridId, string secretKey)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("microgridid", microgridId.ToString());
			slParams.Add("secretkey", secretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		/// <summary>
		/// 为修改调度策略检查签名
		/// </summary>
		/// <returns>签名正确:true/签名错误:false</returns>
		private bool checkSignForUpdateSchedulingStrategy(string api_account, long timeStamp, string sign, Guid microgridId, decimal pGridMax, decimal pOutputMax, decimal dieselStartSOC, decimal dieselPOutputMax,
			decimal dieselStopSOC, decimal dieselStopPower, decimal soc1, decimal soc2, decimal soc3, decimal soc4, decimal soc5, decimal soc6, decimal soc7,
			decimal power1, decimal power2, decimal power3, decimal power4, decimal power5, int chargingStart1, int chargingEnd1, int chargingStart2,
			int chargingEnd2, int dischargeStart1, int dischargeEnd1, int dischargeStart2, int dischargeEnd2, decimal chargingSOCPoint1, decimal chargingSOCPoint2,
			decimal dischargeSOCPoint1, decimal dischargeSOCPoint2, string secretKey)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("MicrogridId", microgridId.ToString());
			slParams.Add("PGridMax", pGridMax);
			slParams.Add("POutputMax", pOutputMax);
			slParams.Add("DieselStartSOC", dieselStartSOC);
			slParams.Add("DieselPOutputMax", dieselPOutputMax);
			slParams.Add("DieselStopSOC", dieselStopSOC);
			slParams.Add("DieselStopPower", dieselStopPower);
			slParams.Add("SOC1", soc1.ToString());
			slParams.Add("SOC2", soc2.ToString());
			slParams.Add("SOC3", soc3.ToString());
			slParams.Add("SOC4", soc4.ToString());
			slParams.Add("SOC5", soc5.ToString());
			slParams.Add("SOC6", soc6.ToString());
			slParams.Add("SOC7", soc7.ToString());
			slParams.Add("Power1", power1.ToString());
			slParams.Add("Power2", power2.ToString());
			slParams.Add("Power3", power3.ToString());
			slParams.Add("Power4", power4.ToString());
			slParams.Add("Power5", power5.ToString());
			slParams.Add("ChargingStart1", chargingStart1.ToString());
			slParams.Add("ChargingEnd1", chargingEnd1.ToString());
			slParams.Add("ChargingStart2", chargingStart2.ToString());
			slParams.Add("ChargingEnd2", chargingEnd2.ToString());
			slParams.Add("DischargeStart1", dischargeStart1.ToString());
			slParams.Add("DischargeEnd1", dischargeEnd1.ToString());
			slParams.Add("DischargeStart2", dischargeStart2.ToString());
			slParams.Add("DischargeEnd2", dischargeEnd2.ToString());
			slParams.Add("ChargingSOCPoint1", chargingSOCPoint1.ToString());
			slParams.Add("ChargingSOCPoint2", chargingSOCPoint2.ToString());
			slParams.Add("DischargeSOCPoint1", dischargeSOCPoint1.ToString());
			slParams.Add("DischargeSOCPoint2", dischargeSOCPoint2.ToString());
			slParams.Add("secretkey", secretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		/// <summary>
		/// 为获取微网汇总信息检查签名
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="microgridId">微网id</param>
		/// <returns>签名正确:true/签名错误:false</returns>
		private bool checkSignForGetMicrogridSummary(string api_account, long timeStamp, string sign, Guid microgridId, string secretKey)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("microgridid", microgridId.ToString());
			slParams.Add("secretkey", secretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		/// <summary>
		/// 为更新微网状态检查签名
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="microgridId">微网id</param>
		/// <param name="command">指令</param>
		/// <returns>签名正确:true/签名错误:false</returns>
		private bool checkSignForChangeMicrogridStateCommand(string api_account, long timeStamp, string sign, Guid microgridId, int command, string secretKey)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("MicrogridId", microgridId.ToString());
			slParams.Add("Command", command.ToString());
			slParams.Add("secretkey", secretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		/// <summary>
		/// 为发送电力调度信息检查签名
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="microgridId">微网id</param>
		/// <param name="cmdIndex">命令识别码</param>
		/// <param name="controlPower">控制功率</param>
		/// <returns>签名正确:true/签名错误:false</returns>
		private bool checkSignForSendPowerDispatchingCommand(string api_account, long timeStamp, string sign, Guid microgridId, string cmdIndex, decimal controlPower, string secretKey)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("MicrogridId", microgridId.ToString());
			slParams.Add("CmdIndex", cmdIndex);
			slParams.Add("ControlPower", controlPower.ToString());
			slParams.Add("secretkey", secretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		#endregion

		/// <summary>
		/// 根据用户获取微网信息
		/// </summary>
		/// <param name="userId">用户编号</param>
		/// <returns>MicrogridInfo实例</returns>
		public MicrogridInfo GetMicrogridInfoByUserId(Guid userId)
		{
			return _microgridInfoRepository.GetAll().FirstOrDefault(x => x.UserId == userId && x.IsDelete == 0);
		}

		/// <summary>
		/// 获取指定微网id的调度策略
		/// </summary>
		/// <param name="microgridId">微网编号</param>
		/// <returns>SchedulingStrategy实例</returns>
		public SchedulingStrategy GetSchedulingStrategyByMicrogridId(Guid microgridId)
		{
			return _schedulingStrategyRepository.GetAll().FirstOrDefault(x => x.MicrogridId == microgridId && x.IsDelete == 0);
		}

		/// <summary>
		/// 更新调度策略
		/// </summary>
		/// <returns>成功:true/失败:false</returns>
		public bool UpdateSchedulingStrategy(Guid microgridId, decimal pGridMax, decimal pOutputMax, decimal dieselStartSOC, decimal dieselPOutputMax,
			decimal dieselStopSOC, decimal dieselStopPower, decimal soc1, decimal soc2, decimal soc3, decimal soc4, decimal soc5, decimal soc6, decimal soc7,
			decimal power1, decimal power2, decimal power3, decimal power4, decimal power5, int chargingStart1, int chargingEnd1, int chargingStart2,
			int chargingEnd2, int dischargeStart1, int dischargeEnd1, int dischargeStart2, int dischargeEnd2, decimal chargingSOCPoint1, decimal chargingSOCPoint2,
			decimal dischargeSOCPoint1, decimal dischargeSOCPoint2)
		{
			var result = false;
			var theSchedulingStrategy = _schedulingStrategyRepository.GetSingleByMicrogridId(microgridId);
			if (theSchedulingStrategy != null)
			{
				try
				{
					theSchedulingStrategy.ChargingEnd1 = chargingEnd1;
					theSchedulingStrategy.ChargingEnd2 = chargingEnd2;
					theSchedulingStrategy.ChargingSOCPoint1 = chargingSOCPoint1;
					theSchedulingStrategy.ChargingSOCPoint2 = chargingSOCPoint2;
					theSchedulingStrategy.ChargingStart1 = chargingStart1;
					theSchedulingStrategy.ChargingStart2 = chargingStart2;
					theSchedulingStrategy.DieselPOutputMax = dieselPOutputMax;
					theSchedulingStrategy.DieselStartSOC = dieselStartSOC;
					theSchedulingStrategy.DieselStopPower = dieselStopPower;
					theSchedulingStrategy.DieselStopSOC = dieselStopSOC;
					theSchedulingStrategy.DischargeEnd1 = dischargeEnd1;
					theSchedulingStrategy.DischargeEnd2 = dischargeEnd2;
					theSchedulingStrategy.DischargeSOCPoint1 = dischargeSOCPoint1;
					theSchedulingStrategy.DischargeSOCPoint2 = dischargeSOCPoint2;
					theSchedulingStrategy.DischargeStart1 = dischargeStart1;
					theSchedulingStrategy.DischargeStart2 = dischargeStart2;
					theSchedulingStrategy.MicrogridId = microgridId;
					theSchedulingStrategy.PGridMax = pGridMax;
					theSchedulingStrategy.POutputMax = pOutputMax;
					theSchedulingStrategy.Power1 = power1;
					theSchedulingStrategy.Power2 = power2;
					theSchedulingStrategy.Power3 = power3;
					theSchedulingStrategy.Power4 = power4;
					theSchedulingStrategy.Power5 = power5;
					theSchedulingStrategy.SOC1 = soc1;
					theSchedulingStrategy.SOC2 = soc2;
					theSchedulingStrategy.SOC3 = soc3;
					theSchedulingStrategy.SOC4 = soc4;
					theSchedulingStrategy.SOC5 = soc5;
					theSchedulingStrategy.SOC6 = soc6;
					theSchedulingStrategy.SOC7 = soc7;
					theSchedulingStrategy.UpdateTime = DateTime.Now;

					_schedulingStrategyRepository.Edit(theSchedulingStrategy);
					_schedulingStrategyRepository.Save();
					result = true;
				}
				catch
				{
					throw;
				}
			}

			return result;
		}

		/// <summary>
		/// 获取知道微网汇总信息
		/// </summary>
		/// <param name="microgridId">微网编号</param>
		/// <returns>MicrogridSummary实例</returns>
		public MicrogridSummary GetMicrogridSummary(Guid microgridId)
		{
			var ms = _microgridSummaryRepository.GetAll();
			MicrogridSummary result = null;
			if (ms.Count() > 0)
			{
				result = _microgridSummaryRepository.GetAll().OrderByDescending(x => x.CreateTime).FirstOrDefault(x => x.MicrogridId == microgridId);
			}

			return result;
		}

		/// <summary>
		/// 获取指定用户的微网信息
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="username">用户名</param>
		/// <returns>OperationResult&lt;MicrogridInfo&gt;实例</returns>
		public OperationResult<MicrogridInfo> GetMicrogridInfoByUserId(string api_account, long timeStamp, string sign, string username)
		{
			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult<MicrogridInfo>(OperationCode.Error_TimeStamp);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult<MicrogridInfo>(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForGetMicrogridInfo(api_account, timeStamp, sign, username, apiAccount.Api_SecretKey))
				return new OperationResult<MicrogridInfo>(OperationCode.Error_Sign);

			var user = _userRepository.GetSingleByUserName(username);

			if (user == null || !user.USERTYPE.Equals(MicrogridManagerRoleName, StringComparison.OrdinalIgnoreCase))
				return new OperationResult<MicrogridInfo>(OperationCode.Error_UserNotExist);

			var mi = GetMicrogridInfoByUserId(user.Key);

			if (mi != null)
			{
				return new OperationResult<MicrogridInfo>(OperationCode.Success, mi);
			}
			else
			{
				return new OperationResult<MicrogridInfo>(OperationCode.Error_NoMicrogridInfo);
			}
		}

		/// <summary>
		/// 获取指定微网调度策略
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="microgridId">微网编号</param>
		/// <returns>OperationResult&lt;SchedulingStrategy&gt;</returns>
		public OperationResult<SchedulingStrategy> GetSchedulingStrategyByMicrogridId(string api_account, long timeStamp, string sign, Guid microgridId)
		{
			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult<SchedulingStrategy>(OperationCode.Error_TimeStamp);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult<SchedulingStrategy>(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForGetSchedulingStrategy(api_account, timeStamp, sign, microgridId, apiAccount.Api_SecretKey))
				return new OperationResult<SchedulingStrategy>(OperationCode.Error_Sign);

			var ss = GetSchedulingStrategyByMicrogridId(microgridId);
			if (ss != null)
				return new OperationResult<SchedulingStrategy>(OperationCode.Success, ss);
			else
				return new OperationResult<SchedulingStrategy>(OperationCode.Error_NoSchedulingStrategy);
		}

		/// <summary>
		/// 修改调度策略
		/// </summary>
		/// <returns>OperationResult实例</returns>
		public OperationResult UpdateSchedulingStrategy(string api_account, long timeStamp, string sign, Guid microgridId, decimal pGridMax, decimal pOutputMax, decimal dieselStartSOC, decimal dieselPOutputMax,
			decimal dieselStopSOC, decimal dieselStopPower, decimal soc1, decimal soc2, decimal soc3, decimal soc4, decimal soc5, decimal soc6, decimal soc7,
			decimal power1, decimal power2, decimal power3, decimal power4, decimal power5, int chargingStart1, int chargingEnd1, int chargingStart2,
			int chargingEnd2, int dischargeStart1, int dischargeEnd1, int dischargeStart2, int dischargeEnd2, decimal chargingSOCPoint1, decimal chargingSOCPoint2,
			decimal dischargeSOCPoint1, decimal dischargeSOCPoint2)
		{
			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult(OperationCode.Error_TimeStamp);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForUpdateSchedulingStrategy(api_account, timeStamp, sign, microgridId, pGridMax, pOutputMax, dieselStartSOC, dieselPOutputMax,
																	dieselStopSOC, dieselStopPower, soc1, soc2, soc3, soc4, soc5, soc6, soc7,
																	power1, power2, power3, power4, power5, chargingStart1, chargingEnd1, chargingStart2,
																	chargingEnd2, dischargeStart1, dischargeEnd1, dischargeStart2, dischargeEnd2, chargingSOCPoint1, chargingSOCPoint2,
																	dischargeSOCPoint1, dischargeSOCPoint2, apiAccount.Api_SecretKey))
				return new OperationResult(OperationCode.Error_Sign);

			if (UpdateSchedulingStrategy(microgridId, pGridMax, pOutputMax, dieselStartSOC, dieselPOutputMax,
																	dieselStopSOC, dieselStopPower, soc1, soc2, soc3, soc4, soc5, soc6, soc7,
																	power1, power2, power3, power4, power5, chargingStart1, chargingEnd1, chargingStart2,
																	chargingEnd2, dischargeStart1, dischargeEnd1, dischargeStart2, dischargeEnd2, chargingSOCPoint1, chargingSOCPoint2,
																	dischargeSOCPoint1, dischargeSOCPoint2))
				return new OperationResult(OperationCode.Success);
			else
				return new OperationResult(OperationCode.Error_UpdateSchedulingStrategyFailed);
		}

		/// <summary>
		/// 获取微网汇总信息
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="microgridId">微网编号</param>
		/// <returns>OperationResult&lt;MicrogridSummary&gt;</returns>
		public OperationResult<MicrogridSummary> GetMicrogridSummary(string api_account, long timeStamp, string sign, Guid microgridId)
		{
			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult<MicrogridSummary>(OperationCode.Error_TimeStamp);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult<MicrogridSummary>(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForGetMicrogridSummary(api_account, timeStamp, sign, microgridId, apiAccount.Api_SecretKey))
				return new OperationResult<MicrogridSummary>(OperationCode.Error_Sign);

			var ms = GetMicrogridSummary(microgridId);
			if (ms != null)
				return new OperationResult<MicrogridSummary>(OperationCode.Success, ms);
			else
				return new OperationResult<MicrogridSummary>(OperationCode.Error_NoMicrogridSummary);
		}

		/// <summary>
		/// 更新微网状态
		/// </summary>
		/// <param name="microgridId">微网id</param>
		/// <param name="command">指令</param>
		/// <returns>成功:true/失败:false</returns>
		public bool ChangeMicrogridStateCommand(Guid microgridId, int command)
		{
			return _alphaRemotingService.SendCommand(string.Empty, "Microgrid", new object[] { command, microgridId });
		}

		/// <summary>
		/// 更新微网状态
		/// </summary>
		/// <param name="api_account">api账号</param>
		/// <param name="timeStamp">时间戳</param>
		/// <param name="sign">签名</param>
		/// <param name="microgridId">微网编号</param>
		/// <param name="command">指令</param>
		/// <returns>OperationResult实例</returns>
		public OperationResult ChangeMicrogridStateCommand(string api_account, long timeStamp, string sign, Guid microgridId, int command)
		{
			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult(OperationCode.Error_TimeStamp);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForChangeMicrogridStateCommand(api_account, timeStamp, sign, microgridId, command, apiAccount.Api_SecretKey))
				return new OperationResult(OperationCode.Error_Sign);

			var result = ChangeMicrogridStateCommand(microgridId, command);
			return new OperationResult((result ? OperationCode.Success : OperationCode.Error_SendCommandFailed));
		}

		/// <summary>
		/// 发送电力调度信息
		/// </summary>
		/// <param name="microgridId">微网编号</param>
		/// <param name="cmdIndex">命令识别码</param>
		/// <param name="controlPower">控制功率</param>
		/// <returns>成功:true/失败:false</returns>
		public bool SendPowerDispatchingCommand(Guid microgridId, string cmdIndex, decimal controlPower)
		{
			return _alphaRemotingService.SendCommand(string.Empty, "DispatchPower", new object[] { cmdIndex, microgridId, controlPower });
		}

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
		public OperationResult SendPowerDispatchingCommand(string api_account, long timeStamp, string sign, Guid microgridId, string cmdIndex, decimal controlPower)
		{
			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult(OperationCode.Error_TimeStamp);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForSendPowerDispatchingCommand(api_account, timeStamp, sign, microgridId, cmdIndex, controlPower, apiAccount.Api_SecretKey))
				return new OperationResult(OperationCode.Error_Sign);

			var result = SendPowerDispatchingCommand(microgridId, cmdIndex, controlPower);
			return new OperationResult((result ? OperationCode.Success : OperationCode.Error_SendCommandFailed));
		}
	}
}
