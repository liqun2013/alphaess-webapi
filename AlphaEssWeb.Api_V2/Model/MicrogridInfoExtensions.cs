using AlphaEssWeb.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Model.Dtos;

namespace AlphaEssWeb.Api_V2.Model
{
	internal static class MicrogridInfoExtensions
	{
		public static MicrogridInfoDto ToMicrogridInfoDto(this MicrogridInfo mi)
		{
			return new MicrogridInfoDto
			{
				BatteryModel = mi.BatteryModel,
				CustomerAddress = mi.CustomerAddress,
				CustomerCity = mi.CustomerCity,
				CustomerContact = mi.CustomerContact,
				CustomerCountry = mi.CustomerCountry,
				CustomerEmail = mi.CustomerEmail,
				CustomerName = mi.CustomerName,
				CustomerPhone = mi.CustomerPhone,
				CustomerState = mi.CustomerState,
				DEInstalledPower = mi.DEInstalledPower,
				Id = mi.Key,
				MaxGridPower = mi.MaxGridPower,
				MaxPEPower = mi.MaxPEPower,
				MicrogridName = mi.MicrogridName,
				NumberOfCells = mi.NumberOfCells,
				OPGInstalledPower = mi.OPGInstalledPower,
				PVPower = mi.PVPower,
				RunningState = mi.RunningState,
				SetUpTime = mi.SetUpTime,
				SystemInstalledCount = mi.SystemInstalledCount,
				SystemModel = mi.SystemModel,
				SystemTimeSpan = mi.SystemTimeSpan,
				SystemTotalPower = mi.SystemTotalPower,
				TotalInstalledCapacity = mi.TotalInstalledCapacity,
				WPInstalledPower = mi.WPInstalledPower
			};
		}
	}
}
