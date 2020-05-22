using AutoFixtureDemo.Models;
using System;
using System.Collections.Generic;

namespace AutoFixtureDemo.Services
{
    public interface IWeatherService
    {
        IEnumerable<Weather> GetHourlyWeather(Guid cityId);
    }
}