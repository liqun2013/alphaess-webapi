using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlphaEssWeb.Api_V2.Domain.Services
{
	public class WeatherForecastService : IWeatherForecastService
	{
		private readonly IEntityRepository<SysWeatherForecast, Guid> _weatherForecastRepository;

		public WeatherForecastService(IEntityRepository<SysWeatherForecast, Guid> weatherForecastRepository)
		{
			_weatherForecastRepository = weatherForecastRepository;
		}
		public List<SysWeatherForecast> GetRecentThreeDaysSysWeatherForecastBySn(string sn)
		{
			List<SysWeatherForecast> result = null;
			var today = DateTime.Now.Date;
			var q = _weatherForecastRepository.GetAll().Where(x => x.SYS_SN == sn && x.DATETIME >= today);
			if (q != null && q.Any())
			{
				result = q.OrderBy(x => x.DATETIME).ThenByDescending(x => x.CREATEDATE).Take(3).ToList();
			}

			return result;
		}
	}
}
