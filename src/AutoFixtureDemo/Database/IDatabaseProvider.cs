using AutoFixtureDemo.Models;
using System.Threading.Tasks;

namespace AutoFixtureDemo.Database
{
    public interface IDatabaseProvider
    {
        void AddCity(string name, Country country);
        Task SaveAsync();
    }
}
