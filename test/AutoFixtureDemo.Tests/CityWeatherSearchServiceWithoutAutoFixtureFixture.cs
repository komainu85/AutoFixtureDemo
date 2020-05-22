using AutoFixtureDemo.Models;
using AutoFixtureDemo.Services;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AutoFixtureDemo.Tests
{
    public class CityWeatherSearchServiceWithoutAutoFixtureFixture
    {
        private readonly CityWeatherSearchService _sut;
        private readonly Mock<ICityRepository> _cityRepository;
        private readonly Mock<IWeatherService> _weatherService;

        public CityWeatherSearchServiceWithoutAutoFixtureFixture()
        {
            _cityRepository = new Mock<ICityRepository>();
            _weatherService = new Mock<IWeatherService>();

            _sut = new CityWeatherSearchService(_weatherService.Object, _cityRepository.Object);
        }

        [Fact]
        public void Ctor_NullWeatherService_Guarded()
        {
            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => new CityWeatherSearchService(null, null))
                .Message.Should().Be("Value cannot be null. (Parameter 'weatherService')");
        }

        [Fact]
        public void Ctor_NullCityService_Guarded()
        {
            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => new CityWeatherSearchService(new WeatherService(), null))
                .Message.Should().Be("Value cannot be null. (Parameter 'cityService')");
        }

        [Fact]
        public void GetCityWeather_Guarded()
        {
            // Arrange
            Assert.Throws<ArgumentNullException>(() => _sut.GetCityWeather(null))
                .Message.Should().Be("Value cannot be null. (Parameter 'cityName')");
        }

        [Fact]
        public void GetCityWeather_WithValidCityName_DoesntThrow()
        {
            //Arrange
            var cityName = "MyCity";

            // Act
            Func<CityWeather> act = () => _sut.GetCityWeather(cityName);

            act.Should().NotThrow<ArgumentNullException>();
        }

        [Fact]
        public void GetCityWeather_WithValidCityName_CallsRepository()
        {
            //Arrange
            var cityName = "MyCity";

            _cityRepository.Setup(x => x.GetCity(It.Is<string>(s => s.Equals(cityName))))
                .Returns(new City());

            // Act
            _sut.GetCityWeather(cityName);

            _cityRepository.Verify(v => v.GetCity(It.Is<string>(m => m == cityName)), Times.Once);
        }

        [Fact]
        public void GetCityWeather_WithMatchingResults_ReturnsResults()
        {
            //Arrange 
            var city = new City()
            {
                Name = "MyCity",
                Id = new Guid()
            };

            _cityRepository.Setup(s => s.GetCity(It.Is<string>(m => m == city.Name)))
                .Returns(city);

            var weather = new List<Weather>();

            for (var i = 0; i < 23; i++)
            {
                weather.Add(new Weather(DateTime.Now.AddHours(i), 12.2f));
            }

            _weatherService.Setup(s => s.GetHourlyWeather(It.Is<Guid>(m => m == city.Id)))
                .Returns(weather);

            // Act
            var cityWeather = _sut.GetCityWeather(city.Name);

            // Assert
            _cityRepository.Verify(v => v.GetCity(It.Is<string>(m => m == city.Name)), Times.Once);
            _weatherService.Verify(v => v.GetHourlyWeather(It.Is<Guid>(m => m == city.Id)));

            cityWeather.City.Should().Be(city);
            cityWeather.HourlyWeather.Should().BeSameAs(weather);
        }

        [Fact]
        public void SearchAsync_WitNoWeatherResults_NoWeatherReturned()
        {
            // Arrange
            var city = new City()
            {
                Name = "MyCity",
                Id = new Guid()
            };

            _cityRepository.Setup(s => s.GetCity(It.Is<string>(m => m == city.Name)))
                .Returns(city);

            _weatherService.Setup(c => c.GetHourlyWeather(It.IsAny<Guid>()))
                .Returns(Enumerable.Empty<Weather>());

            // Act
            var weather = _sut.GetCityWeather(city.Name);

            // Assert
            weather.HourlyWeather.Should().BeEmpty();
            weather.City.Should().NotBeNull();
        }

        [Fact]
        public void SearchAsync_WithWeatherResults_NoWeatherReturned()
        {
            // Arrange
            var city = new City()
            {
                Name = "MyCity",
                Id = new Guid()
            };

            var weather = new List<Weather>();

            for (var i = 0; i < 23; i++)
            {
                weather.Add(new Weather(DateTime.Now.AddHours(i), 12.2f));
            }

            _weatherService.Setup(c => c.GetHourlyWeather(It.IsAny<Guid>()))
                .Returns(weather);

            _cityRepository.Setup(s => s.GetCity(It.Is<string>(m => m == city.Name)))
                .Returns(city);

            // Act
            var cityWeather = _sut.GetCityWeather(city.Name);

            cityWeather.HourlyWeather.Should().BeSameAs(weather);
            cityWeather.City.Should().NotBeNull();
        }
    }
}
