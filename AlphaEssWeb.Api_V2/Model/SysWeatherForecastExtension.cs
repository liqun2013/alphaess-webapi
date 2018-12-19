using AlphaEssWeb.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Model.Dtos;

namespace AlphaEssWeb.Api_V2.Model
{
	internal static class SysWeatherForecastExtension
	{
		public static SysWeatherForecastDto ToSysWeatherForecastDto(this SysWeatherForecast w)
		{
			return new SysWeatherForecastDto
			{
				Id = w.Key, clouds = w.clouds, DATETIME = w.DATETIME, humidity = w.humidity, pressure = w.pressure, remark = w.remark,
				temp_max = w.temp_max.HasValue ? (int)w.temp_max.Value - 273 : 0, temp_min = w.temp_min.HasValue ? (int)w.temp_min.Value - 273 : 0, weather = w.weather, wind_speed = w.wind_speed
			};
		}
	}
}
