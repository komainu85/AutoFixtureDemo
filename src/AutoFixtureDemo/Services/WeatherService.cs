using AutoFixtureDemo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoFixtureDemo.Services
{
    public class WeatherService : IWeatherService
    {
        public IEnumerable<Weather> GetHourlyWeather(Guid cityId)
        {
            return new List<Weather>();
        }
    }
}
