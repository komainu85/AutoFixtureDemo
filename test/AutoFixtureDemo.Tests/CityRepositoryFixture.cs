using AutoFixture;
using AutoFixture.Xunit2;
using AutoFixtureDemo.Database;
using AutoFixtureDemo.Models;
using AutoFixtureDemo.Services;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AutoFixtureDemo.Tests
{
    public class CityRepositoryFixture
    {
        public static Action<IFixture> AutoSetup => f =>
        {
            //f.Customizations.Add(new MethodInvoker(new GreedyConstructorQuery()));

            //// Customise how Country is created by setting a property /
            // f.Customize<Country>(x => x.With(p => p.Name, "United Kingdom"));

            //// Customise how country is created by specifying an action to run.
            //f.Customize<Country>(x => x.Do(a => a.States.Add("My State")));

            //// Factory method to use to create an instance
            //f.Customize<Country>(x => x.FromFactory(() => new Country()));

            //// Creates an instance of a type.
            // var testString = f.Create<string>();

            //// You need to inject a type that you have created so AutoFixture can use it.
            //// When you ask for an instance of a type you will always get the one you have injected.
            // f.Inject(testString);

            //// Create a enumerable of generated instances of a type
            //var strings = f.CreateMany<string>();

            //// Creates a one off new object, ignores any customisations previously registered for the type.
            //var instance = f.Build<Country>().With(p => p.Id, Guid.NewGuid());
        };

        [Theory]
        [AutoNSubstituteData]
        public async Task SaveCity_WithValidData_CallsSaveAsync(
            [Frozen] IDatabaseProvider databaseProvider,
            CityRepository sut,
            string cityName,
            Country country)
        {
            // Act
            await sut.SaveCity(cityName, country);

            // Assert
            await databaseProvider.Received(1).SaveAsync();
        }

        //Using Fixture within a test
        [Theory]
        [AutoNSubstituteData]
        public async Task SaveCity_WithValidData_CallsAddCity(
            [Frozen] IDatabaseProvider databaseProvider,
            CityRepository sut,
            string cityName,
            Fixture fixture)
        {
            //Arrange
            var country = fixture.Create<Country>();

            //Act
            await sut.SaveCity(cityName, country);

            // Assert
            databaseProvider.Received(1).AddCity(Arg.Is<string>(m => m.Equals(cityName)), Arg.Is<Country>(m => m.Equals(country)));
        }
    }
}
