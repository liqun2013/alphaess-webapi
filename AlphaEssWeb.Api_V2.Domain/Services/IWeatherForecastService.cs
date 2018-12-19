using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Collections.Generic;

namespace AlphaEssWeb.Api_V2.Domain.Services
{
	public interface IWeatherForecastService
	{
		List<SysWeatherForecast> GetRecentThreeDaysSysWeatherForecastBySn(string sn);
	}
}
