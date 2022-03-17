using AutoFixtureDemo.Models;
using AutoFixtureDemo.Services;
using System;

namespace AutoFixtureDemo
{
    public class CityWeatherSearchService
    {
        private readonly IWeatherService _weatherService;
        private readonly ICityRepository _cityRepository;

        public CityWeatherSearchService(
            IWeatherService weatherService,
            ICityRepository cityRepository)
        {
            _weatherService = weatherService ?? throw new ArgumentNullException(nameof(weatherService));
            _cityRepository = cityRepository ?? throw new ArgumentNullException(nameof(cityRepository));
        }

        public CityWeather GetCityWeather(string cityName)
        {
            if (string.IsNullOrEmpty(cityName))
                throw new ArgumentNullException(nameof(cityName));

            var city = _cityRepository.GetCity(cityName);

            if (city != null)
            {
                var hourlyWeather = _weatherService.GetHourlyWeather(city.Id);

                return new CityWeather(city, hourlyWeather);
            }

            throw new Exception("Not Found"); //Crap, but fine for demo
        }
    }
}
