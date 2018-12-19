using AlphaEssWeb.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Model.Dtos;

namespace AlphaEssWeb.Api_V1.Model
{
	internal static class EnergyDataExtionsions
	{
		public static EnergyReportDataDto ToEnergyReportDataDto(this EnergyReportData ed)
		{
			return new EnergyReportDataDto
			{
				Ebat = ed.Ebat,
				Echarge = ed.Echarge,
				Eeff = ed.Eeff,
				EGrid2Load = ed.EGrid2Load,
				EGridCharge = ed.EGridCharge,
				Einput = ed.Einput,
				Eload = ed.Eload,
				Eout = ed.Eout,
				Epv2load = ed.Epv2load,
				EpvT = ed.EpvT,
				EselfConsumption = ed.EselfConsumption,
				EselfSufficiency = ed.EselfSufficiency, Timeline = ed.Timeline
			};
		}
	}
}
