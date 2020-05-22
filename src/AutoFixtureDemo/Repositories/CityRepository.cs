using AutoFixtureDemo.Database;
using AutoFixtureDemo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AutoFixtureDemo.Services
{
    public class CityRepository : ICityRepository
    {
        private readonly IDatabaseProvider _databaseProvider;

        public CityRepository(IDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
        }


        public City GetCity(string cityName)
        {
            return new City() { Name = cityName };
        }

        public Task SaveCity(string cityName, Country country)
        {
            _databaseProvider.AddCity(cityName, country);

            return _databaseProvider.SaveAsync();
        }
    }
}
