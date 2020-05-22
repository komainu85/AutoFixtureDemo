using System;
using System.Collections.Generic;

namespace AutoFixtureDemo.Models
{
    public class CityWeather
    {
        public CityWeather(City city, IEnumerable<Weather> hourlyWeather)
        {
            City = city ?? throw new ArgumentNullException(nameof(city));
            HourlyWeather = hourlyWeather?? throw new ArgumentNullException(nameof(hourlyWeather));
        }

        public City City { get;  }

        public IEnumerable<Weather> HourlyWeather { get;}
    }
}
