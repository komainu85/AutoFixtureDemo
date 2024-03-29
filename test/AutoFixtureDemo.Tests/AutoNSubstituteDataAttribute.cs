﻿using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoFixtureDemo.Tests
{
    public sealed class AutoNSubstituteDataAttribute : AutoDataAttribute
    {
        private readonly string _setupName;
        private const string AutoSetupName = "AutoSetup";

        public AutoNSubstituteDataAttribute()
            : base(() => CreateFixture())
        {
        }

        public AutoNSubstituteDataAttribute(string setupName)
            : base(() => CreateFixture())
        {
            _setupName = setupName;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var fields = testMethod.DeclaringType.GetProperties();

            var autoSetup = fields.SingleOrDefault(x => x.PropertyType == typeof(Action<IFixture>) && x.Name == AutoSetupName)?.GetValue(null) as Action<IFixture>;
            var namedSetup = fields.SingleOrDefault(x => x.PropertyType == typeof(Action<IFixture>) && x.Name == _setupName)?.GetValue(null) as Action<IFixture>;

            autoSetup?.Invoke(Fixture);
            namedSetup?.Invoke(Fixture);

            return base.GetData(testMethod);
        }

        public static Fixture CreateFixture()
        {
            var fixture = new Fixture();

            fixture.Customize(new AutoNSubstituteCustomization() { GenerateDelegates = true });

            return fixture;
        }
    }
}
