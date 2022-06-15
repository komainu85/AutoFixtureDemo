using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using AutoFixtureDemo.Models;
using AutoFixtureDemo.Services;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AutoFixtureDemo.Tests
{
    public class CityWeatherSearchServiceFixture
    {
        public static Action<IFixture> AutoSetup => f =>
        {
            var cityRepository = f.Freeze<ICityRepository>();

            cityRepository.GetCity(Arg.Any<string>())
                .Returns(f.Create<City>());

            f.Inject(cityRepository);
        };

        public static Action<IFixture> NoWeatherData => f =>
        {
            var weatherService = f.Freeze<IWeatherService>();

            weatherService.GetHourlyWeather(Arg.Any<Guid>())
                .Returns(Enumerable.Empty<Weather>());

            f.Inject(weatherService);
        };

        public static Action<IFixture> FullDayOfWeatherData => f =>
        {
            var weather = new List<Weather>();

            for (var i = 0; i < 23; i++)
            {
                weather.Add(new Weather(DateTime.Now.AddHours(i), 12.2f));
            }

            var weatherService = f.Freeze<IWeatherService>();

            weatherService.GetHourlyWeather(Arg.Any<Guid>())
                .Returns(weather);

            f.Inject(weatherService);
            f.Inject<IEnumerable<Weather>>(weather);
        };

        [Theory]
        [AutoNSubstituteData]
        public void Ctor_Guarded(GuardClauseAssertion assertion)
        {
            // Act / Assert
            assertion.Verify(typeof(CityWeatherSearchService).GetConstructors());
        }

        [Theory]
        [AutoNSubstituteData]
        public void Methods_Guarded(GuardClauseAssertion assertion)
        {
            // Arrange
            assertion.Verify(typeof(CityWeatherSearchService).GetMethods());
        }

        // Simple object creation
        [Theory]
        [AutoNSubstituteData]
        public void GetCityWeather_WithValidCityName_DoesntThrow(
            CityWeatherSearchService sut,
            string cityName)
        {
            // Act
            Func<CityWeather> act = () => sut.GetCityWeather(cityName);

            act.Should().NotThrow<ArgumentNullException>();
        }

        // Frozen intro / AutoSetup
        [Theory]
        [AutoNSubstituteData]
        public void GetCityWeather_WithValidCityName_CallsRepository(
            [Frozen] ICityRepository cityRepository,
            CityWeatherSearchService sut,
            string cityName)
        {
            // Act
            sut.GetCityWeather(cityName);

            cityRepository.Received(1).GetCity(Arg.Is<string>(m => m == cityName));
        }

        // complex object creation
        [Theory]
        [AutoNSubstituteData]
        public void GetCityWeather_WithMatchingResults_ReturnsResults(
           [Frozen] ICityRepository cityRepository,
           [Frozen] IWeatherService weatherService,
           CityWeatherSearchService sut,
           City city,
           IEnumerable<Weather> weather,
           string cityName)
        {
            //Arrange 
            cityRepository.GetCity(Arg.Is<string>(m => m == cityName))
                .Returns(city);

            weatherService.GetHourlyWeather(Arg.Is<Guid>(m => m == city.Id))
              .Returns(weather);

            // Act
            var cityWeather = sut.GetCityWeather(cityName);

            // Assert
            cityRepository.Received(1).GetCity(Arg.Is<string>(m => m == cityName));
            weatherService.Received(1).GetHourlyWeather(Arg.Is<Guid>(m => m == city.Id));

            cityWeather.City.Should().Be(city);
            cityWeather.HourlyWeather.Should().BeSameAs(weather);
        }

        // Named setup
        [Theory]
        [AutoNSubstituteData(nameof(NoWeatherData))]
        public void SearchAsync_WithNoWeatherResults_NoWeatherReturned(
              CityWeatherSearchService sut,
              string cityName)
        {
            // Act
            var weather = sut.GetCityWeather(cityName);

            weather.HourlyWeather.Should().BeEmpty();
            weather.City.Should().NotBeNull();
        }

        [Theory]
        [AutoNSubstituteData(nameof(FullDayOfWeatherData))]
        public void SearchAsync_WithWeatherResults_WeatherReturned(
            CityWeatherSearchService sut,
            IEnumerable<Weather> weather,
            string cityName)
        {
            // Act
            var cityWeather = sut.GetCityWeather(cityName);

            cityWeather.HourlyWeather.Should().BeSameAs(weather);
            cityWeather.City.Should().NotBeNull();
        }
    }
}
