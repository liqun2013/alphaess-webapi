using AlphaEssWeb.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Model.Dtos;

namespace AlphaEssWeb.Api_V2.Model
{
	internal static class PowerDataExtensions
	{
		public static PowerDataDto ToPowerDataDto(this PowerData pd)
		{
			return new PowerDataDto
			{
				Id = pd.Key, Pbat = pd.Pbat, PmeterDc = pd.PmeterDc, PmeterL1 = pd.PmeterL1, PmeterL2 = pd.PmeterL2, PmeterL3 = pd.PmeterL3, Ppv1 = pd.Ppv1, Ppv2 = pd.Ppv2,
				PrealL1 = pd.PrealL1, PrealL2 = pd.PrealL2, PrealL3 = pd.PrealL3, Pva = pd.Sva, Sn = pd.Sn, UploadTime = pd.UploadTime, VarAC = pd.VarAC, VarDC = pd.VarDC
			};
		}

		public static PowerReportDataDto ToPowerDataDto(this PowerReportData pd)
		{
			return new PowerReportDataDto
			{
				Cbat = pd.Cbat,
				EBat = pd.EBat,
				ECharge = pd.ECharge,
				EFeedIn = pd.EFeedIn,
				EGridCharge = pd.EGridCharge,
				ELoad = pd.ELoad,
				EPvTotal = pd.EPvTotal,
				FeedIn = pd.FeedIn,
				GridCharge = pd.GridCharge,
				LastIndex = pd.LastIndex,
				Ppv = pd.Ppv,
				Time = pd.Time,
				UsePower = pd.UsePower
			};
		}
	}
}
