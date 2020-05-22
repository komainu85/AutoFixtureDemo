using AutoFixtureDemo.Models;

namespace AutoFixtureDemo.Services
{
    public interface ICityRepository
    {
        City GetCity(string cityName);
    }
}