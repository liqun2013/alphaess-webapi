using AlphaEss.Api_V2.Infrastructure;
using AlphaEssWeb.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Model.Dtos;
using AlphaEssWeb.Api_V2.Model.ExternalResponseModels;
using System.Collections.Generic;

namespace AlphaEssWeb.Api_V2.Model
{
	public static class OperationResultExtensions
	{
		internal static ExternalRegisterResponseModel ToExternalRegisterResponseModel(this OperationResult or)
		{
			return new ExternalRegisterResponseModel() { ReturnCode = or.ReturnCode };
		}

		internal static ExternalLoginResponseModel ToExternalLoginResponseModel(this OperationResult<UserContext> or)
		{
			var result = new ExternalLoginResponseModel() { ReturnCode = or.ReturnCode };
			if (or.Entity != null)
			{
				result.Token = or.Entity.Token.ToString();
				result.UserType = or.Entity.UserType;
			}
			return result;
		}

		internal static ExternalChangeUserInfoResponseModel ToExternalChangeUserInfoResponseModel(this OperationResult or)
		{
			return new ExternalChangeUserInfoResponseModel() { ReturnCode = or.ReturnCode };
		}

		internal static ExternalChangePasswordResponseModel ToExternalChangePasswordResponseModel(this OperationResult or)
		{
			return new ExternalChangePasswordResponseModel() { ReturnCode = or.ReturnCode };
		}

		internal static ExternalRetrievePwdResponseModel ToExternalRetrievePwdResponseModel(this OperationResult or)
		{
			return new ExternalRetrievePwdResponseModel() { ReturnCode = or.ReturnCode };
		}

		internal static ExternalRegisterResponseModel ToExternalRegisterResponseModel(this OperationResult<UserWithRoles> op)
		{
			return new ExternalRegisterResponseModel() { ReturnCode = op.ReturnCode };
		}

		internal static ExternalGetUserAgreementByLanguageCodeResponseModel ToExternalGetUserAgreementByLanguageCodeResponseModel(this OperationResult<SYS_USERAGREEMENT> op)
		{
			var result = new ExternalGetUserAgreementByLanguageCodeResponseModel() { ReturnCode = op.ReturnCode };
			if (op.Entity != null)
			{
				result.Result = op.Entity.ToSysUserAgreementDto();
			}

			return result;
		}

		internal static ExternalRegisterResponseModel<SysUserAgreementDto> ToExternalRegisterResponseModel(this OperationResult<SYS_USERAGREEMENT> op)
		{
			var result = new ExternalRegisterResponseModel<SysUserAgreementDto>() { ReturnCode = op.ReturnCode };
			if (op.Entity != null)
			{
				result.Result = op.Entity.ToSysUserAgreementDto();
			}

			return result;
		}

		internal static ExternalQuerySystemByUserResponseModel<PaginatedDto<VtSystemDto>> ToExternalQuerySystemByUserResponseModel(this OperationResult<PaginatedList<VT_SYSTEM>> op)
		{
			var result = new ExternalQuerySystemByUserResponseModel<PaginatedDto<VtSystemDto>>() { ReturnCode = op.ReturnCode };
			if (op.Entity != null)
				result.Result = op.Entity.ToPaginatedListVtSystemDto();

			return result;
    }

		internal static ExternalGetPowerDataResponseModel ToExternalQueryPowerDataResponseModel(this OperationResult<PowerReportData> or)
		{
			var result = new ExternalGetPowerDataResponseModel() { ReturnCode = or.ReturnCode };
			if (or.Entity != null)
				result.Result = or.Entity.ToPowerDataDto();

			return result;
    }

		internal static ExternalGetEnergeDataResponseModel ToExternalGetEnergeDataResponseModel(this OperationResult<EnergyReportData> or)
		{
			var result = new ExternalGetEnergeDataResponseModel() { ReturnCode = or.ReturnCode };
			if (or.Entity != null)
				result.Result = or.Entity.ToEnergyReportDataDto();

			return result;
		}

		internal static ExternalGetProfitDataResponseModel ToExternalGetProfitDataResponseModel(this OperationResult<ProfitReportData> or)
		{
			var result = new ExternalGetProfitDataResponseModel() { ReturnCode = or.ReturnCode };
			if (or.Entity != null)
				result.Result = or.Entity.ToProfitReportDataDto();

			return result;
		}

		internal static ExternalGetEnergySummaryResponseModel ToExternalGetEnergySummaryResponseModel(this OperationResult<PaginatedList<Report_Energy>> or)
		{
			var result = new ExternalGetEnergySummaryResponseModel() { ReturnCode = or.ReturnCode };
			if (or.Entity != null)
				result.Result = or.Entity.ToPaginatedListReportEnergyDto();

			return result;
		}

		internal static ExternalQuerySystemStatusResponseModel ToExternalGetSystemStatusResponseModel(this OperationResult<IEnumerable<SystemState>> or)
		{
			var result = new ExternalQuerySystemStatusResponseModel() { ReturnCode = or.ReturnCode };
			if (or.Entity != null)
				result.Result = or.Entity.ToCollectionSystemStateDto();

			return result;
		}

		internal static ExternalGetCompanyContactsResponseModel ToExternalGetCompanyContactsResponseModel(this OperationResult<IEnumerable<CompanyContactDetail>> or)
		{
			var result = new ExternalGetCompanyContactsResponseModel() { ReturnCode = or.ReturnCode };
			if (or.Entity != null)
				result.Result = or.Entity.ToCollectionCompanyContactDetailDto();

			return result;
		}
		internal static ExternalUserinfoResponseModel ToExternalUserinfoResponseModel(this OperationResult<UserWithRoles> or)
		{
			var result = new ExternalUserinfoResponseModel() { ReturnCode = or.ReturnCode };
			if (or.Entity != null)
				result.Result = or.Entity.ToSysUserDto();

			return result;
    }

		internal static ExternalUserinfoResponseModel ToExternalUserinfoResponseModel(this OperationResult<UserWithRolesAndSystems> urs)
		{
			var result = new ExternalUserinfoResponseModel() { ReturnCode = urs.ReturnCode };
			if (urs.Entity != null)
				result.Result = urs.Entity.ToSysUserDto();

			return result;
    }

		internal static ExternalTheLastAppClientVersionResponseModel ToExternalTheLastAppClientVersionResponseModel(this OperationResult<APP_Version> orv)
		{
			var result = new ExternalTheLastAppClientVersionResponseModel { ReturnCode = orv.ReturnCode };
			if (orv.Entity != null)
				result.Result = orv.Entity.ToAppVersionDto();

			return result;
		}

		internal static ExternalGetMicrogridInfoResponseModel ToExternalGetMicrogridInfoResponseModel(this OperationResult<MicrogridInfo> or)
		{
			var result = new ExternalGetMicrogridInfoResponseModel { ReturnCode = or.ReturnCode };
			if (or.Entity != null)
				result.Result = or.Entity.ToMicrogridInfoDto();

			return result;
		}

		internal static ExternalGetMicrogridSchedulingStrategyResponseModel ToExternalGetMicrogridSchedulingStrategyResponseModel(this OperationResult<SchedulingStrategy> or)
		{
			var result = new ExternalGetMicrogridSchedulingStrategyResponseModel { ReturnCode = or.ReturnCode };
			if (or.Entity != null)
				result.Result = or.Entity.ToSchedulingStrategyDto();

			return result;
		}

		internal static ExternalUpdateSchedulingStrategyResponseModel ToExternalUpdateSchedulingStrategyResponseModel(this OperationResult or)
		{
			return new ExternalUpdateSchedulingStrategyResponseModel { ReturnCode = or.ReturnCode };
		}

		internal static ExternalGetMicrogridSummaryResponseModel ToExternalGetMicrogridSummaryResponseModel(this OperationResult<MicrogridSummary> or)
		{
			var result = new ExternalGetMicrogridSummaryResponseModel { ReturnCode = or.ReturnCode };
			if (or.Entity != null)
				result.Result = or.Entity.ToMicrogridSummaryDto();

			return result;
		}

		internal static ExternalMicrogridControlCommandResponseModel ToExternalMicrogridControlCommandResponseModel(this OperationResult or)
		{
			return new ExternalMicrogridControlCommandResponseModel { ReturnCode = or.ReturnCode };
		}

		internal static ExternalElectricDispatchingControlRecordResponseModel ToExternalElectricDispatchingControlRecordResponseModel(this OperationResult or)
		{
			return new ExternalElectricDispatchingControlRecordResponseModel { ReturnCode = or.ReturnCode };
		}

		//internal static ExternalGetDeviceInfoResponseModel ToExternalDeviceInfoResponseModel(this OperationResult<DeviceInfo> urs)
		//{
		//	var result = new ExternalGetDeviceInfoResponseModel() { ReturnCode = urs.ReturnCode };
		//	if (urs.Entity != null)
		//	{
		//		result.Status = urs.Entity.Status;
		//		result.DeviceID = urs.Entity.DeviceID;
		//		result.Voltage = urs.Entity.Voltage;
		//		result.TimeZone = urs.Entity.TimeZone;
		//		result.UpdateTime = urs.Entity.UpdateTime;
		//	}

		//	return result;
		//}

		//internal static ExternalGetDeviceCVResponseModel ToExternalDeviceCVResponseModel(this OperationResult<DeviceCV> urs)
		//{
		//	var result = new ExternalGetDeviceCVResponseModel() { ReturnCode = urs.ReturnCode };
		//	if (urs.Entity != null)
		//	{
		//		result.Status = urs.Entity.Status;
		//		result.Result = urs.Entity.ToDeviceCVDto();
		//	}

		//	return result;
		//}

		internal static ExternalVtColDataResponseModel<PaginatedDto<VtColDataDto>> ToExternaRunningDataResponseModel(this OperationResult<PaginatedList<VT_COLDATA>> op)
		{
			var result = new ExternalVtColDataResponseModel<PaginatedDto<VtColDataDto>>() { ReturnCode = op.ReturnCode };
			if (op.Entity != null && op.Entity.TotalCount > 0)
				result.Result = op.Entity.ToPaginatedRunningDataDto();

			return result;
		}

		internal static ExternalRemoteDispatchResponseModel ToExternaRemoteDispatchResponseModel(this OperationResult<Sys_RemoteDispatch> rd)
		{
			return new ExternalRemoteDispatchResponseModel { ReturnCode = rd.ReturnCode };
		}

		internal static ExternalGetSystemDetailResponseModel ToExternalGetSystemDetailResponseModel(this OperationResult<VT_SYSTEM> resp)
		{
			return new ExternalGetSystemDetailResponseModel() { ReturnCode = resp.ReturnCode, Result = resp.Entity != null ? resp.Entity.ToVTSYSTEMDto() : null };
		}

		internal static ExternalSetSystemResponseModel ToExternalSetSystemResponseModel(this OperationResult resp)
		{
			return new ExternalSetSystemResponseModel() { ReturnCode = resp.ReturnCode };
		}

		internal static ExternalBindNewSystemResponseModel ToExternalBindNewSystemResponseModel(this OperationResult resp)
		{
			return new ExternalBindNewSystemResponseModel() { ReturnCode = resp.ReturnCode };
		}

		internal static ExternalInstallNewSystemResponseModel ToExternalInstallNewSystemResponseModel(this OperationResult resp)
		{
			return new ExternalInstallNewSystemResponseModel() { ReturnCode = resp.ReturnCode };
		}
		internal static ExternalAddNewComplaintsResponseModel ToExternalAddNewComplaintsResponseModel(this OperationResult resp)
		{
			return new ExternalAddNewComplaintsResponseModel() { ReturnCode = resp.ReturnCode };
		}
		internal static ExternalGetComplaintsListResponseModel<PaginatedDto<ComplaintsDto>> ToExternalGetComplaintsListResponseModel(this OperationResult<PaginatedList<Complaints>> resp)
		{
			var result = new ExternalGetComplaintsListResponseModel<PaginatedDto<ComplaintsDto>>() { ReturnCode = resp.ReturnCode };
			if (resp.Entity != null && resp.Entity.TotalCount > 0)
				result.Result = resp.Entity.ToPaginatedComplaintsDto();

			return result;
		}
		internal static ExternalEvaluateComplaintsResponseModel ToExternalEvaluateComplaintsResponseModel(this OperationResult resp)
		{
			return new ExternalEvaluateComplaintsResponseModel() { ReturnCode = resp.ReturnCode };
		}
		internal static ExternalGetMsgResponseModel<PaginatedDto<SysMsgDto>> ToExternalGetMsgResponseModel(this OperationResult<PaginatedList<SYS_MSGUSER>> resp)
		{
			var result = new ExternalGetMsgResponseModel<PaginatedDto<SysMsgDto>>() { ReturnCode = resp.ReturnCode };
			if (resp.Entity != null && resp.Entity.TotalCount > 0)
				result.Result = resp.Entity.ToPaginatedMsgDto();

			return result;
		}

		internal static ExternalGetFirmwareUpdateResponseModel<FirmwareVersionData> ToExternalGetFirmwareUpdateResponsModel(this OperationResult<FirmwareVersionData> resp)
		{
			return new ExternalGetFirmwareUpdateResponseModel<FirmwareVersionData> { ReturnCode = resp.ReturnCode, Result = resp.Entity };
		}
		internal static ExternalUpdateSystemFirmwareResponseModel ToExternalUpdateSystemFirmwareResponseModel(this OperationResult resp)
		{
			return new ExternalUpdateSystemFirmwareResponseModel() { ReturnCode = resp.ReturnCode };
		}
		internal static ExternalUpdateMsgFlagResponseModel ToExternalUpdateMsgFlagResponseModel(this OperationResult resp)
		{
			return new ExternalUpdateMsgFlagResponseModel() { ReturnCode = resp.ReturnCode };
		}
		internal static ExternalGetSystemSummaryStatisticsDataResponsModel<SystemSummaryStatisticsData> ToExternalGetSystemSummaryStatisticsDataResponsModel(this OperationResult<SystemSummaryStatisticsData> resp)
		{
			return new ExternalGetSystemSummaryStatisticsDataResponsModel<SystemSummaryStatisticsData> { ReturnCode = resp.ReturnCode, Result = resp.Entity };
		}
	}
}
