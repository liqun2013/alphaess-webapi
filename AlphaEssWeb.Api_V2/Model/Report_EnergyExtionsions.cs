using AlphaEssWeb.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Model.Dtos;

namespace AlphaEssWeb.Api_V2.Model
{
	internal static class Report_EnergyExtionsions
	{
		public static ReportEnergyDto ToReportEnergDto(this Report_Energy ed)
		{
			return new ReportEnergyDto
			{
				Ebat = ed.Ebat ?? 0,
				Echarge = ed.Echarge ?? 0,
				Eeff = ed.Eeff ?? 0,
				EGrid2Load = ed.EGrid2Load ?? 0,
				EGridCharge = ed.EGridCharge ?? 0,
				Einput = ed.Einput ?? 0,
				Eload = ed.Eload ?? 0,
				Eoutput = ed.Eoutput ?? 0,
				Epv2Load = ed.Epv2Load ?? 0,
				Epvtotal = ed.Epvtotal ?? 0,
				EselfConsumption = ed.EselfConsumption ?? 0,
				EselfSufficiency = ed.EselfSufficiency ?? 0,
				TotalIncome = ed.TotalIncome ?? 0
			};
		}

		public static EnergyReportDataDto ToEnergyReportDataDto(this EnergyReportData ed)
		{
			return new EnergyReportDataDto
			{
				Ebat = ed.Ebat, Echarge = ed.Echarge, Eeff = ed.Eeff, EGrid2Load = ed.EGrid2Load,
				EGridCharge = ed.EGridCharge, Einput = ed.Einput, Eload = ed.Eload, Eout = ed.Eout,
				Epv2load = ed.Epv2load, EpvT = ed.EpvT, EselfConsumption = ed.EselfConsumption, EselfSufficiency = ed.EselfSufficiency, Timeline = ed.Timeline
			};
		}
	}
}
