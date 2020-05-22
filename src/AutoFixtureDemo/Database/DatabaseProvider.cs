using AutoFixtureDemo.Models;
using System.Threading.Tasks;

namespace AutoFixtureDemo.Database
{
    public class DatabaseProvider : IDatabaseProvider
    {
        public void AddCity(string name, Country country)
        {
            //Do DB Stuff
        }

        public async Task SaveAsync()
        {

        }
    }
}
