using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoFixtureDemo.Tests
{
   public class AutoMoqDataAttribute : AutoDataAttribute
    {
        private readonly string[] _setupNames;

        private const string AutoSetupName = "AutoSetup";

        public AutoMoqDataAttribute(params string[] setupNames)
            : base(CreateFixture)
        {
            _setupNames = setupNames;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            if (testMethod.DeclaringType != null)
            {
                var fields = testMethod.DeclaringType.GetProperties();

                RunAutoSetup(fields);

                RunNamedSetups(fields);
            }

            return base.GetData(testMethod);
        }

        public static Fixture CreateFixture()
        {
            var fixture = new Fixture();

            fixture.Customize(new AutoMoqCustomization() { GenerateDelegates = true });

            fixture.Register(() => fixture);

            return fixture;
        }

        private void RunNamedSetups(PropertyInfo[] fields)
        {
            if (_setupNames == null) return;
            foreach (var setupName in _setupNames)
            {
                var namedSetup =
                    fields.SingleOrDefault(x => x.PropertyType == typeof(Action<IFixture>) && x.Name == setupName)
                        ?.GetValue(null) as Action<IFixture>;

                namedSetup?.Invoke(Fixture);
            }
        }

        private void RunAutoSetup(PropertyInfo[] fields)
        {
            var autoSetup =
                fields.SingleOrDefault(x => x.PropertyType == typeof(Action<IFixture>) && x.Name == AutoSetupName)
                    ?.GetValue(null) as Action<IFixture>;

            autoSetup?.Invoke(Fixture);
        }
    }
}
